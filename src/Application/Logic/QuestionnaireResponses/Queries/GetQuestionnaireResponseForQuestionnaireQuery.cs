using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Queries;

public class GetQuestionnaireResponseForQuestionnaireQuery : IRequest<QuestionnaireResponseDto>
{
	public Guid? QuestionnaireId { get; set; }
}

public class GetQuestionnaireResponseForQuestionnaireQueryValidator : AbstractValidator<GetQuestionnaireResponseForQuestionnaireQuery>
{
	public GetQuestionnaireResponseForQuestionnaireQueryValidator()
	{
		RuleFor(query => query.QuestionnaireId)
			.NotNull();
	}
}

public class GetQuestionnaireResponseForQuestionnaireQueryHandler : IRequestHandler<GetQuestionnaireResponseForQuestionnaireQuery, QuestionnaireResponseDto>
{
	private readonly IMapper _mapper;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public GetQuestionnaireResponseForQuestionnaireQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService currentUserService)
	{
		_mapper = mapper;
		_context = context;
		_currentUserService = currentUserService;
	}

	public async Task<QuestionnaireResponseDto> Handle(GetQuestionnaireResponseForQuestionnaireQuery request, CancellationToken cancellationToken)
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
