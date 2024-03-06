namespace CaptainCoder.Dungeoneering.Model.IO;

using Newtonsoft.Json;

public static class IOExtensions
{

    public static string ToJson(this WallMap wallMap)
    {
        return JsonConvert.SerializeObject(wallMap, WallMapJsonConverter.Shared);
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
        return JsonConvert.DeserializeObject<WallMap>(json, WallMapJsonConverter.Shared);
    }

}

public class WallMapJsonConverter : JsonConverter<WallMap>
{
    public static WallMapJsonConverter Shared { get; } = new();
    public override WallMap? ReadJson(JsonReader reader, Type objectType, WallMap? existingValue, bool hasExistingValue, JsonSerializer serializer)
    {
        (TileEdge edge, WallType wall)[]? elems = JsonConvert.DeserializeObject<(TileEdge, WallType)[]>((string)reader.Value);
        return new WallMap(elems);
    }

    public override void WriteJson(JsonWriter writer, WallMap? value, JsonSerializer serializer)
    {
        (TileEdge, WallType)[] elems = value.Map.Select(kvp => (kvp.Key, kvp.Value)).ToArray();
        string json = JsonConvert.SerializeObject(elems);
        writer.WriteValue(json);
    }
}