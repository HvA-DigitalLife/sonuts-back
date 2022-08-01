using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Goals.Queries;

public record GetGoalsQuery : IRequest<ICollection<GoalDto>>;

public class GetIntentionsQueryHandler : IRequestHandler<GetGoalsQuery, ICollection<GoalDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	private readonly IFhirOptions _fhirOptions;
	private readonly ICarePlanDao _dao;

	public GetIntentionsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IFhirOptions fhirOptions, ICarePlanDao dao)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<ICollection<GoalDto>> Handle(GetGoalsQuery request, CancellationToken cancellationToken) =>
		_mapper.Map<ICollection<GoalDto>>(await _context.Goals
			.Where(goal => goal.CarePlan.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)))
			.Include(goal => goal.Activity.Image)
			.Include(goal => goal.Executions)
			.ToListAsync(cancellationToken));
}
