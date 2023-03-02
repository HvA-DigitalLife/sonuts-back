using AutoMapper;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Participants.Queries;

public record GetAllParticipantsQuery : IRequest<IEnumerable<ParticipantRecord>>;

internal class GetAllParticipantsQueryHandler : IRequestHandler<GetAllParticipantsQuery, IEnumerable<ParticipantRecord>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetAllParticipantsQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<IEnumerable<ParticipantRecord>> Handle(GetAllParticipantsQuery request, CancellationToken cancellationToken)
	{
		return await _context.Participants
			.ProjectToListAsync<ParticipantRecord>(_mapper.ConfigurationProvider, cancellationToken);
	}
}

public class ParticipantRecord : IMapFrom<Participant>
{
	public Guid Id { get; set; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateOnly? Birth { get; set; }
	public string? Gender { get; set; }
	public decimal? Weight { get; set; }
	public decimal? Height { get; set; }
	public string? MaritalStatus { get; set; }
	public bool IsActive { get; set; } = true;
}
