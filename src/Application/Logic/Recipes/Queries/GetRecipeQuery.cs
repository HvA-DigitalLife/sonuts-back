using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Recipes.Queries;
public class GetRecipeQuery : IRequest<RecipeDto>
{
	public Guid? Id { get; set; }
}


public class GetRecipeQueryValidator : AbstractValidator<GetRecipeQuery>
{
	public GetRecipeQueryValidator()
	{
		RuleFor(query => query.Id)
			.NotNull();
	}
}

public class GetRecipeQueryHandler : IRequestHandler<GetRecipeQuery, RecipeDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetRecipeQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<RecipeDto> Handle(GetRecipeQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<RecipeDto>(await _context.Recipes
			.Include(recipe => recipe.Image)
			.Include(recipe => recipe.Ingredients)
			.Include(recipe => recipe.Steps)
			.FirstOrDefaultAsync(x => x.Id.Equals(request.Id!.Value), cancellationToken))
			?? throw new NotFoundException(nameof(Recipe), request.Id!.Value);
	}
}
