using CommunityToolkit.Mvvm.ComponentModel;
using SongsInLearning.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class SongCardViewModel : ViewModelBase
{
    [ObservableProperty] private string _name = "";
    [ObservableProperty] private string _artist = "";
    [ObservableProperty] private int _year;
    [ObservableProperty] private string _instrument = "";
    [ObservableProperty] private string _difficulty = "";
    [ObservableProperty] private string _progress = "";

    public SongCardViewModel(Song song)
    {
        Name = song.Name;
        Artist = song.Artist;
        Year = song.Year;

        Instrument = song.Instrument.ToString();
        Difficulty = song.Difficulty.ToString();
        Progress = song.Progress.ToString();
    }
}
