namespace MazeCS.Collectables;

internal class Key : ICollectable
{
    public int Value => 0;
    public bool IsPersistent => true;

    private readonly Guid _id = Guid.NewGuid();
}

