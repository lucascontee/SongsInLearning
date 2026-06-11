using CommunityToolkit.Mvvm.ComponentModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class SongCardViewModel : ViewModelBase
{
    [ObservableProperty] private string _name = "Eruption";
    [ObservableProperty] private string _artist = "Van Halen";
    [ObservableProperty] private int _year = 1978;
    [ObservableProperty] private string _instrument = "Guitarra";
    [ObservableProperty] private string _difficulty = "Díficl";
    [ObservableProperty] private string _progress = "Em andamento";

    public SongCardViewModel()
    {
        
    }
}
