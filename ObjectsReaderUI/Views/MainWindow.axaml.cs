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
            Title = "Open file",
            AllowMultiple = false,
            FileTypeFilter =
            [
                new FilePickerFileType("Supported files") { Patterns = ["*.ddb", "*.dat", "*.tpi"] },
                new FilePickerFileType("Database files")  { Patterns = ["*.ddb"] },
                new FilePickerFileType("Package files")   { Patterns = ["*.dat", "*.tpi"] },
                new FilePickerFileType("All files")       { Patterns = ["*.*"] },
            ]
        });

        if (files is [var file] && DataContext is MainWindowViewModel vm)
        {
            var path = file.TryGetLocalPath();
            if (path is not null)
                await vm.OpenAsync(path);
        }
    }

    private async void OnSaveClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel { HasFile: true } vm)
            return;

        // Save straight to the loaded file if we know where it is, otherwise prompt.
        if (vm.CurrentPath is { } path)
            vm.Save(path);
        else
            await SaveAs(vm);
    }

    private async void OnSaveAsClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is MainWindowViewModel { HasFile: true } vm)
            await SaveAs(vm);
    }

    private async void OnExportJsonClicked(object? sender, RoutedEventArgs e)
    {
        if (DataContext is not MainWindowViewModel { SelectedDefinition: { } def } vm)
            return;

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Export definition to JSON",
            DefaultExtension = "json",
            SuggestedFileName = $"{Sanitize(def.Get_Name())}.json",
            FileTypeChoices =
            [
                new FilePickerFileType("JSON files") { Patterns = ["*.json"] },
            ]
        });

        var path = file?.TryGetLocalPath();
        if (path is not null)
            vm.ExportDefinitionJson(path);
    }

    // Strips characters that aren't legal in a filename so the definition name can seed one.
    private static string Sanitize(string name)
    {
        if (string.IsNullOrWhiteSpace(name)) return "definition";
        var chars = name.Select(c => System.IO.Path.GetInvalidFileNameChars().Contains(c) ? '_' : c);
        return new string(chars.ToArray());
    }

    private async Task SaveAs(MainWindowViewModel vm)
    {
        // Keep the same extension/format as what was loaded.
        var ext = vm.CurrentPath is { } cp
            ? System.IO.Path.GetExtension(cp).TrimStart('.').ToLowerInvariant()
            : "ddb";
        if (ext.Length == 0) ext = "ddb";

        var file = await StorageProvider.SaveFilePickerAsync(new FilePickerSaveOptions
        {
            Title = "Save As",
            DefaultExtension = ext,
            SuggestedFileName = vm.CurrentPath is { } p ? System.IO.Path.GetFileName(p) : $"objects.{ext}",
            FileTypeChoices =
            [
                new FilePickerFileType($"{ext} files") { Patterns = [$"*.{ext}"] },
                new FilePickerFileType("All files")    { Patterns = ["*.*"] },
            ]
        });

        var path = file?.TryGetLocalPath();
        if (path is not null)
            vm.Save(path);
    }
}
