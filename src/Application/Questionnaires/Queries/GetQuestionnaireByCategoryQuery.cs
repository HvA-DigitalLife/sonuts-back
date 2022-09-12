using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Questionnaires.Queries;

public record GetQuestionnaireByCategoryQuery : IRequest<QuestionnaireDto>
{
	public Guid? CategoryId { get; set; }
}

public class GetQuestionnaireByTypeQueryValidator : AbstractValidator<GetQuestionnaireByCategoryQuery>
{
	public GetQuestionnaireByTypeQueryValidator()
	{
		RuleFor(query => query.CategoryId)
			.NotNull();
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
		var category = new Category{Id = request.CategoryId!.Value};
		
		// Fhir query
		if (_fhirOptions.Read) {
			category.Questionnaire = await _dao.SelectByCategoryId(request.CategoryId!.Value);
		}
		// entity query
		else {
			category = await _context.Categories
				.Include(category => category.Questionnaire.Questions.OrderBy(question => question.Order))
				.ThenInclude(question => question.AnswerOptions!.OrderBy(answerOption => answerOption.Order))
				.FirstOrDefaultAsync(category => category.Id.Equals(request.CategoryId!.Value), cancellationToken);

		}
		// exception if category is non existant
		if (category == null)
			throw new NotFoundException(nameof(Category), request.CategoryId!.Value);


		return _mapper.Map<QuestionnaireDto>(category.Questionnaire);
	}
}
