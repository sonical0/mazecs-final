namespace MazeCS;

public interface IGridDisplay
{
    void DrawGridCell(Vec2d pos, Cell cell) => DrawGridCell(pos, cell.Content, cell.Color);
    void DrawGridCell(Vec2d pos, string content, ConsoleColor color);
}
