using AutoMapper;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Common.Mappings;

namespace Sonuts.Application.Categories.Queries;

public record GetCategoriesQuery : IRequest<ICollection<CategoryDto>>;

public class GetCategoriesQueryHandler : IRequestHandler<GetCategoriesQuery, ICollection<CategoryDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	private readonly IFhirOptions _fhirOptions;

	private readonly ICategoryDao _dao;

	public GetCategoriesQueryHandler(IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, ICategoryDao dao)
	{
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<ICollection<CategoryDto>> Handle(GetCategoriesQuery request, CancellationToken cancellationToken)
	{
		
		var categories = _fhirOptions.Read ?  
			await _dao.SelectAll():
			await _context.Categories
			.Include(category => category.Themes)
			.Where(category => category.IsActive)
			.ToListAsync(cancellationToken);



		return _mapper.Map<ICollection<CategoryDto>>(categories);
	}
}
