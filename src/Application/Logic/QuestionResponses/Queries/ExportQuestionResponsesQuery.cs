using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;

namespace Sonuts.Application.Logic.QuestionResponses.Queries;

public record ExportQuestionResponsesQuery : IRequest<ExportFile>;

internal class ExportQuestionResponsesQueryHandler : IRequestHandler<ExportQuestionResponsesQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _csvFileBuilder;

	public ExportQuestionResponsesQueryHandler(IApplicationDbContext context, ICsvFileBuilder csvFileBuilder)
	{
		_context = context;
		_csvFileBuilder = csvFileBuilder;
	}

	public async Task<ExportFile> Handle(ExportQuestionResponsesQuery request, CancellationToken cancellationToken)
	{
		var questionResponses = await _context.QuestionResponses.ToArrayAsync(cancellationToken);

		return new ExportFile
		{
			Content = await _csvFileBuilder.BuildQuestionResponseFileAsync(questionResponses, cancellationToken),
			FileName = $"Executions-{DateTime.Now:yyyy-MM-dd}.csv"
		};
	}
}
