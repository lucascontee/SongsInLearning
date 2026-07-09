using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using SongsInLearning.Messages;
using SongsInLearning.Models;
using SongsInLearning.Models.Enums;
using SongsInLearning.Services;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;

namespace SongsInLearning.ViewModels;

public partial class EditSongViewModel : ViewModelBase
{
    private readonly SongService _songService;
    private readonly Song _originalSong; 

    [ObservableProperty] private string _name = string.Empty;
    [ObservableProperty] private string _artist = string.Empty;
    [ObservableProperty] private string _yearText = string.Empty;
    [ObservableProperty] private string _bpmText = string.Empty;
    [ObservableProperty] private string _userAnnotations = string.Empty;
    [ObservableProperty] private string _infosGeneratedByIA = string.Empty;

    [ObservableProperty] private Instrument _selectedInstrument;
    [ObservableProperty] private Difficulty _selectedDifficulty;
    [ObservableProperty] private Tuning _selectedTuning;
    [ObservableProperty] private Progress _selectedProgress;

    public IEnumerable<Instrument> AvailableInstruments => Enum.GetValues<Instrument>();
    public IEnumerable<Difficulty> AvailableDifficulties => Enum.GetValues<Difficulty>();
    public IEnumerable<Tuning> AvailableTunings => Enum.GetValues<Tuning>();
    public IEnumerable<Progress> AvailableProgresses => Enum.GetValues<Progress>();

    public EditSongViewModel(SongService songService, Song songToEdit)
    {
        _songService = songService;
        _originalSong = songToEdit;

        LoadSongData();
    }

    private void LoadSongData()
    {
        Name = _originalSong.Name;
        Artist = _originalSong.Artist;
        YearText = _originalSong.Year > 0 ? _originalSong.Year.ToString() : string.Empty;
        BpmText = _originalSong.Bpm > 0 ? _originalSong.Bpm.ToString(CultureInfo.InvariantCulture) : string.Empty;
        UserAnnotations = _originalSong.UserAnnotations;
        InfosGeneratedByIA = _originalSong.InfosGeneratedByIA;

        SelectedInstrument = _originalSong.Instrument;
        SelectedDifficulty = _originalSong.Difficulty;
        SelectedTuning = _originalSong.Tuning;
        SelectedProgress = _originalSong.Progress;
    }

    [RelayCommand]
    public async Task SaveAsync()
    {
        int year = 0;
        if (!string.IsNullOrWhiteSpace(YearText) && !int.TryParse(YearText, out year))
        {
            WeakReferenceMessenger.Default.Send(new ShowNotificationMessage("Ano inválido.", NotificationType.Error, 3000));
            return;
        }

        double bpm = 0.0;
        if (!string.IsNullOrWhiteSpace(BpmText) && !double.TryParse(BpmText, NumberStyles.Any, CultureInfo.InvariantCulture, out bpm))
        {
            WeakReferenceMessenger.Default.Send(new ShowNotificationMessage("BPM inválido.", NotificationType.Error, 3000));
            return;
        }

        _originalSong.Name = Name;
        _originalSong.Artist = Artist;
        _originalSong.Year = year;
        _originalSong.Bpm = bpm;
        _originalSong.Instrument = SelectedInstrument;
        _originalSong.Difficulty = SelectedDifficulty;
        _originalSong.Tuning = SelectedTuning;
        _originalSong.Progress = SelectedProgress;
        _originalSong.UserAnnotations = UserAnnotations;
        _originalSong.InfosGeneratedByIA = InfosGeneratedByIA;

        await _songService.UpdateAsync(_originalSong); 

        WeakReferenceMessenger.Default.Send(new ShowNotificationMessage("Música atualizada com sucesso!", NotificationType.Success, 4000));
        WeakReferenceMessenger.Default.Send(new NavigateToHomeMessage());
    }

    [RelayCommand]
    public async Task DeleteAsync()
    {
        await _songService.DeleteAsync(_originalSong.Id);

        WeakReferenceMessenger.Default.Send(new ShowNotificationMessage("Música excluída.", NotificationType.Information, 4000));
        WeakReferenceMessenger.Default.Send(new NavigateToHomeMessage());
    }

    [RelayCommand]
    public void OpenStudio()
    {
        WeakReferenceMessenger.Default.Send(new NavigateToStudioMessage(_originalSong));
    }
}