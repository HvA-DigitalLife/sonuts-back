using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.CarePlans.Queries;

public class GetCurrentCarePlanQuery : IRequest<CarePlanDto>
{
	public Guid? ParticipantId { get; set; }
}

internal class GetCurrentCarePlanQueryValidator : AbstractValidator<GetCurrentCarePlanQuery>
{
	public GetCurrentCarePlanQueryValidator(ICurrentUserService currentUserService)
	{
		RuleFor(query => query.ParticipantId)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.Must(participantId => participantId!.Value.Equals(Guid.Parse(currentUserService.AuthorizedUserId))
				? true
				: throw new NotFoundException(nameof(Participant), participantId.Value));
	}
}

internal class GetCurrentCarePlanQueryHandler : IRequestHandler<GetCurrentCarePlanQuery, CarePlanDto>
{
	private readonly IMapper _mapper;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;

	public GetCurrentCarePlanQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService currentUserService)
	{
		_mapper = mapper;
		_context = context;
		_currentUserService = currentUserService;
	}

	public async Task<CarePlanDto> Handle(GetCurrentCarePlanQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<CarePlanDto>(await _context.CarePlans.Current(Guid.Parse(_currentUserService.AuthorizedUserId), cancellationToken) ?? throw new NotFoundException());
	}
}
