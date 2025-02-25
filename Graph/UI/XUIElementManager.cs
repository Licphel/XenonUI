using XenonUI.Maths;

namespace XenonUI.Graph.UI;

public class XUIElementManager : XUIElementGroup
{

	public List<XElement> Components = new List<XElement>();
	public readonly List<XElement> ToRemove = new List<XElement>();
	public readonly List<XElement> ToAdd = new List<XElement>();

	public T Join<T>(T component) where T : XElement
	{
		ToAdd.Add(component);
		return component;
	}

	public void Remove(XElement stru)
	{
		ToRemove.Add(stru);
	}

	//Set the index to the top to let it get displayed firstly to users.
	//Will cause id gap. but it doesn't matter.
	public void Ascend(XElement stru)
	{
		ToRemove.Add(stru);
		ToAdd.Add(stru);
	}

	public void UpdateComponents(VaryingVector2 cursor, Vector2 tls)
	{
		foreach(XElement comp in Components)
		{
			comp.Bound.Translate(tls.x, tls.y);
			comp.Cursor.Copy(cursor);//do not override it
			comp.Update();
			if(comp.Removed)
				Remove(comp);
			comp.Bound.Translate(-tls.x, -tls.y);
		}

		foreach(XElement c in ToRemove)
		{
			Components.Remove(c);
		}

		foreach(XElement component in ToAdd)
		{
			Components.Add(component);
			component.Parent = this;
		}

		ToRemove.Clear();
		ToAdd.Clear();
	}

	public List<XElement> Values => Components;

}
