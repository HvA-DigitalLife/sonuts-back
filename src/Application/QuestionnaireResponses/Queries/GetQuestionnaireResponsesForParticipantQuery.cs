using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;
using Sonuts.Application.QuestionnaireResponses.Models;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.QuestionnaireResponses.Queries;

public class GetQuestionnaireResponsesForParticipantQuery : IRequest<IList<QuestionnaireResponseVm>>
{
	public Guid? ParticipantId { get; set; }
}

internal class GetQuestionnaireResponsesForParticipantQueryValidator : AbstractValidator<GetQuestionnaireResponsesForParticipantQuery>
{
	public GetQuestionnaireResponsesForParticipantQueryValidator(ICurrentUserService currentUserService)
	{
		RuleFor(query => query.ParticipantId)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.Must(participantId => participantId!.Value.Equals(Guid.Parse(currentUserService.AuthorizedUserId))
				? true
				: throw new NotFoundException(nameof(Participant), participantId.Value));
	}
}

internal class GetQuestionnaireResponsesForParticipantQueryHandler : IRequestHandler<GetQuestionnaireResponsesForParticipantQuery, IList<QuestionnaireResponseVm>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetQuestionnaireResponsesForParticipantQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IList<QuestionnaireResponseVm>> Handle(GetQuestionnaireResponsesForParticipantQuery request, CancellationToken cancellationToken)
	{
		return await _context.QuestionnaireResponses
			.Where(questionnaireResponse => questionnaireResponse.Participant.Id.Equals(request.ParticipantId!.Value))
			.ProjectToListAsync<QuestionnaireResponseVm>(_mapper.ConfigurationProvider, cancellationToken);
	}
}
