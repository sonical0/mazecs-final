namespace MazeCS;

using MazeCS.Cells;
using MazeCS.Collectables;

public class MazeGen(Vec2d MazeSize, Vec2d StartPos, double coinRate, double doorRate) : IMazeGenerator
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
        var keys = new List<ICollectable>();

        GenerateMazeRec(StartPos, keys);

        SetTile(StartPos, new Start(keys));
        SetTile(
            (MazeSize + Vec2d.North + Vec2d.West).Even(),
            Exit.Instance
        );
        return grid;

        void SetTile(Vec2d pos, Cell type) => grid[pos.X, pos.Y] = type;
        void GenerateMazeRec(Vec2d pos, IList<ICollectable> keys)
        {
            SetTile(pos, NewRoom(rng, keys));
            foreach (var index in rng.GetItems(orders, 1).First())
            {
                var next = pos + dirs[index];

                if (next.IsIn(MazeSize) && grid[next.X, next.Y] is Wall)
                {
                    GenerateMazeRec(next, keys);
                    SetTile((pos + next) / 2, NewRoomOrDoor(rng, keys));
                }
            }
        }
    }
    private static IEnumerable<ICollectable> EnumItems(params ICollectable?[] items) =>
        items.Where(item => item is not null).Select(item => item!);
    private static ICollectable? RemoveAt(IList<ICollectable> items, int index)
    {
        if (index >= items.Count)
            return null;
        var item = items[index];
        items.RemoveAt(index);
        return item;
    }
    private Cell NewRoom(Random rng, IList<ICollectable> keys) => new Room(EnumItems(
        rng.NextDouble() < coinRate ? Coin.Instance : null,
        keys.Count > 0 ? RemoveAt(keys, rng.Next((int)(1.0 / doorRate))) : null
    ));
    private Cell NewRoomOrDoor(Random rng, IList<ICollectable> keys)
    {
        if(rng.NextDouble() >= doorRate)
            return NewRoom(rng, keys);
        var door = new Door();

        keys.Add(door.CloseAndTakeKey());
        return door;
    }
}
