namespace CaptainCoder.Dungeoneering.Model;

public record class TileEdge(Position Position, Facing Facing)
{
}

public static class TileEdgeExtensions
{
    public static TileEdge Normalize(this TileEdge edge)
    {
        return edge.Facing switch
        {
            Facing.North or Facing.East => edge,
            Facing.South => edge with { Facing = Facing.North, Position = edge.Position.Step(Facing.South) },
            Facing.West => edge with { Facing = Facing.East, Position = edge.Position.Step(Facing.West) },
            _ => throw new NotImplementedException($"Unknown facing: {edge.Facing}"),
        };
    }
}