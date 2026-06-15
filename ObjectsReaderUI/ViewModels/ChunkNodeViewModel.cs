namespace ObjectsReaderUI.ViewModels;

public class ChunkNodeViewModel
{
    public string Label { get; }
    public object? Data { get; }
    public List<ChunkNodeViewModel> Children { get; }

    public ChunkNodeViewModel(string label, object? data = null, List<ChunkNodeViewModel>? children = null)
    {
        Label = label;
        Data = data;
        Children = children ?? [];
    }
}
