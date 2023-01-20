using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.Themes.Queries;
public class GetThemeQuery : IRequest<ThemeDto>
{
	public Guid? Id { get; set; }
}

public class GetThemeQueryValidator : AbstractValidator<GetThemeQuery>
{
	public GetThemeQueryValidator()
	{
		RuleFor(query => query.Id)
			.NotNull();
	}
}

public class GetThemeQueryHandler : IRequestHandler<GetThemeQuery, ThemeDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetThemeQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ThemeDto> Handle(GetThemeQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ThemeDto>(await _context.Themes
			.Include(theme => theme.Activities).ThenInclude(activity => activity.Image)
			.FindOrNotFoundAsync(request.Id!.Value, cancellationToken));
	}
}
