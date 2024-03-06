
using System.Collections.ObjectModel;

namespace CaptainCoder.Dungeoneering.Model;

public class WallMap
{
    public WallType this[Position position, Facing facing]
    {
        get => GetWall(position, facing);
        set => SetWall(position, facing, value);
    }
    private readonly Dictionary<TileEdge, WallType> _map = new();
    public IReadOnlyDictionary<TileEdge, WallType> Map => new ReadOnlyDictionary<TileEdge, WallType>(_map);
    public bool SetWall(Position position, Facing facing, WallType wall) => _map.TryAdd(new TileEdge(position, facing).Normalize(), wall);
    public bool RemoveWall(Position position, Facing facing) => _map.Remove(new TileEdge(position, facing).Normalize());
    public WallType GetWall(Position position, Facing facing) => _map[new TileEdge(position, facing).Normalize()];
    public bool TryGetWall(Position position, Facing facing, out WallType wall) => _map.TryGetValue(new TileEdge(position, facing).Normalize(), out wall);
    public int Count => _map.Count;
}
