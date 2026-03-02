var grid = new CellType[50, 20];

const int width = 50;
const int height = 20;

const int offsetY = 3;
const int offsetX = 0;

const int marginYMessage = 3;
const int messageHeight = 5;

const string sHeader = """
    ╔══════════════════════════════════════════════════╗
    ║          🏃 LABYRINTHE ASCII  C#  🏃             ║
    ╚══════════════════════════════════════════════════╝
    """;
const string sInstructions = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string sWin = """
    ╔════════════════════════════════╗
    ║   🎉  FÉLICITATIONS !  🎉      ║
    ║   Vous avez trouvé la sortie ! ║
    ╚════════════════════════════════╝
""";
const string sCanceled = "\n  Partie abandonnée. À bientôt !";
const string sPressKey = "  Appuyez sur une key pour quitter...";

const ConsoleColor SuccessColor     = ConsoleColor.Green;
const ConsoleColor DangerColor      = ConsoleColor.Red;
const ConsoleColor InfoColor        = ConsoleColor.Cyan;
const ConsoleColor InstructionColor = ConsoleColor.DarkCyan;
const ConsoleColor WallColor        = ConsoleColor.DarkGray;
const ConsoleColor CorridorColor    = ConsoleColor.DarkBlue;
const ConsoleColor PlayerColor      = ConsoleColor.Yellow;
const ConsoleColor ExitColor        = ConsoleColor.Green;

var playerX = 0;
var playerY = 0;
var mode = State.Playing;

GenerateMaze(grid, playerX, playerY);
DrawScreen();

while (mode == State.Playing)
{
    var key = Console.ReadKey(true).Key;

    var nx2 = playerX;
    var ny2 = playerY;

    switch (key)
    {
        case ConsoleKey.Z or ConsoleKey.UpArrow:    ny2--; break;
        case ConsoleKey.S or ConsoleKey.DownArrow:  ny2++; break;
        case ConsoleKey.Q or ConsoleKey.LeftArrow:  nx2--; break;
        case ConsoleKey.D or ConsoleKey.RightArrow: nx2++; break;
        case ConsoleKey.Escape: mode = State.Canceled; break;
    }
    if (nx2 >= 0 && nx2 < width && ny2 >= 0 && ny2 < height && grid[nx2, ny2] != CellType.Wall)
    {
        if (grid[nx2, ny2] == CellType.Exit) mode = State.Won;

        UpdateCell(playerX      , playerY      , CellType.Corridor);
        UpdateCell(playerX = nx2, playerY = ny2, CellType.Player  );
    }
}

DrawTextColorXY(0, offsetY + height + marginYMessage,
    mode == State.Won 
    ? (sWin, SuccessColor) 
    : (sCanceled, DangerColor)
);
DrawTextXY(0, offsetY + height + marginYMessage + messageHeight, sPressKey);
Console.CursorVisible = true;
Console.ReadKey(true);



void DrawTextXY(int x, int y, string text, ConsoleColor? color = null)
{
    Console.SetCursorPosition(x, y);
    if(color.HasValue)
    {
        Console.ForegroundColor = color.Value;
    }
    Console.Write(text);
    Console.ResetColor();
}

void DrawTextColorXY(int x, int y, (string text, ConsoleColor color) info) =>
    DrawTextXY(x, y, info.text, info.color);

void DrawCell(int cx, int cy) => DrawTextColorXY(
    offsetX + cx, 
    offsetY + cy,
    grid[cx, cy] switch
    {
        CellType.Wall   => ("█", WallColor),
        CellType.Player => ("@", PlayerColor),
        CellType.Exit   => ("★", ExitColor),
        _               => ("·", CorridorColor)
    });

void UpdateCell(int cx, int cy, CellType type)
{
    grid[cx, cy] = type;
    DrawCell(cx, cy);
}

void DrawScreen()
{
    Console.Clear();
    Console.CursorVisible = false;

    DrawTextXY(0, 0, sHeader, InfoColor);
    for (var y = 0; y < height; y++)
    {
        for (var x = 0; x < width; x++)
        {
            DrawCell(x, y);
        }
    }
    DrawTextXY(0, offsetY + height, sInstructions, InstructionColor);
}

void GenerateMaze(CellType[,] grid, int playerStartX, int playerStartY)
{
    const int cellW = width / 2;
    const int cellH = height / 2; 

    for (var y = 0; y < height; y++)
        for (var x = 0; x < width; x++)
            grid[x, y] = CellType.Wall;

    var stackX = new int[cellW * cellH];
    var stackY = new int[cellW * cellH];
    var stackTop = 0;

    var visited = new bool[cellW, cellH];

    int[] dx = [ 0, 1, 0, -1 ];
    int[] dy = [ -1, 0, 1, 0 ];

    Random rng = new Random();

    var startCX = 0;
    var startCY = 0;
    visited[startCX, startCY] = true;
    grid[startCX * 2, startCY * 2] = CellType.Corridor;

    stackX[stackTop] = startCX;
    stackY[stackTop] = startCY;
    stackTop++;

    while (stackTop > 0)
    {
        var cx = stackX[stackTop - 1];
        var cy = stackY[stackTop - 1];

        int[] order = { 0, 1, 2, 3 };
        rng.Shuffle(order);

        var found = false;
        foreach (var dir in order)
        {
            var nx = cx + dx[dir];
            var ny = cy + dy[dir];
            if (nx >= 0 && nx < cellW && ny >= 0 && ny < cellH && !visited[nx, ny])
            {
                grid[cx * 2 + dx[dir], cy * 2 + dy[dir]] = CellType.Corridor;
                grid[nx * 2, ny * 2] = CellType.Corridor;
                visited[nx, ny] = true;
                stackX[stackTop] = nx;
                stackY[stackTop] = ny;
                stackTop++;
                found = true;
                break;
            }
        }
        if (!found) stackTop--;
    }
    var outX = (cellW - 1) * 2;
    var outY = (cellH - 1) * 2;

    grid[playerStartX, playerStartY] = CellType.Player;
    grid[outX, outY] = CellType.Exit;
}

enum State
{
    Playing,
    Won,
    Canceled
}
enum CellType
{
    Corridor = 0,
    Wall = 1,
    Player = 2,
    Exit = 3
}