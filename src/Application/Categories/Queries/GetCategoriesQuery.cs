using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Mappings;

namespace Sonuts.Application.Categories.Queries;

public record GetCategoriesQuery : IRequest<ICollection<CategoryDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ICollection<CategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ICollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<ICollection<CategoryDto>>(await _context.Categories
			.Include(category => category.Themes)
			.Where(category => category.IsActive)
			.ToListAsync(cancellationToken));
	}
}
