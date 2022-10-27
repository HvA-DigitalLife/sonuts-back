using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.Recipes.Queries;
public class GetRecipesQuery : IRequest<List<RecipeDto>>
{
	public Guid? ThemeId { get; set; }
}


public class GetRecipesQueryValidator : AbstractValidator<GetRecipesQuery>
{
	public GetRecipesQueryValidator()
	{
		RuleFor(query => query.ThemeId)
			.NotNull();
	}
}

public class GetRecipesQueryHandler : IRequestHandler<GetRecipesQuery, List<RecipeDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetRecipesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<List<RecipeDto>> Handle(GetRecipesQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<List<RecipeDto>>(await _context.Recipes
			.Include(recipe => recipe.Image)
			.Where(recipe => recipe.Theme.Id.Equals(request.ThemeId!.Value))
			.ToListAsync(cancellationToken));
	}
}
