using Jacobi.Vst.Core;
using Jacobi.Vst.Core.Host;

namespace SongsInLearning.Services;

public class VstHostCommandStub : IVstHostCommandStub, IVstHostCommands20
{
    public IVstPluginContext PluginContext { get; set; }

    public IVstHostCommands20 Commands => (IVstHostCommands20)this;
    public int GetSampleRate() => 44100;
    public int GetBlockSize() => 1024;
    public double GetSampleLocation() => 0;


    public string GetProductString() => "SongsInLearning Host";
    public bool BeginEdit(int index) => false;
    public VstCanDoResult CanDo(string cando) => VstCanDoResult.Unknown;
    public bool CloseFileSelector(VstFileSelect fileSelect) => false;
    public bool EndEdit(int index) => false;
    public string GetDirectory() => string.Empty;
    public int GetLanguage() => 1; 
    public VstProcessLevels GetProcessLevel() => VstProcessLevels.Unknown;
    public VstTimeInfo GetTimeInfo(VstTimeInfoFlags filterFlags) => null!;
    public int GetVersion() => 2400;
    public bool IoChanged() => false;
    public bool OpenFileSelector(VstFileSelect fileSelect) => false;
    public bool ProcessEvents(VstEvent[] events) => false;
    public bool SizeWindow(int width, int height) => false;
    public bool UpdateDisplay() => false;

    float IVstHostCommands20.GetSampleRate() => 44100;

    public int GetInputLatency() => 0;

    public int GetOutputLatency() => 0;

    public VstAutomationStates GetAutomationState() => VstAutomationStates.Off;
    public string GetVendorString() => "Meu Estúdio";

    public string GetVendorVersion() => "1.0.0";

    VstHostLanguage IVstHostCommands20.GetLanguage() => VstHostLanguage.English;

    public void SetParameterAutomated(int index, float value)
    {
    }

    public int GetCurrentPluginID() => 0;

    public void ProcessIdle()
    {
    }

    int IVstHostCommands20.GetVendorVersion() => 1;
}