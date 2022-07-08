using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Intentions.Queries;

public record GetIntentionsQuery : IRequest<ICollection<GoalDto>>;

public class GetIntentionsQueryHandler : IRequestHandler<GetIntentionsQuery, ICollection<GoalDto>>
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

	public async Task<ICollection<GoalDto>> Handle(GetIntentionsQuery request, CancellationToken cancellationToken) =>
		await _context.Goals
			.Where(goal => goal.CarePlan.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)))
			.Include(goal => goal.Activity.Image)
			.Include(goal => goal.Executions)
			.ProjectToListAsync<GoalDto>(_mapper.ConfigurationProvider, cancellationToken);
}
