using XenonUI.Maths;

namespace XenonUI.Graph;

public class MatrixStack
{

	private readonly Stack<Transform> Stack = new Stack<Transform>();

	public bool Changed;

	private Transform[] TempMats;
	public Transform Top = new Transform();

	public MatrixStack()
	{
		RecreateMatsForLen(128);
		Stack.Push(Top);
	}

	public bool IsEmpty => Stack.Count == 1;//Top0 is never removed.

	public void RecreateMatsForLen(int len)
	{
		if(TempMats == null || TempMats.Length < len)
		{
			TempMats = new Transform[len];
			for(int i = 0; i < TempMats.Length; i++) TempMats[i] = new Transform();
		}
	}

	public void Push()
	{
		int take = Stack.Count;
		if(take < TempMats.Length)
		{
			Transform aff = TempMats[take];
			aff.Identity();
			Push(aff);
			return;
		}

		Push(new Transform());
	}

	private void Push(Transform aff)
	{
		Top = aff;
		Top.Set(Stack.Peek());
		Stack.Push(Top);
		Changed = true;
	}

	public void Pop()
	{
		Stack.Pop();
		Top = Stack.Peek();
		Changed = true;
	}

	public void Load(Transform Transform)
	{
		Top.Set(Transform);
		Changed = true;
	}

	public void RotateRad(float f)
	{
		Top.Rotate(f);
		Changed = true;
	}

	public void RotateDeg(float f)
	{
		Top.Rotate(FloatMath.Rad(f));
		Changed = true;
	}

	public void RotateRad(float f, float x, float y)
	{
		Translate(x, y);
		RotateRad(f);
		Translate(-x, -y);
	}

	public void RotateDeg(float f, float x, float y)
	{
		Translate(x, y);
		RotateDeg(f);
		Translate(-x, -y);
	}

	public void Translate(float x, float y)
	{
		Top.Translate(x, y);
		Changed = true;
	}

	public void Scale(float x, float y)
	{
		Top.Scale(x, y);
		Changed = true;
	}

}
