namespace XenonUI.Audio;

/// <summary>
/// A track represents a playable sound instance.
/// </summary>
public interface Track : IDisposable
{

    TrackData Data { get; }
    bool IsPlaying { get; }
    bool IsPaused { get; }
    bool IsDestroyed { get; }

    void Play();
    void Loop();
    void Pause();
    void Resume();
    void Stop();
    void Set(Controller controller, object v);
    object Get(Controller controller);
   
    public enum Controller
    {

        Gain,
        Pitch,
        PlayPos,
        
    }

}