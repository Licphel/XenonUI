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
		int ticks = FloatMath.Round(seconds * Application.MaxTps);
		return Time.Ticks % ticks == 0;
	}

}
