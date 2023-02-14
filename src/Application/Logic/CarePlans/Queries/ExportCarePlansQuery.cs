using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;
using Sonuts.Application.Common.Security;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.CarePlans.Queries;

[Authorize(Roles = "Admin")]
public record ExportCarePlansQuery : IRequest<ExportFile>;

internal class ExportCarePlansQueryHandler : IRequestHandler<ExportCarePlansQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _fileBuilder;

	public ExportCarePlansQueryHandler(IApplicationDbContext context, ICsvFileBuilder fileBuilder)
	{
		_context = context;
		_fileBuilder = fileBuilder;
	}

	public async Task<ExportFile> Handle(ExportCarePlansQuery request, CancellationToken cancellationToken)
	{
		var carePlans = await _context.CarePlans
			.Include(cp => cp.Participant)
			.Include(cp => cp.Goals).ThenInclude(g => g.Activity)
			.Include(cp => cp.Goals).ThenInclude(g => g.Executions)
			.ToListAsync(cancellationToken);

		if (!carePlans.Any())
			return new ExportFile
			{
				Content = Array.Empty<byte>(),
				FileName = $"{DateTime.Now.ToShortDateString()} CarePlans.csv"
			};

		List<string> headers = new()
		{
			nameof(Participant.FirstName),
			nameof(Participant.LastName)
		};

		var goalHeaders = Enumerable.Range(0, carePlans.Select(cp => cp.Goals.Count).Max()).Select(i => i.ToString())
			.ToArray();

		headers.AddRange(goalHeaders);

		List<List<string?>> rows = new();
		
		foreach (var carePlan in carePlans)
		{
			var row = new List<string?>
			{
				carePlan.Participant.FirstName,
				carePlan.Participant.LastName
			};

			var goals = carePlan.Goals
				.Select(g => g.Activity.Name)
				.ToArray();

			row.AddRange(goals.Concat(goalHeaders[goals.Length..]));

			rows.Add(row);
		}

		return new ExportFile
		{
			Content = await _fileBuilder.BuildDynamicFile(headers, rows),
			FileName = $"{DateTime.Now.ToShortDateString()} CarePlans.csv"
		};
	}
}
