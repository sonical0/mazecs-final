namespace MazeCS;

public abstract class Cell
{
    public const string EmptyRoomSymbol = " ";
    public const string RoomWithItemsSymbol = ".";
    public const string OpenedDoorSymbol = "_";
    public const string ClosedDoorSymbol = "/";
    public const string ExitSymbol = "★";
    public const string WallSymbol = "█";
    public abstract ConsoleColor Color { get; }
    public abstract string Content { get; }
    public virtual bool TryTraverse(ICollection<ICollectable> withItems) => false;
    public virtual bool IsStartPos => false;
    public virtual bool IsEndPos => false;
    public virtual IEnumerable<ICollectable> Collect(ref int score) => [];
}
