namespace SylLab.MazeCS;

public class MazeGen(Vec2d MazeSize, Vec2d Start) : IMazeGenerator
{    
    public CellType[,] Generate()
    {
        var grid = new CellType[MazeSize.X, MazeSize.Y];
        
        for (var pos = Vec2d.Origin; pos.IsIn(MazeSize); pos = pos.NextLTR(MazeSize.X))
        {
            SetTile(pos, CellType.Wall);
        }
        int[][] orders = [
            [ 0, 1, 2, 3 ], [ 0, 1, 3, 2 ], [ 0, 2, 1, 3 ], [ 0, 2, 3, 1 ], [ 0, 3, 1, 2 ], [ 0, 3, 2, 1 ],
            [ 1, 0, 2, 3 ], [ 1, 0, 3, 2 ], [ 1, 2, 0, 3 ], [ 1, 2, 3, 0 ], [ 1, 3, 0, 2 ], [ 1, 3, 2, 0 ],
            [ 2, 0, 1, 3 ], [ 2, 0, 3, 1 ], [ 2, 1, 0, 3 ], [ 2, 1, 3, 0 ], [ 2, 3, 0, 1 ], [ 2, 3, 1, 0 ],
            [ 3, 0, 1, 2 ], [ 3, 0, 2, 1 ], [ 3, 1, 0, 2 ], [ 3, 1, 2, 0 ], [ 3, 2, 0, 1 ], [ 3, 2, 1, 0 ]
        ];
        Vec2d[] dirs = [Vec2d.North * 2, Vec2d.East * 2, Vec2d.South * 2, Vec2d.West * 2];
        var rng = new Random();

        GenerateMazeRec(Start);

        SetTile(Start, CellType.Start);
        SetTile(

            (MazeSize + Vec2d.North + Vec2d.West).Even(),
            CellType.Exit
        );
        return grid;

        void SetTile(Vec2d pos, CellType type) => grid[pos.X, pos.Y] = type;

        void GenerateMazeRec(Vec2d pos)
        {
            SetTile(pos, CellType.Corridor);
            foreach (var index in rng.GetItems(orders, 1).First())
            {
                var next = pos + dirs[index];

                if (next.IsIn(MazeSize) && grid[next.X, next.Y] == CellType.Wall)
                {
                    SetTile((pos + next) / 2, CellType.Corridor);
                    GenerateMazeRec(next);
                }
            }
        }
    }

}
