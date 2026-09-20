using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using ObjectsReaderUI.Editing;
using RenData.Definitions;
using RenData.Packaging;
using RenData.SaveLoad;

namespace ObjectsReaderUI.ViewModels;

/// <summary>Which kind of file is currently loaded, so Save routes to the matching serializer.</summary>
public enum LoadedFileKind { Ddb, PackagesDat, Tpi }

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ChunkNodeViewModel> RootNodes { get; } = [];
    public RangeObservableCollection<PropertyRow> Properties { get; } = [];

    [ObservableProperty] private ChunkNodeViewModel? _selectedNode;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _windowTitle = "Objects.ddb Viewer";
    [ObservableProperty] private string _searchText = "";

    // Enables the Save menu items once a file is loaded.
    [ObservableProperty] private bool _hasFile;

    // The top-level chunks in original file order, kept so Save can round-trip them.
    private List<TopLevelChunk> _loadedChunks = [];

    // Packages loaded from a packages.dat / .tpi file (null when a .ddb is loaded).
    private List<PackageClass> _loadedPackages = [];

    // Maps each loaded definition to its tree node, so a click-through reference can
    // select the target definition. Keyed by reference identity (DefinitionClass doesn't
    // override Equals, so the default comparer already compares by reference).
    private Dictionary<DefinitionClass, ChunkNodeViewModel> _definitionNodes = [];

    private LoadedFileKind _loadedKind = LoadedFileKind.Ddb;

    // Browser-style navigation history of visited nodes. _historyIndex points at the current
    // entry; Back/Forward move it. _navigatingHistory suppresses re-recording while we
    // programmatically re-select a node during a Back/Forward replay.
    private readonly List<ChunkNodeViewModel> _history = [];
    private int _historyIndex = -1;
    private bool _navigatingHistory;

    public string? CurrentPath { get; private set; }

    public void Save(string path)
    {
        switch (_loadedKind)
        {
            case LoadedFileKind.PackagesDat:
                PackagesDatFile.Save(path, _loadedPackages);
                break;
            case LoadedFileKind.Tpi:
                PackagesDatFile.SaveTpi(path, _loadedPackages[0]);
                break;
            default:
                FileLoader.Save(path, _loadedChunks);
                break;
        }
    }

    // The definition backing the current selection, if any — the target for JSON export.
    public DefinitionClass? SelectedDefinition => SelectedNode?.Data as DefinitionClass;

    // Drives the enabled state of the "Export Def to JSON" menu item.
    public bool CanExportJson => SelectedDefinition is not null;

    public void ExportDefinitionJson(string path)
    {
        if (SelectedDefinition is { } def)
            System.IO.File.WriteAllText(path, DefinitionJsonExporter.ToJson(def));
    }

    // Coalesce rapid keystrokes so the tree is filtered once the user pauses,
    // rather than re-walking the whole tree on every character.
    private readonly DispatcherTimer _searchDebounce;

    public MainWindowViewModel()
    {
        _searchDebounce = new DispatcherTimer { Interval = TimeSpan.FromMilliseconds(200) };
        _searchDebounce.Tick += (_, _) =>
        {
            _searchDebounce.Stop();
            var filter = SearchText.Trim();
            foreach (var node in RootNodes)
                ApplyFilter(node, filter);
        };
    }

    partial void OnSearchTextChanged(string value)
    {
        // Restart the countdown; the filter runs when typing settles.
        _searchDebounce.Stop();
        _searchDebounce.Start();
    }

    // Returns true if the node or any descendant matches the filter. Rather than
    // toggling per-node visibility (which forces the virtualizing panel to walk
    // every hidden node), we rebuild each node's VisibleChildren to hold only the
    // matching subtree, so the tree materializes work proportional to matches.
    private static bool ApplyFilter(ChunkNodeViewModel node, string filter)
    {
        if (filter.Length == 0)
        {
            node.IsVisible = true;
            node.IsExpanded = false;
            foreach (var child in node.Children)
                ApplyFilter(child, filter);
            node.VisibleChildren.ReplaceAll(node.Children);
            return true;
        }

        bool selfMatch = node.Label.Contains(filter, StringComparison.OrdinalIgnoreCase);

        var visible = new List<ChunkNodeViewModel>();
        foreach (var child in node.Children)
            if (ApplyFilter(child, filter))
                visible.Add(child);

        node.VisibleChildren.ReplaceAll(visible);
        node.IsVisible = selfMatch || visible.Count > 0;
        node.IsExpanded = visible.Count > 0;
        return node.IsVisible;
    }

    partial void OnSelectedNodeChanged(ChunkNodeViewModel? value)
    {
        OnPropertyChanged(nameof(SelectedDefinition));
        OnPropertyChanged(nameof(CanExportJson));

        // Record every user-driven selection (tree clicks and link jumps alike) as a
        // history entry; replays during Back/Forward set _navigatingHistory to skip this.
        if (!_navigatingHistory && value is not null)
            RecordHistory(value);

        if (value?.Data is null)
        {
            Properties.ReplaceAll([]);
            return;
        }

        var rows = new List<PropertyRow>();
        BuildProperties(value.Data, rows);
        Properties.ReplaceAll(rows);
    }

    /// <summary>
    /// Selects the definition with the given ID in the tree (used by click-through
    /// reference links). No-op if the ID doesn't resolve to a loaded definition.
    /// </summary>
    public void NavigateToDefinition(uint id)
    {
        var target = DefinitionMgrClass.Find_Definition(id, twiddle: false);
        if (target is null || !_definitionNodes.TryGetValue(target, out var node))
            return;

        SelectNode(node);
    }

    /// <summary>
    /// Reveals a node in the tree — clearing any active search filter and expanding its
    /// ancestor chain so a virtualized/collapsed node is realized — then selects it.
    /// Shared by link navigation and Back/Forward history replay.
    /// </summary>
    private void SelectNode(ChunkNodeViewModel node)
    {
        // A live search filter may be hiding the node; clear it so the node is reachable.
        if (SearchText.Length > 0)
        {
            SearchText = "";        // clears the box (and schedules a debounced re-filter)
            _searchDebounce.Stop();  // cancel it — we reset the filter synchronously below
            foreach (var root in RootNodes)
                ApplyFilter(root, "");
        }

        for (var parent = node.Parent; parent is not null; parent = parent.Parent)
            parent.IsExpanded = true;
        node.IsVisible = true;

        SelectedNode = node;
    }

    // ── Navigation history (Back/Forward) ───────────────────────────────────────

    /// <summary>Appends a newly-selected node, dropping any forward entries (browser semantics).</summary>
    private void RecordHistory(ChunkNodeViewModel node)
    {
        // Re-selecting the current entry (e.g. clicking the already-selected node) is a no-op.
        if (_historyIndex >= 0 && ReferenceEquals(_history[_historyIndex], node))
            return;

        // Truncate the forward history — navigating from the middle forks a new path.
        if (_historyIndex < _history.Count - 1)
            _history.RemoveRange(_historyIndex + 1, _history.Count - 1 - _historyIndex);

        _history.Add(node);
        _historyIndex = _history.Count - 1;
        NotifyHistoryCommands();
    }

    private void ClearHistory()
    {
        _history.Clear();
        _historyIndex = -1;
        NotifyHistoryCommands();
    }

    private void NotifyHistoryCommands()
    {
        GoBackCommand.NotifyCanExecuteChanged();
        GoForwardCommand.NotifyCanExecuteChanged();
    }

    private bool CanGoBack => _historyIndex > 0;
    private bool CanGoForward => _historyIndex >= 0 && _historyIndex < _history.Count - 1;

    [RelayCommand(CanExecute = nameof(CanGoBack))]
    private void GoBack()
    {
        if (!CanGoBack) return;
        _historyIndex--;
        ReplayHistory();
    }

    [RelayCommand(CanExecute = nameof(CanGoForward))]
    private void GoForward()
    {
        if (!CanGoForward) return;
        _historyIndex++;
        ReplayHistory();
    }

    // Re-selects the node at the current history index without recording it as a new entry.
    private void ReplayHistory()
    {
        _navigatingHistory = true;
        try { SelectNode(_history[_historyIndex]); }
        finally { _navigatingHistory = false; }
        NotifyHistoryCommands();
    }

    /// <summary>Opens a file, dispatching to the right loader based on its extension.</summary>
    public Task OpenAsync(string path)
    {
        var ext = System.IO.Path.GetExtension(path).ToLowerInvariant();
        return ext switch
        {
            ".tpi" => LoadPackagesAsync(path, isTpi: true),
            ".dat" => LoadPackagesAsync(path, isTpi: false),
            _ => LoadFileAsync(path),
        };
    }

    /// <summary>Loads a packages.dat (or single .tpi) and builds a package → files tree.</summary>
    public async Task LoadPackagesAsync(string path, bool isTpi)
    {
        IsLoading = true;
        RootNodes.Clear();
        Properties.Clear();
        SelectedNode = null;
        ClearHistory();

        List<PackageClass> packages;
        try
        {
            packages = await Task.Run(() => isTpi
                ? [PackagesDatFile.LoadTpi(path)]
                : PackagesDatFile.Load(path));
        }
        finally
        {
            IsLoading = false;
        }

        foreach (var pkg in packages)
        {
            var fileNodes = pkg.Files
                .Select(f => new ChunkNodeViewModel(f.FileName, f))
                .ToList();
            RootNodes.Add(new ChunkNodeViewModel($"{pkg.Name} [{pkg.Files.Count} files]", pkg, fileNodes));
        }

        _loadedPackages = packages;
        _loadedChunks = [];
        _definitionNodes = [];   // no navigable definitions in a package view
        _loadedKind = isTpi ? LoadedFileKind.Tpi : LoadedFileKind.PackagesDat;
        CurrentPath = path;
        HasFile = true;
        WindowTitle = $"Package Viewer — {System.IO.Path.GetFileName(path)}";
    }

    public async Task LoadFileAsync(string path)
    {
        IsLoading = true;
        RootNodes.Clear();
        Properties.Clear();
        SelectedNode = null;
        ClearHistory();

        List<TopLevelChunk> chunks;
        List<ChunkNodeViewModel> defChildren;

        try
        {
            (chunks, defChildren) = await Task.Run(() =>
            {
                var loaded = FileLoader.Load(path);
                var children = DefinitionMgrClass.FileOrderedItems
                    .OfType<DefinitionClass>()
                    .Select(def => new ChunkNodeViewModel($"{def.Get_Name()} [{def.GetType().Name}]", def))
                    .ToList();
                return (loaded, children);
            });
        }
        finally
        {
            IsLoading = false;
        }

        foreach (var chunk in chunks)
        {
            var children = chunk.Data is DefinitionMgrClass ? defChildren : null;
            RootNodes.Add(new ChunkNodeViewModel(chunk.Label, chunk.Data, children));
        }

        // Index the definition nodes by their backing definition for click-through navigation.
        _definitionNodes = defChildren
            .Where(n => n.Data is DefinitionClass)
            .ToDictionary(n => (DefinitionClass)n.Data!);

        _loadedChunks = chunks;
        _loadedPackages = [];
        _loadedKind = LoadedFileKind.Ddb;
        CurrentPath = path;
        HasFile = true;
        WindowTitle = $"Objects.ddb Viewer — {System.IO.Path.GetFileName(path)}";
    }

    private void BuildProperties(object obj, List<PropertyRow> rows, string prefix = "", HashSet<object>? visited = null, int depth = 0)
    {
        if (depth > 4) return;
        visited ??= new HashSet<object>(ReferenceEqualityComparer.Instance);
        if (!visited.Add(obj)) return;

        if (obj is PackageClass pkg && depth == 0)
        {
            BuildPackageRows(rows, pkg);
            return;
        }

        if (obj is PackageFileEntry entry && depth == 0)
        {
            BuildFileEntryRows(rows, entry);
            return;
        }

        if (obj is DefinitionClass def && depth == 0)
        {
            rows.Add(new PropertyRow("ID", def.Get_ID().ToString()));
            rows.Add(new PropertyRow("Name", def.Get_Name()));
            rows.Add(new PropertyRow("Type", obj.GetType().Name));
        }
        else if (obj is UnknownChunk unk && depth == 0)
        {
            rows.Add(new PropertyRow("ChunkId", $"0x{unk.ChunkId:X8}"));
            rows.Add(new PropertyRow("DataLength", unk.ChunkLength.ToString()));
            return;
        }

        var type = obj.GetType();

        // Direct members of an editable definition become editable rows (driven entirely by
        // the definition's schema); everything else stays read-only as before.
        var schema = depth == 0 ? DefinitionEditor.GetSchema(obj) : null;

        foreach (var field in DefinitionReflection.GetAllInstanceFields(type))
        {
            string name = prefix == "" ? field.Name : $"{prefix}.{field.Name}";
            if (depth == 0 && field.FieldType == typeof(int) &&
                DefinitionReferences.IsReference(type, field.Name))
            {
                rows.Add(MakeReferenceRow(name, (int)(field.GetValue(obj) ?? 0)));
                continue;
            }
            if (depth == 0 && field.FieldType == typeof(int[]) &&
                DefinitionReferences.IsReference(type, field.Name))
            {
                var ids = (int[])(field.GetValue(obj) ?? Array.Empty<int>());
                rows.Add(new PropertyRow(name, $"[{ids.Length} items]"));
                for (int i = 0; i < ids.Length; i++)
                    rows.Add(MakeReferenceRow($"{name}[{i}]", ids[i]));
                continue;
            }
            if (depth == 0 && field.FieldType == typeof(int[,]) &&
                DefinitionReferences.IsReference(type, field.Name))
            {
                var ids = (int[,])(field.GetValue(obj) ?? new int[0, 0]);
                int rowCount = ids.GetLength(0), colCount = ids.GetLength(1);
                rows.Add(new PropertyRow(name, $"[{rowCount}×{colCount} items]"));
                for (int i = 0; i < rowCount; i++)
                    for (int j = 0; j < colCount; j++)
                        rows.Add(MakeReferenceRow($"{name}[{i},{j}]", ids[i, j]));
                continue;
            }
            if (schema is not null &&
                DefinitionEditor.TryAddRows(rows, name, field.Name, field.FieldType, () => field.GetValue(obj), v => field.SetValue(obj, v), schema))
                continue;
            try { AddMemberValue(rows, name, field.GetValue(obj), visited, depth); }
            catch { rows.Add(new PropertyRow(name, "(error)")); }
        }

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                  .OrderBy(p => p.Name))
        {
            string name = prefix == "" ? prop.Name : $"{prefix}.{prop.Name}";
            if (schema is not null && prop.CanWrite &&
                DefinitionEditor.TryAddRows(rows, name, prop.Name, prop.PropertyType, () => prop.GetValue(obj), v => prop.SetValue(obj, v), schema))
                continue;
            try { AddMemberValue(rows, name, prop.GetValue(obj), visited, depth); }
            catch { rows.Add(new PropertyRow(name, "(error)")); }
        }
    }

    // Builds a row for an int that references another definition by ID. When the target
    // resolves to a loaded definition, the row is a click-through link; otherwise it's a
    // plain read-only row noting the ID couldn't be resolved.
    private PropertyRow MakeReferenceRow(string name, int id)
    {
        if (id == 0)
            return new PropertyRow(name, "0 (none)");

        var target = DefinitionMgrClass.Find_Definition((uint)id, twiddle: false);
        if (target is null || !_definitionNodes.ContainsKey(target))
            return new PropertyRow(name, $"{id} (not found)");

        string display = $"{id} → {target.Get_Name()} [{target.GetType().Name}]";
        return new PropertyRow(name, display, () => NavigateToDefinition((uint)id));
    }

    private void AddMemberValue(List<PropertyRow> rows, string name, object? value, HashSet<object> visited, int depth)
    {
        if (value is null)
        {
            rows.Add(new PropertyRow(name, "(null)"));
            return;
        }

        var type = value.GetType();

        if (type.IsPrimitive || value is string || type.IsEnum)
        {
            rows.Add(new PropertyRow(name, value.ToString() ?? ""));
            return;
        }

        if (value is byte[] bytes)
        {
            rows.Add(new PropertyRow(name, $"byte[{bytes.Length}]"));
            return;
        }

        if (value is IList list)
        {
            rows.Add(new PropertyRow(name, $"[{list.Count} items]"));
            int limit = Math.Min(list.Count, 20);
            for (int i = 0; i < limit; i++)
            {
                if (list[i] is not { } item) continue;
                string itemName = $"{name}[{i}]";
                var itemType = item.GetType();
                if (itemType.IsPrimitive || item is string || itemType.IsEnum)
                    rows.Add(new PropertyRow(itemName, item.ToString() ?? ""));
                else
                    BuildProperties(item, rows, itemName, visited, depth + 1);
            }
            return;
        }

        BuildProperties(value, rows, name, visited, depth + 1);
    }

    // ── Package editing rows ────────────────────────────────────────────────────
    // Packages aren't schema-driven definitions, so we emit editable rows directly.
    // Edits mutate the live PackageClass/PackageFileEntry, which Save then serializes.

    private static void BuildPackageRows(List<PropertyRow> rows, PackageClass pkg)
    {
        rows.Add(HexRow("PackageCRC", () => pkg.PackageCRC, v => pkg.PackageCRC = v));
        rows.Add(TextRow("Name", () => pkg.Name, v => pkg.Name = v));
        rows.Add(TextRow("Version", () => pkg.Version, v => pkg.Version = v));
        rows.Add(TextRow("Owner", () => pkg.Owner, v => pkg.Owner = v));
        rows.Add(UIntRow("Type", () => pkg.Type, v => pkg.Type = v));
        rows.Add(new PropertyRow("Files", $"{pkg.Files.Count} files"));
    }

    private static void BuildFileEntryRows(List<PropertyRow> rows, PackageFileEntry entry)
    {
        rows.Add(HexRow("FileCRC", () => entry.FileCRC, v => entry.FileCRC = v));
        rows.Add(UIntRow("FileSize", () => entry.FileSize, v => entry.FileSize = v));
        rows.Add(TextRow("FileName", () => entry.FileName, v => entry.FileName = v));
        // The name this entry has as a flat blob in the sibling files/ directory.
        rows.Add(new PropertyRow("Blob file (files/)", entry.BlobFileName));
    }

    private static PropertyRow TextRow(string name, Func<string> get, Action<string> set) =>
        new(name, get(), PropertyEditorKind.Text, v => { set(v); return get(); });

    private static PropertyRow UIntRow(string name, Func<uint> get, Action<uint> set) =>
        new(name, get().ToString(), PropertyEditorKind.Text, v =>
        {
            set(uint.Parse(v.Trim()));   // invalid input throws → PropertyRow keeps the typed text
            return get().ToString();
        });

    private static PropertyRow HexRow(string name, Func<uint> get, Action<uint> set) =>
        new(name, $"0x{get():X8}", PropertyEditorKind.Text, v =>
        {
            var t = v.Trim();
            if (t.StartsWith("0x", StringComparison.OrdinalIgnoreCase)) t = t[2..];
            set(Convert.ToUInt32(t, 16));
            return $"0x{get():X8}";
        });
}
