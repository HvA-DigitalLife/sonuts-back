using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using Sonuts.Infrastructure.Files.Maps;

namespace Sonuts.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
	public async Task<byte[]> BuildDynamicFileAsync(List<string> headers, List<List<string?>> rows)
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
					csvWriter.WriteField(column ?? string.Empty);
				}
				await csvWriter.NextRecordAsync();
			}
		}

		return memoryStream.ToArray();
	}

	public async Task<byte[]> BuildParticipantsFileAsync(IEnumerable<Participant> participants, CancellationToken cancellationToken = default) =>
		await BuildCsvFile<Participant, ParticipantMap>(participants, cancellationToken);

	public async Task<byte[]> BuildExecutionsFileAsync(IEnumerable<Execution> executions, CancellationToken cancellationToken = default) =>
		await BuildCsvFile<Execution, ExecutionMap>(executions, cancellationToken);

	private static async Task<byte[]> BuildCsvFile<TInput, TClassMap>(IEnumerable<TInput> records, CancellationToken cancellationToken = default) where TClassMap : ClassMap<TInput>
	{
		using var memoryStream = new MemoryStream();
		await using (var streamWriter = new StreamWriter(memoryStream))
		{
			await using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			csvWriter.Context.RegisterClassMap<TClassMap>();
			await csvWriter.WriteRecordsAsync(records, cancellationToken);
		}

		return memoryStream.ToArray();
	}
}
