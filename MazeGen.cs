namespace SylLab.MazeCS;

using SylLab.MazeCS.Cells;

public class MazeGen(Vec2d MazeSize, Vec2d StartPos) : IMazeGenerator
{    
    public Cell[,] Generate()
    {
        var grid = new Cell[MazeSize.X, MazeSize.Y];
        
        for (var pos = Vec2d.Origin; pos.IsIn(MazeSize); pos = pos.NextLTR(MazeSize.X))
        {
            SetTile(pos, Wall.Instance);
        }
        int[][] orders = [
            [ 0, 1, 2, 3 ], [ 0, 1, 3, 2 ], [ 0, 2, 1, 3 ], [ 0, 2, 3, 1 ], [ 0, 3, 1, 2 ], [ 0, 3, 2, 1 ],
            [ 1, 0, 2, 3 ], [ 1, 0, 3, 2 ], [ 1, 2, 0, 3 ], [ 1, 2, 3, 0 ], [ 1, 3, 0, 2 ], [ 1, 3, 2, 0 ],
            [ 2, 0, 1, 3 ], [ 2, 0, 3, 1 ], [ 2, 1, 0, 3 ], [ 2, 1, 3, 0 ], [ 2, 3, 0, 1 ], [ 2, 3, 1, 0 ],
            [ 3, 0, 1, 2 ], [ 3, 0, 2, 1 ], [ 3, 1, 0, 2 ], [ 3, 1, 2, 0 ], [ 3, 2, 0, 1 ], [ 3, 2, 1, 0 ]
        ];
        Vec2d[] dirs = [Vec2d.North * 2, Vec2d.East * 2, Vec2d.South * 2, Vec2d.West * 2];
        var rng = new Random();

        GenerateMazeRec(StartPos);

        SetTile(StartPos, Start.Instance);
        SetTile(

            (MazeSize + Vec2d.North + Vec2d.West).Even(),
            Exit.Instance
        );
        return grid;

        void SetTile(Vec2d pos, Cell type) => grid[pos.X, pos.Y] = type;

        void GenerateMazeRec(Vec2d pos)
        {
            SetTile(pos, Room.Instance);
            foreach (var index in rng.GetItems(orders, 1).First())
            {
                var next = pos + dirs[index];

                if (next.IsIn(MazeSize) && !grid[next.X, next.Y].IsTraversable)
                {
                    SetTile((pos + next) / 2, Room.Instance);
                    GenerateMazeRec(next);
                }
            }
        }
    }

}
