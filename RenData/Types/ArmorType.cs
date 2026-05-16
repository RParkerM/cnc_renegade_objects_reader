using System.Runtime.InteropServices;

namespace RenData.Types;

[StructLayout(LayoutKind.Explicit)]
public readonly struct ArmorType(uint v)
{
    [FieldOffset(0)]
    public readonly uint Value = v;

    public static implicit operator uint(ArmorType id) => id.Value;
    public static implicit operator ArmorType(uint v) => new(v);
}
