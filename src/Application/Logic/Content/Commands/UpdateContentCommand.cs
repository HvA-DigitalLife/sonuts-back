using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Content.Commands;

public record UpdateContentCommand : IRequest<ContentDto>
{
	public ContentType? Type { get; set; }
	public string? Title { get; set; }
	public string? Description { get; set; }
}

public class UpdateContentCommandValidator : AbstractValidator<UpdateContentCommand>
{
	public UpdateContentCommandValidator()
	{
		RuleFor(query => query.Type)
			.NotNull();

		RuleFor(query => query.Title)
			.NotEmpty();

		RuleFor(query => query.Description)
			.NotEmpty();
	}
}

public class UpdateContentCommandHandler : IRequestHandler<UpdateContentCommand, ContentDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public UpdateContentCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ContentDto> Handle(UpdateContentCommand request, CancellationToken cancellationToken)
	{
		var content = (await _context.Content.FirstOrDefaultAsync(content => content.Type.Equals(request.Type!), cancellationToken: cancellationToken)) ??
		              throw new NotFoundException(nameof(Content), request.Type!.Value);

		content.Title = request.Title!;
		content.Description = request.Description!;

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ContentDto>(content);
	}
}
