namespace SylLab.MazeCS;

public abstract class Cell
{
    public abstract ConsoleColor Color { get; }
    public abstract string Content { get; }

    public virtual bool IsTraversable => false;
    public virtual bool IsStartPos => false;
    public virtual bool IsEndPos => false;
}
