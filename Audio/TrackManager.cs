namespace XenonUI.Audio;

/// <summary>
/// A track manager provides functionality to automatically dispose a track when it ends.
/// </summary>
public static class TrackManager
{
    
    public static int MaxTrackCount = 128;

    private static readonly List<Track> tracksPlaying = new List<Track>();
    
    public static void Tick()
    {
        lock(tracksPlaying)
        {
            for(int i = tracksPlaying.Count - 1; i >= 0; i--)
            {
                Track? c = tracksPlaying[i];
                if(!c.IsPlaying && !c.IsPaused)
                {
                    tracksPlaying.RemoveAt(i);
                    c.Dispose();
                }
            }
        }
    }

    /// <summary>
    /// Remind it to dispose a track when it ends. 
    /// </summary>
    /// <param name="track"> A track reference.</param>
    public static bool Remind(Track track)
    {
        lock(tracksPlaying)
        {
            if(tracksPlaying.Count < MaxTrackCount && !tracksPlaying.Contains(track))
            {
                tracksPlaying.Add(track);
                return true;
            }
        }

        return false;
    }

}