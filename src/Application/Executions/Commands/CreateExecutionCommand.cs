using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Executions.Commands;

public record CreateExecutionCommand : IRequest<ExecutionDto>
{
	public bool? IsDone { get; init; }
	public Guid? IntentionId { get; init; }
}

public class CreateExecutionCommandValidator : AbstractValidator<CreateExecutionCommand>
{
	public CreateExecutionCommandValidator()
	{
		RuleFor(query => query.IsDone)
			.NotNull();

		RuleFor(query => query.IntentionId)
			.NotEmpty();
	}
}

public class CreateExecutionCommandHandler : IRequestHandler<CreateExecutionCommand, ExecutionDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CreateExecutionCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<ExecutionDto> Handle(CreateExecutionCommand request, CancellationToken cancellationToken)
	{
		Execution entity = new()
		{
			IsDone = request.IsDone!.Value,
			Intention = (await _context.Intentions.FirstOrDefaultAsync(content => content.Id.Equals(request.IntentionId!), cancellationToken)) ??
			            throw new NotFoundException(nameof(Intention), request.IntentionId!)
		};

		await _context.Executions.AddAsync(entity, cancellationToken);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ExecutionDto>(entity);
	}
}