using XenonUI.Graph.Images;

namespace XenonUI.Graph;

public abstract class SDContext
{

    /// <summary>
    /// Get a standard buffer with a default shader ready for use.
    /// Viewports and scissors are not set, though.
    /// </summary>
    public abstract SDBuffer GetBufferFromType(Image img = null);
    
    /// <summary>
    /// Send the specific buffer to gpu and show it.
    /// </summary>
    public abstract void DrawBuffer(SDBuffer buf);

    public abstract void SwapDoubleBuffers();

}