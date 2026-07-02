using CommunityToolkit.Mvvm.ComponentModel;
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

    private List<SongCardViewModel> _allSongsCache = new();

    [ObservableProperty]
    private string _searchText = string.Empty;

    [ObservableProperty]
    private string _selectedStatusFilter = "Todos";

    public ObservableCollection<SongCardViewModel> FilteredSongs { get; } = new();

    public List<string> AvailableStatusFilters { get; } = new()
    {
        "Todos",
        "Na lista de espera",
        "Em prática",
        "Dominada"
    };

    public HomeViewModel(SongService songService)
    {
        _songService = songService;
        _ = LoadSongsAsync();
    }

    private async Task LoadSongsAsync()
    {
        var songs = await _songService.GetAllAsync();

        _allSongsCache.Clear();
        foreach (var song in songs)
        {
            _allSongsCache.Add(new SongCardViewModel(song));
        }

        ApplyFilters();
    }

    partial void OnSearchTextChanged(string value)
    {
        ApplyFilters();
    }

    partial void OnSelectedStatusFilterChanged(string value)
    {
        ApplyFilters();
    }

    private void ApplyFilters()
    {
        var filteredList = _allSongsCache.AsEnumerable();

        if (!string.IsNullOrWhiteSpace(SearchText))
        {
            var searchLower = SearchText.ToLower();
            filteredList = filteredList.Where(s =>
                s.Name.ToLower().Contains(searchLower) ||
                s.Artist.ToLower().Contains(searchLower));
        }

        if (SelectedStatusFilter != "Todos")
        {
            var targetStatus = SelectedStatusFilter switch
            {
                "Na lista de espera" => Progress.Learn,
                "Em prática" => Progress.Learning,
                "Dominada" => Progress.Learned,
                _ => (Progress?)null
            };

            if (targetStatus.HasValue)
            {
                filteredList = filteredList.Where(s => s.ProgressEnum == targetStatus.Value);
            }
        }

        FilteredSongs.Clear();
        foreach (var song in filteredList)
        {
            FilteredSongs.Add(song);
        }
    }
}