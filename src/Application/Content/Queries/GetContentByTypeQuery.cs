using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Content.Queries;

public record GetContentByTypeQuery : IRequest<ContentDto>
{
	public ContentType? Type { get; init; }
}

public class GetContentByTypeQueryValidator : AbstractValidator<GetContentByTypeQuery>
{
	public GetContentByTypeQueryValidator()
	{
		RuleFor(query => query.Type)
			.NotNull();
	}
}

public class GetContentByTypeQueryHandler : IRequestHandler<GetContentByTypeQuery, ContentDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetContentByTypeQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ContentDto> Handle(GetContentByTypeQuery request, CancellationToken cancellationToken) =>
		_mapper.Map<ContentDto>((await _context.Content.FirstOrDefaultAsync(content => content.Type.Equals(request.Type!), cancellationToken: cancellationToken)) ??
		                        throw new NotFoundException(nameof(Content), request.Type!));
}
