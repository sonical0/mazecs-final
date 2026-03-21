namespace SylLab.MazeCS.Cells;

internal class Wall : Cell
{
    public static readonly Wall Instance = new Wall();
    private Wall() { }

    public override ConsoleColor Color => ConsoleColor.DarkGray;
    public override string Content => "█";
}
