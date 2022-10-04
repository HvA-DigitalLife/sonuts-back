using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using System.Linq;

namespace Sonuts.Application.QuestionnaireResponses.Queries;

public class GetQuestionnaireResponseForQuestionnaireQuery : IRequest<QuestionnaireResponseDto>
{
	public Guid? QuestionnaireId { get; set; }
}

internal class GetQuestionnaireResponseForQuestionnaireQueryValidator : AbstractValidator<GetQuestionnaireResponseForQuestionnaireQuery>
{
	public GetQuestionnaireResponseForQuestionnaireQueryValidator()
	{
		RuleFor(query => query.QuestionnaireId)
			.NotNull();
	}
}

internal class GetQuestionnaireResponseForQuestionnaireQueryHandler : IRequestHandler<GetQuestionnaireResponseForQuestionnaireQuery, QuestionnaireResponseDto>
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
		return _mapper.Map<QuestionnaireResponseDto>((await _context.QuestionnaireResponses
			.Include(questionnaireResponse => questionnaireResponse.Responses)
			.OrderByDescending(questionnaireResponse => questionnaireResponse.CreatedAt)
			.Where(questionnaireResponse => questionnaireResponse.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)) && questionnaireResponse.Questionnaire.Id.Equals(request.QuestionnaireId))
			.Take(1)
			.ToListAsync(cancellationToken))
			.FirstOrDefault()
			?? throw new NotFoundException());
	}
}
