using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Platform.Storage;
using ObjectsReaderUI.ViewModels;

namespace ObjectsReaderUI.Views;

public partial class MainWindow : Window
{
    public MainWindow()
    {
        InitializeComponent();
    }

    private async void OnOpenClicked(object? sender, RoutedEventArgs e)
    {
        var files = await StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Open objects.ddb",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Database files") { Patterns = ["*.ddb"] },
                new FilePickerFileType("All files")      { Patterns = ["*.*"] },
            ]
        });

        if (files is [var file] && DataContext is MainWindowViewModel vm)
        {
            var path = file.TryGetLocalPath();
            if (path is not null)
                await vm.LoadFileAsync(path);
        }
    }
}
