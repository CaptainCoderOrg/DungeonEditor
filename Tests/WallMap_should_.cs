namespace Tests;

using CaptainCoder.Dungeoneering.Model;

using Shouldly;

public class WallMap_should_
{

    [Fact]
    public void not_update_count_when_setting_matching_shared_wall()
    {
        WallMap underTest = new();
        underTest.SetWall(new Position(5, 5), Facing.North, WallType.Solid);
        underTest.SetWall(new Position(5, 4), Facing.South, WallType.Solid);

        underTest.Count.ShouldBe(1);
        underTest[new Position(5, 5), Facing.North].ShouldBe(WallType.Solid);
        underTest[new Position(5, 4), Facing.South].ShouldBe(WallType.Solid);
    }
}