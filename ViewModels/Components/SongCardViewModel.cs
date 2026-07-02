using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SongsInLearning.Messages;
using SongsInLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class SongCardViewModel : ViewModelBase
{
    private readonly Song _song;

    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _artist = "";
    [ObservableProperty] private int _year;
    [ObservableProperty] private string _instrument = "";
    [ObservableProperty] private string _difficulty = "";
    [ObservableProperty] private string _progress = "";
    public Models.Enums.Progress ProgressEnum { get; }


    public SongCardViewModel(Song song)
    {
        _song = song;
        Name = song.Name;
        Artist = song.Artist;
        Year = song.Year;

        Instrument = song.Instrument.ToString();
        Difficulty = GetDifficultyFromEnum(song.Difficulty);
        ProgressEnum = song.Progress;
        Progress = GetProgressFromEnum(ProgressEnum);
    }

    public string GetProgressFromEnum(Models.Enums.Progress progress)
    {
        return progress switch
        {
            Models.Enums.Progress.Learn => "Na lista de espera",
            Models.Enums.Progress.Learning => "Em prática",
            Models.Enums.Progress.Learned => "Dominada",
            _ => "Status não definido"
        };
    }

    public string GetDifficultyFromEnum(Models.Enums.Difficulty difficulty)
    {
        return difficulty switch
        {
            Models.Enums.Difficulty.Easy => "Fácil",
            Models.Enums.Difficulty.Medium => "Médio",
            Models.Enums.Difficulty.Hard => "Difícil",
            Models.Enums.Difficulty.VeryHard => "Muito Difícil",
            Models.Enums.Difficulty.Extreme => "Extrema",
            _ => "Status não definido"
        };
    }

    [RelayCommand]
    public void EditSong()
    {
        WeakReferenceMessenger.Default.Send(new NavigateToEditSongMessage(_song));
    }
}
