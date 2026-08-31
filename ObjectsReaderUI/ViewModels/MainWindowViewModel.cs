using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using ObjectsReaderUI.Editing;
using RenData.Definitions;
using RenData.SaveLoad;

namespace ObjectsReaderUI.ViewModels;

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

    public string? CurrentPath { get; private set; }

    public void Save(string path) => FileLoader.Save(path, _loadedChunks);

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
        CurrentPath = path;
        HasFile = true;
        WindowTitle = $"Objects.ddb Viewer — {System.IO.Path.GetFileName(path)}";
    }

    private static void BuildProperties(object obj, List<PropertyRow> rows, string prefix = "", HashSet<object>? visited = null, int depth = 0)
    {
        if (depth > 4) return;
        visited ??= new HashSet<object>(ReferenceEqualityComparer.Instance);
        if (!visited.Add(obj)) return;

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
}
