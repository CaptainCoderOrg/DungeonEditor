namespace CaptainCoder.Dungeoneering.Model;

public readonly record struct Position(int X, int Y);

public static class PositionExtensions
{
    public static Position Step(this Position position, Facing direction) => (direction, position) switch
    {
        (Facing.North, Position(int _, int y)) => position with { Y = y - 1 },
        (Facing.South, Position(int _, int y)) => position with { Y = y + 1 },
        (Facing.East, Position(int x, int _)) => position with { X = x + 1 },
        (Facing.West, Position(int x, int _)) => position with { X = x - 1 },
        _ => throw new NotImplementedException($"Unknown Facing: {direction}"),
    };
}