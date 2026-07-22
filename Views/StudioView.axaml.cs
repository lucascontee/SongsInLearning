using Avalonia;
using Avalonia.Controls;
using Avalonia.Interactivity;
using Avalonia.Markup.Xaml;
using Avalonia.Platform.Storage;
using SongsInLearning.ViewModels;

namespace SongsInLearning.Views;

public partial class StudioView : UserControl
{
    public StudioView()
    {
        InitializeComponent();
    }

    private async void OnSelectPluginClicked(object? sender, RoutedEventArgs e)
    {
        var topLevel = TopLevel.GetTopLevel(this);
        if (topLevel == null) return;

        var files = await topLevel.StorageProvider.OpenFilePickerAsync(new FilePickerOpenOptions
        {
            Title = "Selecione a DLL do Plugin VST",
            AllowMultiple = false,
            FileTypeFilter = new[] { new FilePickerFileType("VST Plugin") { Patterns = new[] { "*.dll" } } }
        });

        if (files.Count >= 1)
        {
            var filePath = files[0].Path.LocalPath;

            if (DataContext is StudioViewModel vm)
            {
                vm.LoadVstPlugin(filePath);
            }
        }
    }
}