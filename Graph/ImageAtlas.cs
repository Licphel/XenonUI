using XenonUI.Graph.IMP;

namespace XenonUI.Graph;

public interface ImageAtlas
{

    public void Begin();

    public ImageRegion Accept(Image tex);

    public void End();

}