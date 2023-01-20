using AutoMapper;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;

namespace Sonuts.Application.Logic.Participants.Queries;

public record GetParticipantsCsvQuery : IRequest<byte[]>;

internal class GetParticipantsCsvQueryHandler : IRequestHandler<GetParticipantsCsvQuery, byte[]>
{
	private readonly IApplicationDbContext _context;
	private readonly ICsvFileBuilder _csvFileBuilder;
	private readonly IMapper _mapper;

	public GetParticipantsCsvQueryHandler(IApplicationDbContext context, ICsvFileBuilder csvFileBuilder, IMapper mapper)
	{
		_context = context;
		_csvFileBuilder = csvFileBuilder;
		_mapper = mapper;
	}

	public async Task<byte[]> Handle(GetParticipantsCsvQuery request, CancellationToken cancellationToken)
	{
		return await _csvFileBuilder.BuildParticipantsFile(await _context.Participants.ProjectToListAsync<OverviewParticipantDto>(_mapper.ConfigurationProvider, cancellationToken));
	}
}
