namespace CaptainCoder.Dungeoneering.Model.IO;

using Newtonsoft.Json;

public static class IOExtensions
{

    public static string ToJson(this WallMap wallMap)
    {
        (Position, Facing, WallType)[] elems = wallMap.Map.Select(kvp => (kvp.Key.Position, kvp.Key.Facing, kvp.Value)).ToArray();
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
        (Position pos, Facing facing, WallType wall)[]? elems = JsonConvert.DeserializeObject<(Position, Facing, WallType)[]>(json);
        if (elems is null) { throw new ArgumentException($"Invalid JSON format. Could not load WallMap from: \"{json}\""); }
        WallMap map = new();
        foreach (var el in elems)
        {
            map.SetWall(el.pos, el.facing, el.wall);
        }
        return map;
    }

}
