namespace MazeCS.Cells;
internal class Exit : Room
{
    public static readonly Exit Instance = new Exit();
    private Exit() { }
    public override string Content => ExitSymbol;
    public override ConsoleColor Color => ConsoleColor.Green;
    public override bool IsEndPos => true;
}
