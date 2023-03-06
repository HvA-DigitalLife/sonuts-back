using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Logic.Goals.Queries;

public record ExportGoalsQuery : IRequest<ExportFile>;

internal class ExportGoalsQueryHandler : IRequestHandler<ExportGoalsQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _csvFileBuilder;

	public ExportGoalsQueryHandler(IApplicationDbContext context, ICsvFileBuilder csvFileBuilder)
	{
		_context = context;
		_csvFileBuilder = csvFileBuilder;
	}

	public async Task<ExportFile> Handle(ExportGoalsQuery request, CancellationToken cancellationToken)
	{
		var goals = await _context.Goals
			.Include(g => g.CarePlan.Participant)
			.ToArrayAsync(cancellationToken);

		return new ExportFile
		{
			Content = await _csvFileBuilder.BuildGoalsFileAsync(goals, cancellationToken),
			FileName = $"Goals-{DateTime.Now:yyyy-MM-dd}.csv"
		};
	}
}
