using XenonUI.Core;
using XenonUI.Graph.IMP;

namespace XenonUI.Graph;

public class IconAnim : Icon
{

    private readonly int maxIndex;
    private readonly TimeSchedule schedule = new TimeSchedule();

    private readonly ImageRegion[] stream;
    private float frameLen;
    private int index;

    public IconAnim(ImageRegion tex, int count, int w, int h, int u, int v)
    {
        maxIndex = count;
        stream = new ImageRegion[maxIndex];

        for(var i = 0; i < maxIndex; i++)
            stream[i] = ImageRegion.BySize(tex, i * w + u, v, w, h);
    }

    public IconAnim(Image tex, int count, int w, int h, int u, int v)
    {
        maxIndex = count;
        stream = new ImageRegion[maxIndex];

        for(var i = 0; i < maxIndex; i++)
            stream[i] = ImageRegion.BySize(tex, i * w + u, v, w, h);
    }

    public IconAnim(params ImageRegion[] parts)
    {
        stream = parts;
    }

    public void Draw(Graphics graphics, float x, float y, float w, float h)
    {
        if(schedule.PeriodicTaskChecked(frameLen))
        {
            index++;
            if(index >= maxIndex)
                index = 0;
        }

        graphics.Draw(stream[index], x, y, w, h);
    }

    public IconAnim Seconds(float time)
    {
        frameLen = time;
        return this;
    }

    public void Reset()
    {
        index = 0;
    }

    public float GetTimePerCycle()
    {
        return frameLen * (maxIndex - 1);
    }

}