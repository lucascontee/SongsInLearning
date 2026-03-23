using CommunityToolkit.Mvvm.ComponentModel;
using SongsInLearning.Database;
using SongsInLearning.Models;
using System;
using System.Collections.ObjectModel;

namespace SongsInLearning.ViewModels;

public partial class MainViewModel : ViewModelBase
{
    private readonly MusicDbContext _dbContext;

    [ObservableProperty]
    public bool _isSideBarVisible;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public HomeViewModel HomeViewModel { get; }
    
    public MainViewModel()
    {
        HomeViewModel = new HomeViewModel();
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
            _ => CurrentView
        };

        CloseSideBar();
    }

}

