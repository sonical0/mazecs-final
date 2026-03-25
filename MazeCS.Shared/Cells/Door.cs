namespace MazeCS.Cells;

using MazeCS.Collectables;

internal class Door : Cell
{
    public override ConsoleColor Color => ConsoleColor.Magenta;
    public override string Content => _opened ? OpenedDoorSymbol : ClosedDoorSymbol;
    public override bool TryTraverse(ICollection<ICollectable> withItems) =>
        _opened = _opened || withItems.Remove(_key);
    public Key CloseAndTakeKey()
    {
        _opened = false;
        return _key;
    }
    private bool _opened = true;
    private readonly Key _key = new Key();
}
