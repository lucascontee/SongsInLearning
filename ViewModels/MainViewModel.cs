using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using Microsoft.Extensions.DependencyInjection;
using SongsInLearning.Database;
using SongsInLearning.Messages;
using SongsInLearning.Models.Enums;
using SongsInLearning.Services;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class MainViewModel : ObservableObject
{

    [ObservableProperty]
    public bool _isSideBarVisible;

    [ObservableProperty]
    private ViewModelBase _currentView;

    public HomeViewModel HomeViewModel { get; }
    public SideBarViewModel SideBarViewModel { get; }
    public NotificationViewModel NotificationViewModel { get; }


    public IRelayCommand OpenSideBarCommand { get; }


    public MainViewModel()
    {
        OpenSideBarCommand = new RelayCommand(OpenSideBar);
        SideBarViewModel = new SideBarViewModel(this);
        NotificationViewModel = new NotificationViewModel();
        HomeViewModel = Program.AppHost.Services.GetRequiredService<HomeViewModel>();

        WeakReferenceMessenger.Default.Register<NavigateToHomeMessage>(this, (r, m) =>
        {
            CurrentView = Program.AppHost.Services.GetRequiredService<HomeViewModel>();
        });

        WeakReferenceMessenger.Default.Register<ShowNotificationMessage>(this, async (r, m) =>
        {
            await NotificationViewModel.ShowNotificationAsync(m.Message, m.Type, m.Delay);
        });

        WeakReferenceMessenger.Default.Register<NavigateToEditSongMessage>(this, (r, m) =>
        {
            var songService = Program.AppHost.Services.GetRequiredService<SongService>();
            CurrentView = new EditSongViewModel(songService, m.SongToEdit);
        });
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

    public async Task ShowNotificationAsync(string message, NotificationType notificationType, int delay)
    {
        await NotificationViewModel.ShowNotificationAsync(message, notificationType, delay);
    }

}

