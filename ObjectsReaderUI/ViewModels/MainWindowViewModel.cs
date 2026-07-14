using System.Collections;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Numerics;
using System.Reflection;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using RenData.Definitions;
using RenData.SaveLoad;
using RenData.Types;

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

        // Direct scalar members of an editable definition become editable rows; everything
        // else (nested objects, lists, other definition types) stays read-only as before.
        bool editable = depth == 0 && IsEditableDefinition(obj);

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance).OrderBy(f => f.Name))
        {
            string name = prefix == "" ? field.Name : $"{prefix}.{field.Name}";
            if (editable && TryAddEditableRow(rows, name, field.Name, field.FieldType, () => field.GetValue(obj), v => field.SetValue(obj, v)))
                continue;
            try { AddMemberValue(rows, name, field.GetValue(obj), visited, depth); }
            catch { rows.Add(new PropertyRow(name, "(error)")); }
        }

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                  .OrderBy(p => p.Name))
        {
            string name = prefix == "" ? prop.Name : $"{prefix}.{prop.Name}";
            if (editable && prop.CanWrite &&
                TryAddEditableRow(rows, name, prop.Name, prop.PropertyType, () => prop.GetValue(obj), v => prop.SetValue(obj, v)))
                continue;
            try { AddMemberValue(rows, name, prop.GetValue(obj), visited, depth); }
            catch { rows.Add(new PropertyRow(name, "(error)")); }
        }
    }

    // Only these definition types expose editable fields for now.
    private static bool IsEditableDefinition(object obj) => obj is AmmoDefinitionClass;

    // Composite members that get flattened into per-component float rows.
    private static bool IsExpandableComposite(Type t) =>
        t == typeof(Vector2) || t == typeof(Vector3) || t == typeof(RectClass);

    // Emits one editable text row per public instance float field of the composite.
    // Each leaf setter reads the current composite, mutates one component, and writes
    // the whole thing back — required because Vector2/Vector3 are structs (GetValue
    // returns a boxed copy); harmless for the RectClass reference type.
    private static void ExpandComposite(List<PropertyRow> rows, string prefix,
        Func<object?> get, Action<object?> set)
    {
        var container = get();
        if (container is null) return;

        foreach (var leaf in container.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
        {
            if (!leaf.FieldType.IsPrimitive) continue;   // only scalar components (float, etc.)

            var field = leaf;   // capture per-iteration for the closures
            string display = Convert.ToString(field.GetValue(container), CultureInfo.InvariantCulture) ?? "";
            rows.Add(new PropertyRow($"{prefix}.{field.Name}", display, PropertyEditorKind.Text, v =>
            {
                object? box = get();
                if (box is null) return;
                field.SetValue(box, Convert.ChangeType(v, field.FieldType, CultureInfo.InvariantCulture));
                set(box);
            }));
        }
    }

    // Identity/plumbing members that must never be edited, even on an editable definition.
    private static readonly HashSet<string> NonEditableMembers =
    [
        "m_DefinitionMgrLink",
        "Old_Object_Pointer",
    ];

    // int-backed fields that are really enums in the engine. The array index is the
    // stored integer value (engine enums start at 0 and increment).
    private static readonly Dictionary<string, string[]> IntChoiceFields = new()
    {
        ["AmmoType"] = ["AMMO_TYPE_NORMAL", "AMMO_TYPE_C4_REMOTE", "AMMO_TYPE_C4_TIMED", "AMMO_TYPE_C4_PROXIMITY"],
    };

    private static bool TryAddEditableRow(List<PropertyRow> rows, string name, string memberName,
        Type memberType, Func<object?> get, Action<object?> set)
    {
        if (NonEditableMembers.Contains(memberName))
            return false;

        // Vectors / rects: expand into editable float leaves (X/Y/Z, Left/Top/Right/Bottom).
        if (IsExpandableComposite(memberType))
        {
            ExpandComposite(rows, name, get, set);
            return true;
        }

        // int-backed enums: present the named values, store the index.
        if (memberType == typeof(int) && IntChoiceFields.TryGetValue(memberName, out var choices))
        {
            int current = (int)(get() ?? 0);
            string display = current >= 0 && current < choices.Length ? choices[current] : current.ToString();
            rows.Add(new PropertyRow(name, display, PropertyEditorKind.Choice, v =>
            {
                int idx = Array.IndexOf(choices, v);
                if (idx >= 0) set(idx);
            }, choices));
            return true;
        }

        // Real enum types: dropdown of the enum names.
        if (memberType.IsEnum)
        {
            var names = Enum.GetNames(memberType);
            rows.Add(new PropertyRow(name, get()?.ToString() ?? "", PropertyEditorKind.Choice,
                v => set(Enum.Parse(memberType, v)), names));
            return true;
        }

        // Booleans: True/False dropdown.
        if (memberType == typeof(bool))
        {
            rows.Add(new PropertyRow(name, get()?.ToString() ?? "False", PropertyEditorKind.Choice,
                v => set(bool.Parse(v)), ["False", "True"]));
            return true;
        }

        // Numbers and strings: free-form text with culture-invariant round-tripping.
        if (memberType == typeof(string) || (memberType.IsPrimitive && memberType != typeof(char) && memberType != typeof(IntPtr)))
        {
            string display = memberType == typeof(string)
                ? get() as string ?? ""
                : Convert.ToString(get(), CultureInfo.InvariantCulture) ?? "";
            rows.Add(new PropertyRow(name, display, PropertyEditorKind.Text, v =>
            {
                object converted = memberType == typeof(string) ? v : Convert.ChangeType(v, memberType, CultureInfo.InvariantCulture);
                set(converted);
            }));
            return true;
        }

        return false;
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
