using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Queries;

public record ExportQuestionnaireResponsesQuery : IRequest<ExportFile>
{
	public required Guid QuestionnaireId { get; set; }
}

internal class ExportQuestionnaireResponsesQueryHandler : IRequestHandler<ExportQuestionnaireResponsesQuery, ExportFile>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _fileBuilder;

	public ExportQuestionnaireResponsesQueryHandler(IApplicationDbContext context, ICsvFileBuilder fileBuilder)
	{
		_context = context;
		_fileBuilder = fileBuilder;
	}

	public async Task<ExportFile> Handle(ExportQuestionnaireResponsesQuery request, CancellationToken cancellationToken)
	{
		var questionnaire = await _context.Questionnaires
			.Include(q => q.Questions)
			.FindOrNotFoundAsync(request.QuestionnaireId, cancellationToken);

		var questionnaireResponses = _context.QuestionnaireResponses
			.Include(qr => qr.Questionnaire.Questions)
			.Include(qr => qr.Participant)
			.Include(qr => qr.Responses).ThenInclude(qr => qr.Question)
			.Where(qr => qr.Questionnaire.Id.Equals(request.QuestionnaireId));

		List<string> headers = new()
		{
			nameof(Participant.FirstName),
			nameof(Participant.LastName),
			nameof(Participant.Height),
			nameof(Participant.Weight),
			nameof(Participant.Birth)
		};
		headers.AddRange(questionnaire.Questions.Select(question => question.Text));

		List<List<string>> rows = new();

		foreach (var questionnaireResponse in questionnaireResponses)
		{
			var row = new List<string>
			{
				questionnaireResponse.Participant.FirstName,
				questionnaireResponse.Participant.LastName,
				questionnaireResponse.Participant.Height.ToString() ?? "",
				questionnaireResponse.Participant.Weight.ToString() ?? "",
				questionnaireResponse.Participant.Birth.ToString() ?? "",
			};
			row.AddRange(questionnaire.Questions.Select(question => questionnaireResponse.Responses.FirstOrDefault(qr => qr.Question.Id == question.Id)?.Answer ?? ""));

			rows.Add(row);
		}

		return new ExportFile
		{
			Content = await _fileBuilder.BuildDynamicFile(headers, rows),
			FileName = $"{DateTime.Now.ToShortDateString()} {questionnaire.Title}.csv"
		};
	}
}
