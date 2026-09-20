using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
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

    private LoadedFileKind _loadedKind = LoadedFileKind.Ddb;

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

        if (value?.Data is null)
        {
            Properties.ReplaceAll([]);
            return;
        }

        var rows = new List<PropertyRow>();
        BuildProperties(value.Data, rows);
        Properties.ReplaceAll(rows);
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

        _loadedChunks = chunks;
        _loadedPackages = [];
        _loadedKind = LoadedFileKind.Ddb;
        CurrentPath = path;
        HasFile = true;
        WindowTitle = $"Objects.ddb Viewer — {System.IO.Path.GetFileName(path)}";
    }

    private static void BuildProperties(object obj, List<PropertyRow> rows, string prefix = "", HashSet<object>? visited = null, int depth = 0)
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

    private static void AddMemberValue(List<PropertyRow> rows, string name, object? value, HashSet<object> visited, int depth)
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
