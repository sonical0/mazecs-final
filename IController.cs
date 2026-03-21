namespace SylLab.MazeCS;

public interface IController
{
    Vec2d DirectionPressed {  get; }

    bool IsCollectPressed { get; }
    bool IsEscapePressed { get; }
}

