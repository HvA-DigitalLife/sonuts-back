using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Intentions.Queries;

public record GetIntentionsQuery : IRequest<ICollection<IntentionDto>>;

public class GetIntentionsQueryHandler : IRequestHandler<GetIntentionsQuery, ICollection<IntentionDto>>
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

	public async Task<ICollection<IntentionDto>> Handle(GetIntentionsQuery request, CancellationToken cancellationToken) =>
		await _context.Intentions
			.Where(intention => intention.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)))
			.Include(intention => intention.Activity.Image)
			.Include(intention => intention.Executions)
			.ProjectToListAsync<IntentionDto>(_mapper.ConfigurationProvider, cancellationToken);
}
