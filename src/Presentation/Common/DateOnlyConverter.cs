using System.Text.Json;
using System.Text.Json.Serialization;

namespace Sonuts.Presentation.Common;

public class DateOnlyConverter : JsonConverter<DateOnly>
{
	public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
		DateOnly.Parse(reader.GetString()!);

	public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value.ToString("yyyy-MM-dd"));
}

public class NullableDateOnlyConverter : JsonConverter<DateOnly?>
{
	public override DateOnly? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		var value = reader.GetString();
		return value is null ? null : DateOnly.Parse(value);
	}

	public override void Write(Utf8JsonWriter writer, DateOnly? value, JsonSerializerOptions options)
		=> writer.WriteStringValue(value?.ToString("yyyy-MM-dd"));

}
