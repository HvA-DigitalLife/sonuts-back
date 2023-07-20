using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sonuts.Presentation.Common;

public class TimeOnlyConverter : JsonConverter<TimeOnly>
{
	public override TimeOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		TimeOnly.Parse(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, TimeOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToShortTimeString());
}

public class NullableTimeOnlyConverter : JsonConverter<TimeOnly?>
{
	public override TimeOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();

		if (TimeOnly.TryParse(value, out var timeOnly))
			return timeOnly;

		if (DateTime.TryParse(value, out var dateTime))
			return TimeOnly.FromDateTime(dateTime);

		return null;
	}

	public override void Write(Utf8JsonWriter writer, TimeOnly? value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value?.ToShortTimeString());
}
