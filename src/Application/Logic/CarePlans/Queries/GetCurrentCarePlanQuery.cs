using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.CarePlans.Queries;

public class GetCurrentCarePlanQuery : IRequest<CarePlanDto>
{
	public Guid? ParticipantId { get; set; }
}

public class GetCurrentCarePlanQueryValidator : AbstractValidator<GetCurrentCarePlanQuery>
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

public class GetCurrentCarePlanQueryHandler : IRequestHandler<GetCurrentCarePlanQuery, CarePlanDto>
{
	private readonly IMapper _mapper;
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IFhirOptions _fhirOptions;
	private readonly ICarePlanDao _dao;

	public GetCurrentCarePlanQueryHandler(IMapper mapper, IApplicationDbContext context, ICurrentUserService currentUserService, IFhirOptions fhirOptions, ICarePlanDao dao)
	{
		_mapper = mapper;
		_context = context;
		_currentUserService = currentUserService;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<CarePlanDto> Handle(GetCurrentCarePlanQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<CarePlanDto>(await _context.CarePlans.Current(Guid.Parse(_currentUserService.AuthorizedUserId), cancellationToken) ?? throw new NotFoundException());
	}
}
