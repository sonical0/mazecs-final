namespace SylLab.MazeCS;

public class Player(Maze maze)
{
    public bool Move(KeyboardController kbd, out Vec2d prevPos)
    {
        prevPos = _pos;
        _pos = NextWalkablePos(_pos + kbd.DirectionPressed);        
        
        HasWon    = maze[_pos] == CellType.Exit;
        IsPlaying = !HasWon && !kbd.IsEscapePressed;
        return prevPos != _pos;
    }

    private Vec2d NextWalkablePos(Vec2d nextPos) =>
        nextPos.IsIn(maze.MazeSize) && maze[nextPos] != CellType.Wall ? nextPos : _pos;
    public void Draw(ConsoleScreen screen) => 
        screen.DrawGridCell(_pos, ("@", ConsoleColor.Yellow));
    public Maze Maze => maze;
    public bool IsPlaying { get; private set; } = true;
    public bool HasWon    { get; private set; } = false;

    public Vec2d _pos = maze.StartPos;
}
