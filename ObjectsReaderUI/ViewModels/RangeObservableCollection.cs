using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;

namespace ObjectsReaderUI.ViewModels;

/// <summary>
/// ObservableCollection that can replace its entire contents while raising a
/// single Reset notification instead of one event per item.
/// </summary>
public class RangeObservableCollection<T> : ObservableCollection<T>
{
    public void ReplaceAll(IEnumerable<T> items)
    {
        Items.Clear();
        foreach (var item in items)
            Items.Add(item);

        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs(nameof(Count)));
        OnPropertyChanged(new System.ComponentModel.PropertyChangedEventArgs("Item[]"));
        OnCollectionChanged(new NotifyCollectionChangedEventArgs(NotifyCollectionChangedAction.Reset));
    }
}
