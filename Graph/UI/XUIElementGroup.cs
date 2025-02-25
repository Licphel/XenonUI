using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public interface XUIElementGroup
{

    List<XElement> Values { get; }

    T Join<T>(T component) where T : XElement;

    void Remove(XElement stru);

    void Ascend(XElement stru);

    void UpdateComponents(VaryingVector2 cursor, Vector2 tls);

}