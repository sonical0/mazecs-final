using SylLab.MazeCS;

Vec2d Offset = new(0, 3);
Vec2d MazeSize = new(50, 20);

var InfoPos     = Offset    + new Vec2d(0, MazeSize.Y);
var WinEscPos   = InfoPos   + new Vec2d(0, 3);
var PressKeyPos = WinEscPos + new Vec2d(0, 5);

const int HeaderPaddingX = 10;
const int WinPaddingX = 2;

const string HeaderMsg = "🏃 LABYRINTHE ASCII  C#  🏃";
const string InfoMsg   = "  [Z/↑] Haut   [S/↓] Bas   [Q/←] Gauche   [D/→] Droite   [Échap] Quitter";
const string WinMsg1   = "🎉  FÉLICITATIONS !  🎉";
const string WinMsg2   = "Vous avez trouvé la sortie !";
const string EscMsg = "\n  Partie abandonnée. À bientôt !";
const string PressKeyMsg = "  Appuyez sur une key pour quitter...";

const ConsoleColor WinColor      = ConsoleColor.Green;
const ConsoleColor EscColor      = ConsoleColor.Red;
const ConsoleColor HeaderColor   = ConsoleColor.Cyan;
const ConsoleColor InfoColor     = ConsoleColor.DarkCyan;

var player = Vec2d.Origin;
var mode = State.Playing;
var kbd  = new KeyboardController();
var grid = new MazeGen(MazeSize, player).Generate();

using (var screen = new ConsoleScreen(Offset))
{
    screen.DrawFrame(Vec2d.Origin, HeaderPaddingX, HeaderColor, HeaderMsg);
    screen.DrawMaze (grid);
    screen.DrawTextXY(InfoPos, InfoMsg, InfoColor);

    while (mode == State.Playing)
    {
        var newPlayer = player;

        kbd.WaitKey();
        newPlayer += kbd.DirectionPressed;
        if (kbd.IsEscapePressed)
            mode = State.Canceled;
        if (newPlayer.IsIn(MazeSize) && grid[newPlayer.X, newPlayer.Y] != CellType.Wall)
        {
            if (grid[newPlayer.X, newPlayer.Y] == CellType.Exit) mode = State.Won;

            UpdateCell(screen, player, CellType.Corridor);
            UpdateCell(screen, player = newPlayer, CellType.Player);
        }
    }
    if(mode == State.Won)
        screen.DrawFrame(WinEscPos, WinPaddingX, WinColor, WinMsg1, WinMsg2);
    else
        screen.DrawTextXY(WinEscPos, EscMsg, EscColor);
    screen.DrawTextXY(PressKeyPos, PressKeyMsg);
}
kbd.WaitKey();

void UpdateCell(ConsoleScreen screen, Vec2d mazePos, CellType type) =>
    screen.DrawCell(mazePos, grid[mazePos.X, mazePos.Y] = type);
