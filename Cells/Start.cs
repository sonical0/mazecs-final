namespace SylLab.MazeCS.Cells;

internal class Start : Room
{
    public static new readonly Start Instance = new Start();
    private Start() { }
    public override bool IsStartPos => true;
}
