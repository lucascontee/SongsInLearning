using CommunityToolkit.Mvvm.ComponentModel;
using Microsoft.EntityFrameworkCore;
using SongsInLearning.Database;
using SongsInLearning.Models;
using System.Collections.ObjectModel;
using System.Linq;

namespace SongsInLearning.ViewModels;

public partial class HomeViewModel : ViewModelBase
{
    private readonly MusicDbContext _dbContext;

    public ObservableCollection<Music> Musics { get; } = new();

    public HomeViewModel()
    {
        //_dbContext = dbContext;
        //LoadMusics();
    }

    //private void LoadMusics()
    //{
    //    var list = _dbContext.Musics.ToList();
    //    Musics.Clear();
    //    foreach (var music in list)
    //    Musics.Add(music);
    //}
}
 