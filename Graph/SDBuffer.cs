using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using KryptonM;
using XenonUI.Maths;
using static System.Buffer;

namespace XenonUI.Graph;

public unsafe class SDBuffer
{
    
    //For quads drawing.
    public uint* Indices;
    public int IndexCap;
    public int Index;
    
    public byte* Vertices;
    public byte* Vertices0;
    public int VertexCap;
    public int Size => (int)(Vertices - Vertices0);

    public void SetVertexCapacity(int size)
    {
        Vertices0 = Vertices = NativeMem.MemAllocate<byte>(VertexCap = size, true);
        Indices = NativeMem.MemReallocate(Indices, IndexCap = size / 2 * 3, 0);

        for(uint i = 0, k = 0; i < size / 4; i += 6, k += 4)
        {
            Indices[i + 0] = 0 + k;
            Indices[i + 1] = 1 + k;
            Indices[i + 2] = 3 + k;
            Indices[i + 3] = 1 + k;
            Indices[i + 4] = 2 + k;
            Indices[i + 5] = 3 + k;
        }
    }

    public void Clear()
    {
        Vertices = Vertices0;
        Index = 0;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SDBuffer Append<T>(T v) where T : unmanaged
    {
        int s = sizeof(T);
        NativeMemory.Copy((byte*)&v, Vertices, (UIntPtr)s);
        Vertices += s;
        return this;
    }
    
    [MethodImpl(MethodImplOptions.AggressiveInlining)]
    public SDBuffer NewIndex(int i)
    {
        Index += i;
        return this;
    }

}