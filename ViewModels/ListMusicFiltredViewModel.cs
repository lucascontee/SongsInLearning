using CommunityToolkit.Mvvm.ComponentModel;
using SongsInLearning.Database;
using SongsInLearning.Models;
using SongsInLearning.Models.Enums;  
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class ListMusicsFiltredViewModel : ObservableObject
{
    private readonly MusicDbContext _dbContext;

    public ObservableCollection<Music> MusicsLearning { get; } = new();
    public ObservableCollection<Music> MusicsLearned { get; } = new();
    public ObservableCollection<Music> MusicsToLearn { get; } = new();

    public ListMusicsFiltredViewModel(MusicDbContext dbContext)
    {
        _dbContext = dbContext;
        LoadAllMusics();
    }

    private void LoadAllMusics()
    {
        var allMusics = _dbContext.Musics.ToList();

        MusicsLearning.Clear();
        MusicsLearned.Clear();
        MusicsToLearn.Clear();

        foreach (var music in allMusics)
        {
            switch (music.Progress)
            {
                case Progress.Learning:
                    MusicsLearning.Add(music);
                    break;
                case Progress.Learned:
                    MusicsLearned.Add(music);
                    break;
                case Progress.Learn:
                    MusicsToLearn.Add(music);
                    break;
            }
        }
    }
}
