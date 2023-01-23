using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Logic.Participants.Queries;
using Sonuts.Infrastructure.Files.Maps;

namespace Sonuts.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
	public async Task<byte[]> BuildDynamicFile(List<string> headers, List<List<string>> rows)
	{
		using var memoryStream = new MemoryStream();
		await using (var streamWriter = new StreamWriter(memoryStream))
		{
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			headers.ForEach(csvWriter.WriteField);
			await csvWriter.NextRecordAsync();

			foreach (var row in rows)
			{
				foreach (var column in row)
				{
					csvWriter.WriteField(column);
				}
				await csvWriter.NextRecordAsync();
			}
		}

		return memoryStream.ToArray();
	}

	public async Task<byte[]> BuildParticipantsFile(IEnumerable<OverviewParticipantDto> participants) =>
		await BuildCsvFile<OverviewParticipantDto, ParticipantMap>(participants);

	private static async Task<byte[]> BuildCsvFile<TInput, TClassMap>(IEnumerable<TInput> records) where TClassMap : ClassMap<TInput>
	{
		using var memoryStream = new MemoryStream();
		await using (var streamWriter = new StreamWriter(memoryStream))
		{
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			csvWriter.Context.RegisterClassMap<TClassMap>();
			await csvWriter.WriteRecordsAsync(records);
		}

		return memoryStream.ToArray();
	}
}
