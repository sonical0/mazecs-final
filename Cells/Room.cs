namespace SylLab.MazeCS.Cells;

internal class Room : Cell
{
    public static readonly Room Instance = new Room();
    protected Room() { }

    public override ConsoleColor Color => ConsoleColor.DarkBlue;
    public override string Content => ".";
    public override bool IsTraversable => true;
}
