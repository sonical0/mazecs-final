namespace MazeCS.Cells;

internal class Room(IEnumerable<ICollectable>? items = null) : Cell
{
    private readonly List<ICollectable> _items = items?.ToList() ?? [];
    public override ConsoleColor Color => ConsoleColor.DarkBlue;
    public override string Content => _items.Count>0 ? RoomWithItemsSymbol : EmptyRoomSymbol;
    public override bool TryTraverse(ICollection<ICollectable> _) => true;
    public override IEnumerable<ICollectable> Collect(ref int score)
    {
        lock(_items)
        {
            var collected = _items.Where(item => item.IsPersistent).ToArray();

            score += _items.Sum(item => item.Value);
            _items.Clear();
            return collected;
        }
    }
}
