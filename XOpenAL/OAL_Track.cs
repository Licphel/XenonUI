using OpenTK.Audio.OpenAL;
using XenonUI.Audio;
using XenonUI.Maths;

namespace XenonUI.XOpenAL;

public class OAL_Track : Track
{

    public int Id;

    public OAL_Track(OAL_TrackData data)
    {
        Id = AL.GenSource();
        Data = data;

        AL.Source(Id, ALSourcei.Buffer, data.Id);
    }

    public TrackData Data { get; }

    public void Play()
    {
        if(TrackManager.Remind(this)) AL.SourcePlay(Id);
    }

    public void Loop()
    {
        if(TrackManager.Remind(this))
        {
            AL.Source(Id, ALSourceb.Looping, true);
            AL.SourcePlay(Id);
        }
    }

    public void Pause()
    {
        AL.SourcePause(Id);
    }

    public void Resume()
    {
        AL.SourcePlay(Id);
    }

    public void Stop()
    {
        AL.SourceStop(Id);
    }

    public void Set(Track.Controller controller, object v)
    {
        switch(controller)
        {
            case Track.Controller.Gain:
                AL.Source(Id, ALSourcef.Gain, (float)v);
                break;
            case Track.Controller.Pitch:
                AL.Source(Id, ALSourcef.Pitch, (float)v);
                break;
            case Track.Controller.PlayPos:
                Vector3 vec = (Vector3)v;
                AL.Source(Id, ALSource3f.Position, vec.x, vec.y, vec.z);
                break;
        }
    }

    public object Get(Track.Controller controller)
    {
        switch(controller)
        {
            case Track.Controller.Gain:
                AL.GetSource(Id, ALSourcef.Gain, out float v1);
                return v1;
            case Track.Controller.Pitch:
                AL.GetSource(Id, ALSourcef.Pitch, out float v2);
                return v2;
            case Track.Controller.PlayPos:
                AL.GetSource(Id, ALSource3f.Position, out float v3, out float v4, out float v5);
                return new Vector3(v3, v4, v5);
        }

        return null;
    }

    public bool IsPlaying
    {
        get
        {
            AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
            return v == (int)ALSourceState.Playing;
        }
    }

    public bool IsPaused
    {
        get
        {
            AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
            return v == (int)ALSourceState.Paused;
        }
    }

    public bool IsDestroyed
    {
        get
        {
            AL.GetSource(Id, ALGetSourcei.SourceState, out int v);
            return v == (int)ALSourceState.Stopped;
        }
    }

    public void Dispose()
    {
        AL.DeleteSource(Id);
    }

}