using System.Numerics;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace RenData;

[StructLayout(LayoutKind.Sequential)]
public struct Matrix3
{
    public FixedArray3<Vector3> Row;
}

[StructLayout(LayoutKind.Sequential)]
public struct Matrix3D
{
    public FixedArray3<Vector4> Row;
}

[StructLayout(LayoutKind.Sequential)]
public struct OBBoxClass
{
    public Vector3 Center;
    public Vector3 Extent;
    public Matrix3 Basis;
}

[StructLayout(LayoutKind.Sequential)]
public struct FixedArray3<T> where T: unmanaged
{
    public T _0;
    public T _1;
    public T _2;

    public ref T this[int index]
    {
        get => ref MemoryMarshal.CreateSpan(ref _0, 3)[index];
    }

    public readonly int Length => 3;
    public Span<T> AsSpan() => MemoryMarshal.CreateSpan(ref _0, 3);
    public ReadOnlySpan<T> AsReadOnlySpan() => MemoryMarshal.CreateReadOnlySpan(ref Unsafe.AsRef(in _0), 3);

    public override string ToString()
        => $"[{_0}, {_1}, {_2}]";
}

//[StructLayout(LayoutKind.Sequential)]
//public struct FixedArray3<T>
//{
//    private T[] _items;

//    public FixedArray3(T item1, T item2, T item3)
//    {
//        _items = new T[3];
//        _items[0] = item1;
//        _items[1] = item2;
//        _items[2] = item3;
//    }

//    public FixedArray3()
//    {
//        _items = new T[3];
//    }

//    public T this[int index]
//    {
//        readonly get
//        {
//            if (index < 0 || index >= 3)
//                throw new IndexOutOfRangeException("Index must be between 0 and 2.");
//            return _items[index];
//        }
//        set
//        {
//            if (index < 0 || index >= 3)
//                throw new IndexOutOfRangeException("Index must be between 0 and 2.");
//            _items[index] = value;
//        }
//    }

//    public readonly int Length => _items.Length;

//    public override string ToString()
//    {
//        return $"[{_items[0]}, {_items[1]}, {_items[2]}]";
//    }
//}