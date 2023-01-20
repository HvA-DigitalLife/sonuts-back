using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.Goals.Queries;

public record GetGoalsQuery : IRequest<ICollection<GoalDto>>;

public class GetIntentionsQueryHandler : IRequestHandler<GetGoalsQuery, ICollection<GoalDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public GetIntentionsQueryHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<ICollection<GoalDto>> Handle(GetGoalsQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ICollection<GoalDto>>(await _context.Goals
			.Where(goal => goal.CarePlan.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)))
			.Include(goal => goal.Activity.Theme.Category)
			.Include(goal => goal.Activity.Theme.Image)
			.Include(goal => goal.Activity.Image)
			.Include(goal => goal.Executions)
			.ToListAsync(cancellationToken));
	}
}
