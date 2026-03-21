namespace SylLab.MazeCS;

public class Maze
{
    public Maze(IMazeGenerator gen)
    {
        _grid = gen.Generate();
        MazeSize = new(_grid.GetLength(0), _grid.GetLength(1));
    }
    public Vec2d StartPos => CellPositions.First(pos => this[pos] == CellType.Start);
    public Vec2d MazeSize { get; init; }
    public CellType this[Vec2d pos] => _grid[pos.X, pos.Y];
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

    public void RedrawCell(ConsoleScreen screen, Vec2d mazePos) => screen.DrawGridCell(
        mazePos,
        this[mazePos] switch {
            CellType.Wall     => ("█", ConsoleColor.DarkGray),
            CellType.Corridor or
            CellType.Start    => (".", ConsoleColor.DarkBlue),
            CellType.Exit     => ("★", ConsoleColor.Green),
            _ => throw new InvalidOperationException($"Invalid cell type: {this[mazePos]}")
        }
    );

    public void Draw(ConsoleScreen screen)
    {
        foreach (var pos in CellPositions)
            RedrawCell(screen, pos);
    }

    private readonly CellType[,] _grid;
}

