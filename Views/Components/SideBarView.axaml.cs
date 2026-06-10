using Avalonia.Controls;
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
        var button = new Button()
        {
            Content = vm.Title,
            Classes = { "MenuBtn" },
            CommandParameter = vm.Key,
            Command = ((SideBarViewModel)DataContext).NavigateCommand,
            IsEnabled = vm.Enabled
        };

        return button;
    }
}