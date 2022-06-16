using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
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

	public GetQuestionnaireByTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<QuestionnaireDto> Handle(GetQuestionnaireByCategoryQuery request, CancellationToken cancellationToken)
	{
		var category = await _context.Categories
			.Include(category => category.Questionnaire)
			.FirstOrDefaultAsync(category => category.Id.Equals(request.CategoryId!.Value), cancellationToken);

		if (category == null)
			throw new NotFoundException(nameof(Category), request.CategoryId!.Value);

		return _mapper.Map<QuestionnaireDto>(category.Questionnaire);
	}
}
