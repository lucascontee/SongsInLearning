using Avalonia;
using Avalonia.Controls;
using Avalonia.Layout;
using Avalonia.Media;
using SongsInLearning.ViewModels;

namespace SongsInLearning.Views;

public partial class SideBarView : UserControl
{
    public SideBarView()
    {
        InitializeComponent();
        this.DataContextChanged += SideBarView_DataContextChanged;

    }

    private void SideBarView_DataContextChanged(object? sender, System.EventArgs e)
    {
        if (DataContext is SideBarViewModel viewModel)
        {
            viewModel.PropertyChanged -= ViewModel_PropertyChanged;
            viewModel.PropertyChanged += ViewModel_PropertyChanged;

            RebuildMenu(viewModel);
        }
    }

    private void ViewModel_PropertyChanged(object? sender, System.ComponentModel.PropertyChangedEventArgs e)
    {
        if (DataContext is SideBarViewModel viewModel && e.PropertyName == nameof(SideBarViewModel.Items))
        {
            RebuildMenu(viewModel);
        }
    }

    private void RebuildMenu(SideBarViewModel viewModel)
    {
        var panel = new StackPanel();

        foreach (var item in viewModel.Items)
        {
            panel.Children.Add(BuildMenuControls(item));
        }

        contentPresenter.Content = panel;
    }

    private Control BuildMenuControls(SideBarItemViewModel item)
    {
        if (item.IsCategory)
        {
            var expander = new Expander()
            {
                Header = item.Title,
                Classes = { "MenuExpander" },
                IsExpanded = false,
                Background = new SolidColorBrush(Color.FromRgb(5, 24, 56))
            };

            var stackPanel = new StackPanel();
            foreach (var child in item.Children)
            {
                stackPanel.Children.Add(BuildMenuControls(child));
            }

            expander.Content = stackPanel;
            return expander;
        }

        return CreateNavigationButton(item);
    }

    private Button CreateNavigationButton(SideBarItemViewModel vm)
    {

        var stackPanel = new StackPanel()
        {
            Orientation = Orientation.Horizontal,
            Spacing = 12,
            VerticalAlignment = VerticalAlignment.Center,
        };

        var pathIcon = new PathIcon()
        {
            Width = 20,
            Height = 20,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brush.Parse("#e8e8e8")
        };

        if (!string.IsNullOrEmpty(vm.IconKey) &&
        Application.Current!.TryFindResource(vm.IconKey, out var resource) &&
        resource is StreamGeometry geometry)
        {
            pathIcon.Data = geometry;
        }

        var textBlock = new TextBlock()
        {
            Text = vm.Title,
            VerticalAlignment = VerticalAlignment.Center,
            Foreground = Brush.Parse("#e8e8e8")

        };

        stackPanel.Children.Add(pathIcon);
        stackPanel.Children.Add(textBlock);

        var button = new Button()
        {
            Content = stackPanel,
            Classes = { "MenuBtn" },
            CommandParameter = vm.Key,
            Command = ((SideBarViewModel)DataContext!).NavigateCommand,
            IsEnabled = vm.Enabled,
            Height = 50,

        };

        return button;
    }
}