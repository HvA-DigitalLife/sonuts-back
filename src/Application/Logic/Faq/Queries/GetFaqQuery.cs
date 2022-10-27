using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;

namespace Sonuts.Application.Logic.Faq.Queries;
public class GetFaqQuery : IRequest<List<FaqDto>>
{
	public Guid? ThemeId { get; set; }
}


public class GetFaqQueryValidator : AbstractValidator<GetFaqQuery>
{
	public GetFaqQueryValidator()
	{
		RuleFor(query => query.ThemeId)
			.NotNull();
	}
}

public class GetFaqQueryHandler : IRequestHandler<GetFaqQuery, List<FaqDto>>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public GetFaqQueryHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<List<FaqDto>> Handle(GetFaqQuery request, CancellationToken cancellationToken)
	{
		return _mapper.Map<List<FaqDto>>(await _context.Faq.Where(x => x.Theme.Id.Equals(request.ThemeId!.Value)).ToListAsync(cancellationToken));
	}
}
