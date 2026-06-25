using CommunityToolkit.Mvvm.ComponentModel;
using SongsInLearning.Models.Enums;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class NotificationViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isVisible;
    [ObservableProperty] private string _message = string.Empty;

    public async Task ShowNotificationAsync(string message, NotificationType notificationType, int delay) 
    {
        Message = message;
        IsVisible = true;

        await Task.Delay(delay);

        IsVisible = false;
        Message = string.Empty;
    }
}