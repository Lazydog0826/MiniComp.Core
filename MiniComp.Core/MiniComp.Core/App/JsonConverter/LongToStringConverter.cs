using Newtonsoft.Json;

namespace MiniComp.Core.App.JsonConverter;

public class LongToStringConverter : JsonConverter<long?>
{
    public override long? ReadJson(
        JsonReader reader,
        Type objectType,
        long? existingValue,
        bool hasExistingValue,
        JsonSerializer serializer
    )
    {
        try
        {
            if (reader.Value == null || string.IsNullOrEmpty(reader.Value.ToString()))
                return null;
            return Convert.ToInt64(reader.Value);
        }
        catch
        {
            throw new JsonException();
        }
    }

    public override void WriteJson(JsonWriter writer, long? value, JsonSerializer serializer)
    {
        if (value.HasValue)
        {
            writer.WriteValue(value.ToString());
        }
        else
        {
            writer.WriteNull();
        }
    }
}
