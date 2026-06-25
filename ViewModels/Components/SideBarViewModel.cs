using CommunityToolkit.Mvvm.Input;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace SongsInLearning.ViewModels;

public partial class SideBarViewModel : ViewModelBase
{
    private readonly MainViewModel _mainViewModel;
    public IRelayCommand CloseSideBarCommand { get; }

    private ObservableCollection<SideBarItemViewModel> _items = new ObservableCollection<SideBarItemViewModel>();
    public ObservableCollection<SideBarItemViewModel> Items
    {
        get => _items;
        set => SetProperty(ref _items, value);
    }

    public SideBarViewModel(MainViewModel mainViewModel) 
    {
        _mainViewModel = mainViewModel;
        CloseSideBarCommand = new RelayCommand(CloseSideBar);
        BuildMenu();

    }

    public void BuildMenu()
    {
        var newItems = new List<SideBarItemViewModel>
        {
            new SideBarItemViewModel("Home", "Home", 1, true, "HomeRegular"),
            new SideBarItemViewModel("NewSong", "Nova música", 1, true, "AddRegular"),

        };

        Items = new ObservableCollection<SideBarItemViewModel>(newItems);
    }

    private void CloseSideBar()
    {
        _mainViewModel.IsSideBarVisible = false;
    }

    [RelayCommand]
    private void Navigate(string destination)
    {
        _mainViewModel?.Navigate(destination);
    }

}

