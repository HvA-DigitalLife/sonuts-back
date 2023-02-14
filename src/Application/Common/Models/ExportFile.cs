namespace Sonuts.Application.Common.Models;

public record ExportFile
{
	public required byte[] Content { get; init; }

	public string ContentType { get; init; } = "text/csv";

	public required string FileName { get; init; }
}
