using CommunityToolkit.Mvvm.ComponentModel;

namespace ObjectsReaderUI.ViewModels;

public partial class ChunkNodeViewModel : ObservableObject
{
    public string Label { get; }
    public object? Data { get; }
    public List<ChunkNodeViewModel> Children { get; }

    // The node this one hangs under (null for roots). Lets navigation expand the
    // ancestor chain so a programmatically selected node is actually visible.
    public ChunkNodeViewModel? Parent { get; private set; }

    public RangeObservableCollection<ChunkNodeViewModel> VisibleChildren { get; }

    [ObservableProperty] private bool _isVisible = true;
    [ObservableProperty] private bool _isExpanded;

    public ChunkNodeViewModel(string label, object? data = null, List<ChunkNodeViewModel>? children = null)
    {
        Label = label;
        Data = data;
        Children = children ?? [];
        foreach (var child in Children)
            child.Parent = this;
        VisibleChildren = [];
        VisibleChildren.ReplaceAll(Children);
    }
}
