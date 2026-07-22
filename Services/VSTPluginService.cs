using Jacobi.Vst.Core;
using Jacobi.Vst.Host.Interop;
using System;
using System.Linq;

namespace SongsInLearning.Services;

public class VstPluginService : IDisposable
{
    private VstPluginContext? _pluginContext;

    public bool IsPluginLoaded => _pluginContext != null;

    public string PluginName
    {
        get
        {
            if (_pluginContext == null) return "Nenhum Plugin";
            return _pluginContext.PluginCommandStub.Commands.GetEffectName() ?? "Desconhecido";
        }
    }

    public void LoadPlugin(string dllPath)
    {
        try
        {
            Dispose();

            var hostStub = new VstHostCommandStub();

            _pluginContext = VstPluginContext.Create(dllPath, hostStub);
            hostStub.PluginContext = _pluginContext;

            var commands = _pluginContext.PluginCommandStub.Commands;

            commands.Open();
            commands.SetSampleRate(44100f);
            commands.SetBlockSize(1024);
            commands.MainsChanged(true);

            string product = commands.GetProductString() ?? commands.GetEffectName() ?? "Plugin VST";
            string vendor = commands.GetVendorString() ?? "Fabricante";

            Console.WriteLine($"[VST] {product} da {vendor} carregado com sucesso!");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[VST] Erro fatal ao carregar {dllPath}: {ex.Message}");
            _pluginContext = null;
        }
    }

    public void Dispose()
    {
        if (_pluginContext != null)
        {
            try
            {
                var commands = _pluginContext.PluginCommandStub.Commands;
                commands.MainsChanged(false);
                commands.Close();
                _pluginContext.Dispose();
            }
            catch
            {
            }
            finally
            {
                _pluginContext = null;
            }
        }
    }

    public byte[] ProcessAudio(byte[] inputBytes, int bytesRecorded)
    {
        if (_pluginContext == null) return inputBytes;

        int sampleCount = bytesRecorded / 2; 
        int inputChannels = _pluginContext.PluginInfo.AudioInputCount;
        int outputChannels = _pluginContext.PluginInfo.AudioOutputCount;

        if (inputChannels == 0 || outputChannels == 0) return inputBytes;

        float[] floatInput = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            short sample = BitConverter.ToInt16(inputBytes, i * 2);
            floatInput[i] = sample / 32768f;
        }

        using var inBufMgr = new VstAudioBufferManager(inputChannels, sampleCount);
        using var outBufMgr = new VstAudioBufferManager(outputChannels, sampleCount);

        var inBuffers = inBufMgr.Buffers.ToArray();
        var outBuffers = outBufMgr.Buffers.ToArray();

        for (int c = 0; c < inputChannels; c++)
        {
            for (int i = 0; i < sampleCount; i++)
            {
                inBuffers[c][i] = floatInput[i];
            }
        }

        _pluginContext.PluginCommandStub.Commands.ProcessReplacing(inBuffers, outBuffers);

        float[] floatOutput = new float[sampleCount];
        for (int i = 0; i < sampleCount; i++)
        {
            floatOutput[i] = outBuffers[0][i];
        }

        byte[] outputBytes = new byte[bytesRecorded];
        for (int i = 0; i < sampleCount; i++)
        {
            float sample = floatOutput[i];

            if (sample > 1.0f) sample = 1.0f;
            if (sample < -1.0f) sample = -1.0f;

            short shortSample = (short)(sample * 32767f);
            byte[] sampleBytes = BitConverter.GetBytes(shortSample);
            outputBytes[i * 2] = sampleBytes[0];
            outputBytes[i * 2 + 1] = sampleBytes[1];
        }

        return outputBytes; 
    }
}