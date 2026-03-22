namespace MazeCS.Collectables;

internal class Coin : ICollectable
{
    private Coin() { }
    public static Coin Instance { get; } = new Coin();

    public int Value => 1;
    public bool IsPersistent => false;
}
