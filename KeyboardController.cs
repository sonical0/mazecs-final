namespace SylLab.MazeCS;

public class KeyboardController : IController
{
    ConsoleKey? _key = null;

    public void WaitKey() => _key = Console.ReadKey(true).Key;

    public Vec2d DirectionPressed =>
        _key switch
        {
            ConsoleKey.Q or ConsoleKey.LeftArrow  => Vec2d.West,
            ConsoleKey.D or ConsoleKey.RightArrow => Vec2d.East,
            ConsoleKey.Z or ConsoleKey.UpArrow    => Vec2d.North,
            ConsoleKey.S or ConsoleKey.DownArrow  => Vec2d.South,
            _ => new Vec2d(0, 0)
         };

    public bool IsCollectPressed => _key == ConsoleKey.Spacebar;
    public bool IsEscapePressed => _key == ConsoleKey.Escape;
}

