using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Activities.Queries;

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

	public GetActivityQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ActivityDto> Handle(GetActivityQuery request, CancellationToken cancellationToken) =>
		_mapper.Map<ActivityDto>(await _context.Activities
			.FirstOrDefaultAsync(activity => activity.Id.Equals(request.Id), cancellationToken) ?? throw new NotFoundException(nameof(Activity), request.Id!));
}
