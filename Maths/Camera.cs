namespace XenonUI.Maths;

public class Camera
{

    private readonly VaryingVector2 tmp = new VaryingVector2();

    public Vector2 Center;
    public Transform CombinedTransform = new Transform();
    public Transform InvertedTransform = new Transform();
    public Transform ProjectionTransform = new Transform();
    public Transform TranslationTransform = new Transform();
    public Angle Rotation = Angle.Radian(0);
    public float ScaleX = 1.0f;
    public float ScaleY = 1.0f;
    public bool ShouldSeeCenter;
    public Rect Viewport = new Rect();

    public void Push()
    {
        if(ShouldSeeCenter) Center = new Vector2(Viewport.w / 2.0f, Viewport.h / 2.0f);

        float hw = Viewport.w / 2.0f;
        float hh = Viewport.h / 2.0f;
        ProjectionTransform.Orthographic(ScaleX * -hw, ScaleX * hw, ScaleY * -hh, ScaleY * hh);
        TranslationTransform.Identity();
        TranslationTransform.Rotate(Rotation);
        TranslationTransform.Translate(-Center.x, -Center.y);
        CombinedTransform.Load(ProjectionTransform);
        CombinedTransform.Product(TranslationTransform);
        InvertedTransform.Load(CombinedTransform);
        InvertedTransform.Invert();
        ShouldSeeCenter = false;
    }

    public void ToCenter()
    {
        ShouldSeeCenter = true;
    }

    public void Project(VaryingVector2 v, Vector4 scrCoords)
    {
        CombinedTransform.ApplyTo(v);
        v.x = scrCoords.z * (v.x + 1) / 2 + scrCoords.x;
        v.y = scrCoords.w * (v.y + 1) / 2 + scrCoords.y;
    }

    public void Unproject(VaryingVector2 v, Vector4 scrCoords)
    {
        v.x = 2.0f * (v.x - scrCoords.x) / scrCoords.z - 1.0f;
        v.y = 2.0f * (v.y - scrCoords.y) / scrCoords.w - 1.0f;
        InvertedTransform.ApplyTo(v);
    }

    public float ToWldX(float x, Vector4 scrCoords)
    {
        tmp.x = x;
        tmp.y = 0;
        Unproject(tmp, scrCoords);
        return tmp.x;
    }

    public float ToWldY(float y, Vector4 scrCoords)
    {
        tmp.y = y;
        tmp.x = 0;
        Unproject(tmp, scrCoords);
        return tmp.y;
    }

    public float ToScrX(float x, Vector4 scrCoords)
    {
        tmp.x = x;
        tmp.y = 0;
        Project(tmp, scrCoords);
        return tmp.x;
    }

    public float ToScrY(float y, Vector4 scrCoords)
    {
        tmp.y = y;
        tmp.x = 0;
        Project(tmp, scrCoords);
        return tmp.y;
    }

}