namespace XenonUI.Audio;

public interface TrackData
{

    public Track NewTrack();
    public TimeSpan Length { get; }
    
}