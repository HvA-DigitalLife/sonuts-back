using Sonuts.Application.Logic.Executions.Models;

namespace Sonuts.Application.Common.Interfaces;

public interface ICsvFileBuilder
{
	byte[] BuildExecutionsFile(IEnumerable<ExecutionRecord> records);
}
