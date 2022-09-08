using AutoMapper;
using MediatR;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Participants.Queries;
public class GetCurrentParticipantQuery : IRequest<ParticipantDto>
{
}

internal class GetCurrentParticipantQueryHandler : IRequestHandler<GetCurrentParticipantQuery, ParticipantDto>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public GetCurrentParticipantQueryHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<ParticipantDto> Handle(GetCurrentParticipantQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ParticipantDto>(await _context.Participants.FindOrNotFoundAsync(new Guid(_currentUserService.AuthorizedUserId), cancellationToken));
	}
}
