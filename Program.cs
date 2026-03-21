using SylLab.MazeCS;

Vec2d MazePos  = new(0, 3);
Vec2d MazeSize = new(50, 20);

var InfoPos     = MazePos   + new Vec2d(0, MazeSize.Y);
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

var kbd  = new KeyboardController();
var maze = new Maze(new MazeGen(MazeSize, StartPos: Vec2d.Origin));
var player = new Player(maze);

using (var screen = new ConsoleScreen(MazePos))
{
    screen.DrawFrame(Vec2d.Origin, HeaderPaddingX, HeaderColor, HeaderMsg);
    maze  .Draw(screen);
    player.Draw(screen);
    screen.DrawTextXY(InfoPos, InfoMsg, InfoColor);

    while (player.IsPlaying)
    {
        kbd.WaitKey();
        if(player.Move(kbd, out var prevPos))
        {
            maze.RedrawCell(screen, prevPos);
            player.Draw(screen);
        }
    }
    if(player.HasWon)
        screen.DrawFrame(WinEscPos, WinPaddingX, WinColor, WinMsg1, WinMsg2);
    else
        screen.DrawTextXY(WinEscPos, EscMsg, EscColor);
    screen.DrawTextXY(PressKeyPos, PressKeyMsg);
}
kbd.WaitKey();