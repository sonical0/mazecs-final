namespace SylLab.MazeCS;

public record Vec2d(int X, int Y)
{
    public static readonly Vec2d North = new( 0, -1);
    public static readonly Vec2d East  = new( 1,  0);
    public static readonly Vec2d South = new( 0,  1);
    public static readonly Vec2d West  = new(-1,  0);

    public static Vec2d operator +(Vec2d a, Vec2d b) =>
        new(a.X + b.X, a.Y + b.Y);

    public static Vec2d operator /(Vec2d a, int n) =>
        new(a.X / n, a.Y / n);

    public static Vec2d operator *(Vec2d a, int n) =>
        new(a.X * n, a.Y * n);

    public bool IsIn(Vec2d size) =>
        0 <= X && X < size.X &&
        0 <= Y && Y < size.Y;

    public Vec2d NextLTR(int maxWidth) =>
        X + 1 < maxWidth
        ? new Vec2d(X + 1, Y)
        : new Vec2d(0, Y + 1);

    public Vec2d Even() =>
        new Vec2d(X & ~1, Y & ~1);
}
