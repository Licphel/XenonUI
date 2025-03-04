namespace XenonUI.Graph.Images;

public class IconFixedDelegate : Icon
{

    private Action<Graphics, float, float, float, float> action;

    public IconFixedDelegate(Action<Graphics, float, float, float, float> action, int width, int height)
    {
        this.action = action;
        Width = width;
        Height = height;
    }

    public bool IsFixedSize => true;
    public int Width { get; }
    public int Height { get; }
    
    public void InternalPerform(Graphics graphics, float x, float y, float w, float h)
    {
        action(graphics, x, y, w, h);
    }

}