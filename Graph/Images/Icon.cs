namespace XenonUI.Graph.Images;

public interface Icon
{

    bool IsFixedSize { get; }
    int Width { get; }
    int Height { get; }

    void InternalPerform(Graphics graphics, float x, float y, float w, float h);

}