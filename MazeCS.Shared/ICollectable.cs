namespace MazeCS;

public interface ICollectable
{
    int Value { get; }
    bool IsPersistent { get; }
}
