using CommunityToolkit.Mvvm.ComponentModel;

namespace ObjectsReaderUI.ViewModels;

public partial class ChunkNodeViewModel : ObservableObject
{
    public string Label { get; }
    public object? Data { get; }
    public List<ChunkNodeViewModel> Children { get; }

    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private bool _isExpanded;

    public ChunkNodeViewModel(string label, object? data = null, List<ChunkNodeViewModel>? children = null)
    {
        Label = label;
        Data = data;
        Children = children ?? [];
    }
}
