using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public interface XUIElementGroup
{

	T Join<T>(T component) where T : XElement;

	void Remove(XElement stru);

	void Ascend(XElement stru);

	void UpdateComponents(VaryingVector2 cursor, Vector2 tls);

	List<XElement> Values { get; }

}
