namespace SylLab.MazeCS;

public class ConsoleScreen : IDisposable
{
    public ConsoleScreen(Vec2d gridPos)
    {
        _gridPos = gridPos;
        Console.Clear();
        Console.CursorVisible = false;
    }
    public void Dispose() => 
        Console.CursorVisible = true;

    public void DrawTextXY(Vec2d pos, string text, ConsoleColor? color = null)
    {
        Console.SetCursorPosition(pos.X, pos.Y);
        if (color.HasValue)
        {
            Console.ForegroundColor = color.Value;
        }
        Console.Write(text);
        Console.ResetColor();
    }

    public void DrawFrame(Vec2d pos, int paddingX, ConsoleColor color, params string [] lines)
    {
        Vec2d IncPos() => pos = pos with { Y = pos.Y + 1 };
        
        var width = lines.Max(s => s.Length + 2*paddingX);
        var horizontal = new string('═', width);
        
        DrawTextXY(pos, $"╔{ horizontal}╗", color);
        foreach (var line in lines)
        {
            var dif = width - line.Length;
            var left = new string(' ', dif / 2);
            var right = dif%2==0 ? left : left+' ';

            DrawTextXY(IncPos(), $"║{left}{line}{right}║", color);
        }
        DrawTextXY(IncPos(), $"╚{horizontal}╝", color);
    }

    public void DrawGridCell(Vec2d pos, (string Content, ConsoleColor Color) info) =>
        DrawTextXY(_gridPos + pos, info.Content, info.Color);
    
    private readonly Vec2d _gridPos;
}
