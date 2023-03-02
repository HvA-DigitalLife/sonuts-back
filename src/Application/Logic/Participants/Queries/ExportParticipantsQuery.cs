using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Logic.Participants.Queries;

public record ExportParticipantsQuery : IRequest<ExportFile>;

internal class ExportParticipantsQueryHandler : IRequestHandler<ExportParticipantsQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _csvFileBuilder;

	public ExportParticipantsQueryHandler(IApplicationDbContext context, ICsvFileBuilder csvFileBuilder)
	{
		_context = context;
		_csvFileBuilder = csvFileBuilder;
	}

	public async Task<ExportFile> Handle(ExportParticipantsQuery request, CancellationToken cancellationToken)
	{
		var participants = await _context.Participants.ToArrayAsync(cancellationToken);

		return new ExportFile
		{
			Content = await _csvFileBuilder.BuildParticipantsFileAsync(participants, cancellationToken),
			ContentType = "text/csv",
			FileName = $"Participants-{DateTime.Now:yyyy-MM-dd}.csv"
		};
	}
}
