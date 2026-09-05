using System.Text.Json;
using System.Text.Json.Serialization;
using CinemaCatalog.Domain.ValueObjects;

namespace CinemaCatalog.Domain.ModelBinding;

public class YearJsonConverter : JsonConverter<Year>
{
    public override Year? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        var value = reader.GetString();
        return string.IsNullOrEmpty(value) ? null : Year.Parse(value);
    }

    public override void Write(Utf8JsonWriter writer, Year value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value.ToString());
    }
}