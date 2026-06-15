using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;
using SongsInLearning.Database;

namespace SongsInLearning.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly MusicDbContext _dbContext;

    [ObservableProperty]
    public bool _isSideBarVisible;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public HomeViewModel HomeViewModel { get; }
    public SideBarViewModel SideBarViewModel { get; }


    public IRelayCommand OpenSideBarCommand { get; }


    public MainViewModel()
    {
        OpenSideBarCommand = new RelayCommand(OpenSideBar);
        HomeViewModel = new HomeViewModel();
        SideBarViewModel = new SideBarViewModel(this);
    }

    public void OpenSideBar()
    {
        IsSideBarVisible = !IsSideBarVisible;
    }

    public void CloseSideBar()
    {
        IsSideBarVisible = false;
    }

    public void Navigate(string destination) 
    {
        CurrentView = destination switch
        {
            "Home" => Program.AppHost.Services.GetRequiredService<HomeViewModel>(),
            "NewSong" => Program.AppHost.Services.GetRequiredService<CreateSongViewModel>(),
            _ => CurrentView
        };

        CloseSideBar();
    }

}

