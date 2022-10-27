using System.Globalization;
using CsvHelper.Configuration;
using Sonuts.Application.Logic.Executions.Models;

namespace Sonuts.Infrastructure.Files.Maps;

public sealed class ExecutionRecordMap : ClassMap<ExecutionRecord>
{
	public ExecutionRecordMap()
	{
		AutoMap(CultureInfo.InvariantCulture);

		Map(m => m.IsDone).ConvertUsing(c => c.IsDone ? "Yes" : "No");
	}
}
