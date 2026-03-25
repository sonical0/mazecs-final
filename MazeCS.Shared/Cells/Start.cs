namespace MazeCS.Cells;

internal class Start : Room
{
    public Start(IEnumerable<ICollectable> items):base(items) { }
    public override bool IsStartPos => true;
}
