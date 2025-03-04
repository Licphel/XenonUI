using KryptonM.IO;
using XenonUI.XOpenAL;

namespace XenonUI.Audio;

public abstract class AudioDevice
{

    public static AudioDevice Current;
    
    public const string OpenAL = "OpenAL";
    
    public string Name = "Unknown";
    
    public static AudioDevice GetLocalDevice(string name)
    {
        if(Current != null)
        {
            if(Current.Name != name)
                throw new Exception("Current device is invalid.");
            return Current;
        }
        switch(name)
        {
            case OpenAL:
                return Current = new OAL_AudioDevice() { Name = OpenAL };
        }

        throw new Exception("Unknown backend name.");
    }

    public abstract TrackData NewTrackData(FileHandle file);

}