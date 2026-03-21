using SylLab.MazeCS;

Vec2d Origin = new(0, 0);
Vec2d Offset = new(0, 3);
Vec2d MazeSize = new(50, 20);

var InfoPos     = Offset    + new Vec2d(0, MazeSize.Y);
var WinEscPos   = InfoPos   + new Vec2d(0, 3);
var PressKeyPos = WinEscPos + new Vec2d(0, 5);

var grid = new CellType[MazeSize.X, MazeSize.Y];

const string HeaderMsg = """
    ╔══════════════════════════════════════════════════╗
    ║          🏃 LABYRINTHE ASCII  C#  🏃             ║
    ╚══════════════════════════════════════════════════╝
    """;
const string InfoMsg = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string WinMsg = """
    ╔════════════════════════════════╗
    ║   🎉  FÉLICITATIONS !  🎉      ║
    ║   Vous avez trouvé la sortie ! ║
    ╚════════════════════════════════╝
""";
const string EscMsg = "\n  Partie abandonnée. À bientôt !";
const string PressKeyMsg = "  Appuyez sur une key pour quitter...";

const ConsoleColor WinColor      = ConsoleColor.Green;
const ConsoleColor EscColor      = ConsoleColor.Red;
const ConsoleColor HeaderColor   = ConsoleColor.Cyan;
const ConsoleColor InfoColor     = ConsoleColor.DarkCyan;
const ConsoleColor WallColor     = ConsoleColor.DarkGray;
const ConsoleColor CorridorColor = ConsoleColor.DarkBlue;
const ConsoleColor PlayerColor   = ConsoleColor.Yellow;
const ConsoleColor ExitColor     = ConsoleColor.Green;

var player = Origin;
var mode = State.Playing;

GenerateMaze(grid, player);
DrawScreen();

while (mode == State.Playing)
{
    var key = Console.ReadKey(true).Key;
    var newPlayer = player;

    switch (key)
    {
        case ConsoleKey.Z or ConsoleKey.UpArrow   : newPlayer += Vec2d.North; break;
        case ConsoleKey.S or ConsoleKey.DownArrow : newPlayer += Vec2d.South; break;
        case ConsoleKey.Q or ConsoleKey.LeftArrow : newPlayer += Vec2d.West ; break;
        case ConsoleKey.D or ConsoleKey.RightArrow: newPlayer += Vec2d.East ; break;
        case ConsoleKey.Escape: mode = State.Canceled; break;
    }
    if (newPlayer.IsIn(MazeSize) && grid[newPlayer.X, newPlayer.Y] != CellType.Wall)
    {
        if (grid[newPlayer.X, newPlayer.Y] == CellType.Exit) mode = State.Won;

        UpdateCell(player, CellType.Corridor);
        UpdateCell(player = newPlayer, CellType.Player);
    }
}
DrawTextColorXY(WinEscPos, mode == State.Won ? (WinMsg, WinColor) : (EscMsg, EscColor));
DrawTextXY(PressKeyPos, PressKeyMsg);
Console.CursorVisible = true;
Console.ReadKey(true);

#region Functions

void DrawTextXY(Vec2d pos, string text, ConsoleColor? color = null)
{
    Console.SetCursorPosition(pos.X, pos.Y);
    if (color.HasValue)
    {
        Console.ForegroundColor = color.Value;
    }
    Console.Write(text);
    Console.ResetColor();
}

void DrawTextColorXY(Vec2d pos, (string text, ConsoleColor color) info) =>
    DrawTextXY(pos, info.text, info.color);

void DrawCell(Vec2d mazePos) => DrawTextColorXY(
    Offset + mazePos,
    grid[mazePos.X, mazePos.Y] switch
    {
        CellType.Wall   => ("█", WallColor),
        CellType.Player => ("@", PlayerColor),
        CellType.Exit   => ("★", ExitColor),
        _               => ("·", CorridorColor)
    });

void UpdateCell(Vec2d mazePos, CellType type)
{
    SetTile(mazePos, type);
    DrawCell(mazePos);
}

void DrawScreen()
{
    Console.Clear();
    Console.CursorVisible = false;

    DrawTextXY(Origin, HeaderMsg, HeaderColor);
    for (var pos = Origin; pos.IsIn(MazeSize); pos = pos.NextLTR(MazeSize.X))
    {
        DrawCell(pos);
    }
    DrawTextXY(InfoPos, InfoMsg, InfoColor);
}

void SetTile(Vec2d pos, CellType type) =>
    grid[pos.X, pos.Y] = type;


void GenerateMaze(CellType[,] grid, Vec2d start)
{
    for (var pos = Origin; pos.IsIn(MazeSize); pos = pos.NextLTR(MazeSize.X))
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

    GenerateMazeRec(start);

    SetTile(start, CellType.Player);
    SetTile(

        (MazeSize + Vec2d.North + Vec2d.West).Even(),
        CellType.Exit
    );
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

#endregion
