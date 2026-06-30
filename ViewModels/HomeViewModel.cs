using SongsInLearning.Models.Enums;
using SongsInLearning.Services;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly SongService _songService;

    public ObservableCollection<SongCardViewModel> SongCards { get; } = new();
    public IEnumerable<SongCardViewModel> InProgress =>
            SongCards.Where(s => s.ProgressEnum == Progress.Learning);

    public IEnumerable<SongCardViewModel> ToLearn =>
        SongCards.Where(s => s.ProgressEnum == Progress.Learn);

    public IEnumerable<SongCardViewModel> Learned =>
        SongCards.Where(s => s.ProgressEnum == Progress.Learned);

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

        OnPropertyChanged(nameof(InProgress));
        OnPropertyChanged(nameof(ToLearn));
        OnPropertyChanged(nameof(Learned));
    }
}