using XenonUI.Maths;

namespace XenonUI.Core;

public class TimeSchedule
{

    private int clock;
    private int clocklast;

    public bool PeriodicTaskChecked(float seconds)
    {
        clocklast = clock;
        clock = Time.Ticks;
        if(clock == clocklast)
            return false;
        return PeriodicTask(seconds);
    }

    public static bool PeriodicTask(float seconds)
    {
        if(seconds < 0)
            throw new Exception("Time is negative.");
        if(seconds == 0)
            return true;
        
        int ticks = Mathf.Round(seconds * Application.MaxTps);
        return Time.Ticks % ticks == 0;
    }

}