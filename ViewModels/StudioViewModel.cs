using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NAudio.Wave;
using SongsInLearning.Models;
using SongsInLearning.Services;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Threading.Tasks;

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

    private readonly VstPluginService _vstPluginService;

    [ObservableProperty]
    private string _loadedPluginName = "Nenhum plugin carregado";

    private string _backingTrackFilePath = string.Empty;
    private WaveOutEvent? _backingTrackOut;
    private AudioFileReader? _backingTrackReader;

    [ObservableProperty]
    private string _backingTrackName = "Nenhuma trilha";

    public ObservableCollection<double> BackingTrackPeaks { get; } = new();
    public StudioViewModel(Song song, VstPluginService vstPluginService)
    {
        CurrentSong = song;
        _vstPluginService = vstPluginService;

        string appFolder = Path.Combine("D:", "SongsInLearning", "Audio");
        Directory.CreateDirectory(appFolder);

        _audioFilePath = Path.Combine(appFolder, $"Song_{CurrentSong.Id}_Track1.wav");
        LoadInputDevices();
    }

    public void LoadVstPlugin(string dllPath)
    {
        _vstPluginService.LoadPlugin(dllPath);
        LoadedPluginName = _vstPluginService.PluginName;
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
        if (!File.Exists(_audioFilePath))
        {
            return;
        }

        try
        {
            _audioFileReader = new AudioFileReader(_audioFilePath);
            _waveOut = new WaveOutEvent();

            _waveOut.Init(_audioFileReader);
            _waveOut.PlaybackStopped += OnPlaybackStopped;

            PlayBackingTrack();
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
        StopBackingTrack();
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
            PlayBackingTrack();
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
        StopBackingTrack();
        IsRecording = false;
    }

    private void OnDataAvailable(object? sender, WaveInEventArgs e)
    {
        byte[] processedAudio = _vstPluginService.ProcessAudio(e.Buffer, e.BytesRecorded);

        if (_waveFileWriter != null)
        {
            _waveFileWriter.Write(processedAudio, 0, e.BytesRecorded);
            _waveFileWriter.Flush();
        }

        float max = 0;
        for (int i = 0; i < e.BytesRecorded; i += 2)
        {
            short sample = (short)((processedAudio[i + 1] << 8) | processedAudio[i]);
            var absSample = Math.Abs(sample);
            if (absSample > max) max = absSample;
        }

        double height = (max / 32768.0) * 70.0;
        if (height < 2) height = 2;

        Dispatcher.UIThread.Post(() =>
        {
            try
            {

                WaveformPeaks.Add(height);
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }
        });
    }

    public void LoadBackingTrack(string path)
    {
        _backingTrackFilePath = path;
        BackingTrackName = Path.GetFileName(path);

        Task.Run(() =>
        {
            try
            {
                using var reader = new AudioFileReader(path);
                var peaks = new System.Collections.Generic.List<double>();

                int bars = 300;
                int samplesPerBar = (int)(reader.Length / (reader.WaveFormat.BlockAlign * bars));
                if (samplesPerBar == 0) samplesPerBar = 1;

                float[] buffer = new float[samplesPerBar];
                int read;

                while ((read = reader.Read(buffer, 0, buffer.Length)) > 0)
                {
                    float max = 0;
                    for (int i = 0; i < read; i++)
                    {
                        if (Math.Abs(buffer[i]) > max) max = Math.Abs(buffer[i]);
                    }

                    double height = max * 70.0; 
                    if (height < 2) height = 2;
                    peaks.Add(height);
                }

                Dispatcher.UIThread.Post(() =>
                {
                    BackingTrackPeaks.Clear();
                    foreach (var peak in peaks) BackingTrackPeaks.Add(peak);
                });
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Erro ao gerar onda visual: {ex.Message}");
            }
        });
    }

    private void PlayBackingTrack()
    {
        if (string.IsNullOrEmpty(_backingTrackFilePath)) return;

        try
        {
            _backingTrackOut?.Stop();
            _backingTrackOut?.Dispose();
            _backingTrackReader?.Dispose();

            _backingTrackReader = new AudioFileReader(_backingTrackFilePath);
            _backingTrackOut = new WaveOutEvent();
            _backingTrackOut.Init(_backingTrackReader);
            _backingTrackOut.Play();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Erro ao tocar backing track: {ex.Message}");
        }
    }

    private void StopBackingTrack()
    {
        _backingTrackOut?.Stop();
    }

    private void OnRecordingStopped(object? sender, StoppedEventArgs e)
    {
        _waveIn?.Dispose();
        _waveIn = null;

        _waveFileWriter?.Dispose();
        _waveFileWriter = null;
    }

}