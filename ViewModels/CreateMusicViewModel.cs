using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FluentValidation.Results;
using Microsoft.Extensions.Logging;
using SongsInLearning.Models;
using SongsInLearning.Models.Enums;
using SongsInLearning.Services;
using SongsInLearning.Validators;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;


namespace SongsInLearning.ViewModels;

public partial class CreateMusicViewModel : ObservableObject
{
    private readonly ILogger<CreateMusicViewModel> _logger;
    private readonly MusicService _musicService;

    public CreateMusicViewModel(ILogger<CreateMusicViewModel> logger, MusicService musicService)
    {
        _logger = logger;
        _musicService = musicService;

        Difficulties = Enum.GetValues<Difficulty>().ToList();
        Tunings = Enum.GetValues<Tuning>().ToList();
        Instruments = Enum.GetValues<Instrument>().ToList();
        Progresses = Enum.GetValues<Progress>().ToList();
    }

    [ObservableProperty] private string name = string.Empty;
    [ObservableProperty] private string artist = string.Empty;
    [ObservableProperty] private string yearText;
    [ObservableProperty] private string bpmText;
    [ObservableProperty] private Difficulty selectedDifficulty = Difficulty.Medium;
    [ObservableProperty] private Tuning selectedTuning = Tuning.DefaultTuning;
    [ObservableProperty] private Instrument selectedInstrument = Instrument.Guitar;
    [ObservableProperty] private Progress selectedProgress = Progress.Learn;
    [ObservableProperty] private string userAnnotations = string.Empty;

    public List<Difficulty> Difficulties { get; }
    public List<Tuning> Tunings { get; }
    public List<Instrument> Instruments { get; }
    public List<Progress> Progresses { get; }

    [RelayCommand]
    public async Task Save()
    {
        int year = 0;
        double bpm = 0.0;

        if (!string.IsNullOrWhiteSpace(YearText) && int.TryParse(YearText, out int parsedYear))
        {
            year = parsedYear;
        }
        else
        {
           
            //await Shell.Current.DisplayAlert("Erro de Validação", "Por favor, insira um ano válido (número inteiro).", "OK");
            return; 
        }

        if (!string.IsNullOrWhiteSpace(BpmText) && double.TryParse(BpmText, NumberStyles.Any, CultureInfo.InvariantCulture, out double parsedBpm))
        {
            bpm = parsedBpm;
        }
        else
        {
            //await Shell.Current.DisplayAlert("Erro de Validação", "Por favor, insira um BPM válido (número, use '.' para decimais se houver).", "OK");
            return;
        }

        var music = new Music
        {
            Name = Name,
            Artist = Artist,
            Year = year,
            Bpm = bpm,
            Difficulty = SelectedDifficulty,
            Tuning = SelectedTuning,
            Instrument = SelectedInstrument,
            UserAnnotations = UserAnnotations,
            Progress = SelectedProgress,
            CreatedAt = DateTime.UtcNow
        };

        var validator = new MusicValidator();
        ValidationResult result = validator.Validate(music);

        if (!result.IsValid)
        {
            //await Application.Current.MainPage.DisplayAlert("Erro", result.Errors.First().ErrorMessage, "OK");
            return;
        }

        await _musicService.AddAsync(music);
        //await Application.Current.MainPage.DisplayAlert("Sucesso", "Música salva com sucesso!", "OK");


        Name = string.Empty;
        Artist = string.Empty;
        YearText = string.Empty;
        BpmText = string.Empty;
        UserAnnotations = string.Empty;
        SelectedDifficulty = Difficulty.Medium;
        SelectedTuning = Tuning.DefaultTuning;
        SelectedInstrument = Instrument.Guitar;
        SelectedProgress = Progress.Learn;
    }
}
