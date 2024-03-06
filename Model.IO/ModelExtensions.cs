namespace CaptainCoder.Dungeoneering.Model.IO;

using Newtonsoft.Json;

public static class IOExtensions
{

    public static string ToJson(this WallMap wallMap)
    {
        (TileEdge, WallType)[] elems = wallMap.Map.Select(kvp => (kvp.Key, kvp.Value)).ToArray();
        return JsonConvert.SerializeObject(elems);
    }

    public static T LoadModel<T>(this string json)
    {
        if (typeof(T) == typeof(WallMap))
        {
            WallMap val = LoadWallMapFromJson(json);
            if (val is T tb) { return tb; }
        }
        throw new Exception($"Unable to load model of type {typeof(T)}.");
    }

    public static WallMap LoadWallMapFromJson(string json)
    {
        (TileEdge edge, WallType wall)[]? elems = JsonConvert.DeserializeObject<(TileEdge, WallType)[]>(json);
        if (elems is null) { throw new ArgumentException($"Invalid JSON format. Could not load WallMap from: \"{json}\""); }
        WallMap map = new();
        foreach (var el in elems)
        {
            map.SetWall(el.edge.Position, el.edge.Facing, el.wall);
        }
        return map;
    }

}
