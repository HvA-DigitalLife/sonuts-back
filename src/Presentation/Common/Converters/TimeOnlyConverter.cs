using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sonuts.Presentation.Common.Converters;

public class TimeOnlyConverter : JsonConverter<TimeOnly?>
{
	public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value is null ? null : TimeOnly.Parse(value);
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value?.ToShortTimeString());
}
