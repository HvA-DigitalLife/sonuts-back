using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Activities.Queries;

public record GetActivityQuery : IRequest<ActivityDto>
{
	public Guid? Id { get; init; }
}

public class GetActivityQueryValidator : AbstractValidator<GetActivityQuery>
{
	public GetActivityQueryValidator()
	{
		RuleFor(query => query.Id)
			.NotNull();
	}
}

public class GetActivityQueryHandler : IRequestHandler<GetActivityQuery, ActivityDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly IActivityDao _dao;

	public GetActivityQueryHandler(IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, IActivityDao dao)
	{
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<ActivityDto> Handle(GetActivityQuery request, CancellationToken cancellationToken) =>
		_mapper.Map<ActivityDto>(await _context.Activities
			.Include(activity => activity.Image)
			.Include(activity => activity.Videos)
			.FirstOrDefaultAsync(activity => activity.Id.Equals(request.Id!.Value), cancellationToken) ?? throw new NotFoundException(nameof(Activity), request.Id!));
}
