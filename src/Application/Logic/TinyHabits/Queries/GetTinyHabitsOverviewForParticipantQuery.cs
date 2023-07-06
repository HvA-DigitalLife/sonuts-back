using System;
using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Logic.TinyHabits.Queries;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.TinyHabits.Queries;

public class GetTinyHabitsOverviewForParticipantQuery : IRequest<IEnumerable<TinyHabitRecord>>
{
	public Guid? ParticipantId { get; set; }
}


public class GetTinyHabitsOverviewForParticipantQueryHandler : IRequestHandler<GetTinyHabitsOverviewForParticipantQuery, IEnumerable<TinyHabitRecord>>
{
	private readonly ICurrentUserService _currentUserService;
	private readonly IApplicationDbContext _context;
private readonly IMapper _mapper;

public GetTinyHabitsOverviewForParticipantQueryHandler(ICurrentUserService currentUserService,IApplicationDbContext context, IMapper mapper)
{
		_currentUserService = currentUserService;
		_context = context;
	_mapper = mapper;
}

public async Task<IEnumerable<TinyHabitRecord>> Handle(GetTinyHabitsOverviewForParticipantQuery request, CancellationToken cancellationToken)
{
	return await _context.TinyHabit
		.Where(th => th.Participant.Id == request.ParticipantId)
		.ProjectToListAsync<TinyHabitRecord>(_mapper.ConfigurationProvider, cancellationToken);
}
}

public class TinyHabitRecord : IMapFrom<TinyHabit>
{
public Guid Id { get; set; }
public DateOnly CreatedAt { get; set; }
public required Participant Participant { get; set; }
public required Category Category { get; set; }
public required string TinyHabitText { get; set; }
}
