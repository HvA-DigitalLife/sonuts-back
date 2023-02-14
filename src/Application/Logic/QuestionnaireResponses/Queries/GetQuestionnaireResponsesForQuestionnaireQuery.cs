using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Queries;

public class GetQuestionnaireResponsesForQuestionnaireQuery : IRequest<QuestionnaireResponseDto>
{
	public Guid? QuestionnaireId { get; set; }
}

public class GetQuestionnaireResponsesForQuestionnaireQueryValidator : AbstractValidator<GetQuestionnaireResponsesForQuestionnaireQuery>
{
	public GetQuestionnaireResponsesForQuestionnaireQueryValidator()
	{
		RuleFor(query => query.QuestionnaireId)
			.NotNull();
	}
}

public class GetQuestionnaireResponsesForQuestionnaireQueryHandler : IRequestHandler<GetQuestionnaireResponsesForQuestionnaireQuery, QuestionnaireResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public GetQuestionnaireResponsesForQuestionnaireQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService currentUserService)
	{
		_mapper = mapper;
		_context = context;
		_currentUserService = currentUserService;
	}

	public async Task<QuestionnaireResponseDto> Handle(GetQuestionnaireResponsesForQuestionnaireQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<QuestionnaireResponseDto>(await _context.QuestionnaireResponses
			.Include(questionnaireResponse => questionnaireResponse.Responses)
			.ThenInclude(response => response.Question)
			.OrderByDescending(questionnaireResponse => questionnaireResponse.CreatedAt)
			.FirstOrDefaultAsync(questionnaireResponse => questionnaireResponse.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId))
			                                              && questionnaireResponse.Questionnaire.Id.Equals(request.QuestionnaireId), cancellationToken)
		                                             ?? throw new NotFoundException());
	}
}
