using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Themes.Queries;
public class GetThemeQuery : IRequest<ThemeDto>
{
	public Guid? Id { get; set; }
}


internal class GetThemeQueryValidator : AbstractValidator<GetThemeQuery>
{
	public GetThemeQueryValidator()
	{
		RuleFor(query => query.Id)
			.NotNull();
	}
}

internal class GetThemeQueryHandler : IRequestHandler<GetThemeQuery, ThemeDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	private readonly IFhirOptions _fhirOptions;
	private readonly IThemeDao _dao;

	public GetThemeQueryHandler(IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, IThemeDao dao)
	{
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;		
	}

	public async Task<ThemeDto> Handle(GetThemeQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ThemeDto>(await _context.Themes
			.Include(theme => theme.Activities).ThenInclude(activity => activity.Image)
			.FindOrNotFoundAsync(request.Id!.Value, cancellationToken));
	}
}
