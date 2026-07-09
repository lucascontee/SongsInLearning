using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using SongsInLearning.Models;
using System;
using System.Collections.ObjectModel;
using System.IO;

namespace SongsInLearning.ViewModels;

public partial class StudioViewModel : ViewModelBase
{
    [ObservableProperty]
    private Song _currentSong;

    [ObservableProperty]
    private bool _isRecording;
    [ObservableProperty]
    private bool _isPlaying;

    private WaveInEvent? _waveIn;
    private WaveFileWriter? _waveFileWriter;
    private WaveOutEvent? _waveOut;
    private AudioFileReader? _audioFileReader;
    private string _audioFilePath = string.Empty;

    public ObservableCollection<double> WaveformPeaks { get; } = new();

    public ObservableCollection<string> AvailableInputDevices { get; } = new();
    [ObservableProperty]
    private int _selectedDeviceIndex = 0;

    public StudioViewModel(Song song)
    {
        CurrentSong = song;

        string appFolder = Path.Combine("D:", "SongsInLearning", "Audio");
        Directory.CreateDirectory(appFolder);

        _audioFilePath = Path.Combine(appFolder, $"Song_{CurrentSong.Id}_Track1.wav");
        LoadInputDevices();
    }

    [RelayCommand]
    public void TogglePlayback()
    {
        if (IsRecording) return;

        if (IsPlaying)
            StopPlayback();
        else
            StartPlayback();
    }

    private void StartPlayback()
    {
        // Verifica se o arquivo de áudio realmente existe antes de tentar tocar
        if (!File.Exists(_audioFilePath))
        {
            // Opcional: Você pode disparar uma notificação aqui avisando que não há gravação
            return;
        }

        try
        {
            _audioFileReader = new AudioFileReader(_audioFilePath);
            _waveOut = new WaveOutEvent();

            _waveOut.Init(_audioFileReader);
            _waveOut.PlaybackStopped += OnPlaybackStopped;

            _waveOut.Play();
            IsPlaying = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao reproduzir: {ex.Message}");
        }
    }

    private void StopPlayback()
    {
        _waveOut?.Stop();
    }

    private void OnPlaybackStopped(object? sender, StoppedEventArgs e)
    {

        Dispatcher.UIThread.Post(() =>
        {
            IsPlaying = false;

            _waveOut?.Dispose();
            _waveOut = null;

            _audioFileReader?.Dispose();
            _audioFileReader = null;
        });
    }

    private void LoadInputDevices()
    {
        AvailableInputDevices.Clear();

        for (int i = 0; i < WaveInEvent.DeviceCount; i++)
        {
            var capabilities = WaveInEvent.GetCapabilities(i);
            AvailableInputDevices.Add(capabilities.ProductName);
        }

        if (AvailableInputDevices.Count > 0)
        {
            SelectedDeviceIndex = 0;
        }
    }

    [RelayCommand]
    public void ToggleRecording()
    {
        if (IsPlaying) return;
        if (IsRecording)
        {
            StopRecording();
        }
        else
        {
            StartRecording();
        }
    }

    private void StartRecording()
    {
        try
        {
            WaveformPeaks.Clear();
            _waveIn = new WaveInEvent();
            _waveIn.DeviceNumber = SelectedDeviceIndex;
            _waveIn.WaveFormat = new WaveFormat(44100, 1);

            _waveIn.DataAvailable += OnDataAvailable;
            _waveIn.RecordingStopped += OnRecordingStopped;

            _waveFileWriter = new WaveFileWriter(_audioFilePath, _waveIn.WaveFormat);

            _waveIn.StartRecording();
            IsRecording = true;
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao gravar: {ex.Message}");
        }
    }

    private void StopRecording()
    {
        _waveIn?.StopRecording();
        IsRecording = false;
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        if (_waveFileWriter != null)
        {
            _waveFileWriter.Write(e.Buffer, 0, e.BytesRecorded);
            _waveFileWriter.Flush();
        }

        float max = 0;


        for (int i = 0; i < e.BytesRecorded; i += 2)
        {
            short sample = (short)((e.Buffer[i + 1] << 8) | e.Buffer[i]);
            var absSample = Math.Abs(sample);
            if (absSample > max) max = absSample;
        }

        double height = (max / 32768.0) * 70.0;

        if (height < 2) height = 2;

        Dispatcher.UIThread.Post(() =>
        {
            WaveformPeaks.Add(height);
        });
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs e)
    {
        _waveIn?.Dispose();
        _waveIn = null;

        _waveFileWriter?.Dispose();
        _waveFileWriter = null;
    }

}