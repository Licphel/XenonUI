using KryptonM.IO;
using XenonUI.Audio;

namespace XenonUI.XOpenAL;

public class OAL_AudioDevice : AudioDevice
{

    public override TrackData NewTrackData(FileHandle file)
    {
        return OAL_TrackData.Read(file);
    }

}