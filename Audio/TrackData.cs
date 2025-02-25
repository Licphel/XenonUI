namespace XenonUI.Audio;

public interface TrackData
{

    public TimeSpan Length { get; }

    public Track NewTrack();

}