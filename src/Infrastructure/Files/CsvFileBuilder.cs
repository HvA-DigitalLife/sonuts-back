using System.Globalization;
using CsvHelper;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Logic.Executions.Models;
using Sonuts.Infrastructure.Files.Maps;

namespace Sonuts.Infrastructure.Files;

public class CsvFileBuilder : ICsvFileBuilder
{
	public byte[] BuildExecutionsFile(IEnumerable<ExecutionRecord> records)
	{
		using var memoryStream = new MemoryStream();
		using (var streamWriter = new StreamWriter(memoryStream))
		{
			using var csvWriter = new CsvWriter(streamWriter, CultureInfo.InvariantCulture);

			csvWriter.Configuration.RegisterClassMap<ExecutionRecordMap>();
			csvWriter.WriteRecords(records);
		}

		return memoryStream.ToArray();
	}
}
