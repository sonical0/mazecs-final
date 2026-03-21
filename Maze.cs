namespace SylLab.MazeCS;

public class Maze
{
    public Maze(IMazeGenerator gen)
    {
        _grid = gen.Generate();
        MazeSize = new(_grid.GetLength(0), _grid.GetLength(1));
    }
    public Vec2d StartPos => CellPositions.First(pos => this[pos].IsStartPos);
    public Vec2d MazeSize { get; init; }
    public Cell this[Vec2d pos] => _grid[pos.X, pos.Y];
    IEnumerable<Vec2d> CellPositions
    {
        get
        {
            for (var pos = Vec2d.Origin; pos.IsIn(MazeSize); pos = pos.NextLTR(MazeSize.X))
            {
                yield return pos;
            }
        }
    }

    public void RedrawCell(IGridDisplay gridDisp, Vec2d mazePos) => 
        gridDisp.DrawGridCell(mazePos, this[mazePos]);

    public void Draw(IGridDisplay gridDisp)
    {
        foreach (var pos in CellPositions)
            RedrawCell(gridDisp, pos);
    }

    private readonly Cell[,] _grid;
}

