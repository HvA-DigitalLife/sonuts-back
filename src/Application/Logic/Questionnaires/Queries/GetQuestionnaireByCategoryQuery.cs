using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.Questionnaires.Queries;

public record GetQuestionnaireByCategoryQuery : IRequest<QuestionnaireDto>
{
	public Guid CategoryId { get; set; }
}

public class GetQuestionnaireByTypeQueryValidator : AbstractValidator<GetQuestionnaireByCategoryQuery>
{
	public GetQuestionnaireByTypeQueryValidator()
	{
		RuleFor(query => query.CategoryId)
			.NotEmpty();
	}
}

public class GetQuestionnaireByTypeQueryHandler : IRequestHandler<GetQuestionnaireByCategoryQuery, QuestionnaireDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly IQuestionnaireDao _dao;

	public GetQuestionnaireByTypeQueryHandler(IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, IQuestionnaireDao dao)
	{
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<QuestionnaireDto> Handle(GetQuestionnaireByCategoryQuery request, CancellationToken cancellationToken)
	{
		// pre init category object
		var category = new Category{Id = request.CategoryId, Name = "", Color = "", Questionnaire = new Questionnaire{Title = ""}};
		// FHIR query
		if (_fhirOptions.Read) {
			category.Questionnaire = await _dao.SelectByCategoryId(request.CategoryId);
		}
		else {
			category = await _context.Categories
				.Include(category => category.Questionnaire.Questions.OrderBy(question => question.Order))
				.ThenInclude(question => question.AnswerOptions!.OrderBy(answerOption => answerOption.Order))
				.FirstOrDefaultAsync(category => category.Id.Equals(request.CategoryId), cancellationToken);
		}
		if (category is null)
			throw new NotFoundException(nameof(Category), request.CategoryId);


		return _mapper.Map<QuestionnaireDto>(category.Questionnaire);
	}
}
