using SongsInLearning.Services;
using System.Collections.ObjectModel;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly SongService _songService; // Assumindo a existência deste serviço

    public ObservableCollection<SongCardViewModel> SongCards { get; } = new();

    public HomeViewModel(SongService songService)
    {
        _songService = songService;
        LoadSongs();
    }

    private async Task LoadSongs()
    {
        var songs = await _songService.GetAllAsync();
        SongCards.Clear();
        foreach (var song in songs)
        {
            SongCards.Add(new SongCardViewModel(song));
        }
    }
}