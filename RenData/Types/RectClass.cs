using System.Numerics;
using System.Runtime.InteropServices;

namespace RenData.Types;

[StructLayout(LayoutKind.Sequential)]
public struct RectClassStruct
{
    public float Left;
    public float Top;
    public float Right;
    public float Bottom;

    public RectClassStruct(float left, float top, float right, float bottom)
        => (Left, Top, Right, Bottom) = (left, top, right, bottom);
    public static implicit operator RectClassStruct(RectClass c) =>
        c is null ? default : new RectClassStruct(c.Left, c.Top, c.Right, c.Bottom);
}
public class RectClass
{
    public float Left;
    public float Top;
    public float Right;
    public float Bottom;

    // Constructors
    public RectClass() {}
    public RectClass(in RectClass r) { Left = r.Left; Top = r.Top; Right = r.Right; Bottom = r.Bottom; }
    public RectClass(float left, float top, float right, float bottom) { Left = left; Top = top; Right = right; Bottom = bottom; }
    public RectClass(in Vector2 top_left, in Vector2 bottom_right) { Left = top_left.X; Top = top_left.Y; Right = bottom_right.X; Bottom = bottom_right.Y; }

    // Assignment

    public void Set(float left, float top, float right, float bottom) { Left = left; Top = top; Right = right; Bottom = bottom; }
    public void Set(in Vector2 top_left, in Vector2 bottom_right) { Left = top_left.X; Top = top_left.Y; Right = bottom_right.X; Bottom = bottom_right.Y; }
    public void Set(in RectClass r) { Left = r.Left; Top = r.Top; Right = r.Right; Bottom = r.Bottom; }
    public void Set(in RectClassStruct r) { Left = r.Left; Top = r.Top; Right = r.Right; Bottom = r.Bottom; }

    // Access
    public float Width() { return Right - Left; }
    public float Height() { return Bottom - Top; }
    public Vector2 Center() { return new Vector2((Left + Right) / 2, (Top + Bottom) / 2); }
    public Vector2 Extent() { return new Vector2((Right - Left) / 2, (Bottom - Top) / 2); }
    public Vector2 Upper_Left() { return new Vector2(Left, Top); }
    public Vector2 Lower_Right() { return new Vector2(Right, Bottom); }
    public Vector2 Upper_Right() { return new Vector2(Right, Top); }
    public Vector2 Lower_Left() { return new Vector2(Left, Bottom); }

    // Scaling
    //RectClass & operator *=(float k) { return Scale(k); }
    //public RectClass & operator /=(float k) { return Scale(1 / k); }
    //public RectClass & Scale_Relative_Center(float k) { Vector2 center = Center(); *this -= center; Left *= k; Top *= k; Right *= k; Bottom *= k; *this += center; return *this; }
    //public RectClass & Scale(float k) { Left *= k; Top *= k; Right *= k; Bottom *= k; return *this; }
    //public RectClass & Scale( const Vector2 &k ) { Left *= k.X; Top *= k.Y; Right *= k.X; Bottom *= k.Y; return *this; }
    //public RectClass & Inverse_Scale(in Vector2 k) { Left /= k.X; Top /= k.Y; Right /= k.X; Bottom /= k.Y; return *this; }

    // Offset
    //RectClass & operator +=( const Vector2 & o ) { Left += o.X; Top += o.Y; Right += o.X; Bottom += o.Y; return *this; }
    //RectClass & operator -=( const Vector2 & o ) { Left -= o.X; Top -= o.Y; Right -= o.X; Bottom -= o.Y; return *this; }

    // Inflate
    //void Inflate( const Vector2 & o ) { Left -= o.X; Top -= o.Y; Right += o.X; Bottom += o.Y; }

    // Union
    //RectClass & operator +=( const RectClass & r ) { Left = MIN(Left, r.Left); Top = MIN(Top, r.Top); Right = MAX(Right, r.Right); Bottom = MAX(Bottom, r.Bottom); return *this; }

    // Equality
    //bool operator ==( const RectClass &rval ) const { return (rval.Left == Left) && (rval.Right == Right) && (rval.Top == Top) && (rval.Bottom == Bottom); }
    //bool operator !=( const RectClass &rval ) const { return (rval.Left != Left) || (rval.Right != Right) || (rval.Top != Top) || (rval.Bottom != Bottom); }

    // Containment
    public bool Contains(in Vector2 pos) { return (pos.X >= Left) && (pos.X <= Right) && (pos.Y >= Top) && (pos.Y <= Bottom); }

    // Misc
    public void Snap_To_Units(in Vector2 u) { Left = (int)(Left / u.X + 0.5f) * u.X; Right = (int)(Right / u.X + 0.5f) * u.X; Top = (int)(Top / u.Y + 0.5f) * u.Y; Bottom = (int)(Bottom / u.Y + 0.5f) * u.Y; }
};