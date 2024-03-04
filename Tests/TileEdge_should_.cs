namespace Tests;

using CaptainCoder.Dungeoneering.Model;

using Shouldly;

public class TileEdge_should_
{

    public static IEnumerable<object[]> NormalizeTestData => [
        // Start, Move Direction, Expected state
        [new TileEdge(new Position(0, 0), Facing.North), new TileEdge(new Position(0, 0), Facing.North)],
        [new TileEdge(new Position(0, 0), Facing.South), new TileEdge(new Position(0, 1), Facing.North)],
        [new TileEdge(new Position(0, 0), Facing.East), new TileEdge(new Position(0, 0), Facing.East)],
        [new TileEdge(new Position(0, 0), Facing.West), new TileEdge(new Position(-1, 0), Facing.East)],
    ];

    [Theory]
    [MemberData(nameof(NormalizeTestData))]
    public void normalize(TileEdge underTest, TileEdge expected)
    {
        underTest.Normalize().ShouldBe(expected);
    }
}