namespace XenonUI.Graph;

public interface ShaderAttrib
{

    public enum Type
    {

        UByte,
        Int,
        Float,
        
    }

    void Pointer(Type type, int components, int stride, int offset);

}