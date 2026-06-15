using System.Collections;
using System.Collections.ObjectModel;
using System.Reflection;
using CommunityToolkit.Mvvm.ComponentModel;
using RenData.Definitions;
using RenData.SaveLoad;

namespace ObjectsReaderUI.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public ObservableCollection<ChunkNodeViewModel> RootNodes { get; } = [];
    public ObservableCollection<PropertyRow> Properties { get; } = [];

    [ObservableProperty] private ChunkNodeViewModel? _selectedNode;
    [ObservableProperty] private bool _isLoading;
    [ObservableProperty] private string _windowTitle = "Objects.ddb Viewer";

    partial void OnSelectedNodeChanged(ChunkNodeViewModel? value)
    {
        Properties.Clear();
        if (value?.Data is null) return;

        var rows = new List<PropertyRow>();
        BuildProperties(value.Data, rows);
        foreach (var row in rows)
            Properties.Add(row);
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

        foreach (var field in type.GetFields(BindingFlags.Public | BindingFlags.Instance).OrderBy(f => f.Name))
        {
            string name = prefix == "" ? field.Name : $"{prefix}.{field.Name}";
            try { AddMemberValue(rows, name, field.GetValue(obj), visited, depth); }
            catch { rows.Add(new PropertyRow(name, "(error)")); }
        }

        foreach (var prop in type.GetProperties(BindingFlags.Public | BindingFlags.Instance)
                                  .Where(p => p.CanRead && p.GetIndexParameters().Length == 0)
                                  .OrderBy(p => p.Name))
        {
            string name = prefix == "" ? prop.Name : $"{prefix}.{prop.Name}";
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
