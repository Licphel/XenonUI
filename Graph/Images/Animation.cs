using XenonUI.Core;

namespace XenonUI.Graph.Images;

public class Animation : Icon
{

    private readonly int maxIndex;
    private readonly TimeSchedule schedule = new TimeSchedule();

    private readonly Image[] stream;
    private float frameLen;
    private int index;

    public Animation(Image tex, int count, int w, int h, int u, int v)
    {
        if(count <= 0)
            throw new Exception("Animation at least should have one frame.");
        
        maxIndex = count;
        stream = new Image[maxIndex];

        for(int i = 0; i < maxIndex; i++)
            stream[i] = Image.Subimage(tex, i * w + u, v, w, h);
    }

    public Animation(params Image[] parts)
    {
        if(parts.Length <= 0)
            throw new Exception("Animation at least should have one frame.");
        stream = parts;
    }

    public bool IsFixedSize => true;
    public int Width => stream[0].Width;
    public int Height => stream[0].Height;

    public void InternalPerform(Graphics graphics, float x, float y, float w, float h)
    {
        if(schedule.PeriodicTaskChecked(frameLen))
        {
            index++;
            if(index >= maxIndex)
                index = 0;
        }

        graphics.DrawImage(stream[index], x, y, w, h);
    }

    public Animation Seconds(float time)
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