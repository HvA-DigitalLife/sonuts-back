using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Logic.Executions.Queries;

public record ExportExecutionsQuery : IRequest<ExportFile>;

internal class ExportExecutionsQueryHandler : IRequestHandler<ExportExecutionsQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _csvFileBuilder;

	public ExportExecutionsQueryHandler(IApplicationDbContext context, ICsvFileBuilder csvFileBuilder)
	{
		_context = context;
		_csvFileBuilder = csvFileBuilder;
	}

	public async Task<ExportFile> Handle(ExportExecutionsQuery request, CancellationToken cancellationToken)
	{
		var executions = await _context.Executions
			.Include(e => e.Goal.Activity.Theme)
			.Include(e => e.Goal.CarePlan.Participant)
			.ToArrayAsync(cancellationToken);

		return new ExportFile
		{
			Content = await _csvFileBuilder.BuildExecutionsFileAsync(executions, cancellationToken),
			FileName = $"Executions-{DateTime.Now:yyyy-MM-dd}.csv"
		};
	}
}
