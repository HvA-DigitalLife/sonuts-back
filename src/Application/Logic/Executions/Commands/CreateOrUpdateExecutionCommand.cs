using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Executions.Commands;

public record CreateOrUpdateExecutionCommand : IRequest<ExecutionDto>
{
	public Guid? GoalId { get; init; }
	public bool? IsDone { get; init; }
	public Guid? ExecutionId { get; init; }
}

public class CreateOrUpdateExecutionCommandValidator : AbstractValidator<CreateOrUpdateExecutionCommand>
{
	public CreateOrUpdateExecutionCommandValidator()
	{
		RuleFor(query => query.GoalId)
			.NotEmpty();

		RuleFor(query => query.IsDone)
			.NotNull();
	}
}

internal class CreateOrUpdateExecutionCommandHandler : IRequestHandler<CreateOrUpdateExecutionCommand, ExecutionDto>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public CreateOrUpdateExecutionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<ExecutionDto> Handle(CreateOrUpdateExecutionCommand request, CancellationToken cancellationToken)
	{
		var carePlan = await _context.CarePlans
			               .Include(plan => plan.Goals)
			               .FirstOrDefaultAsync(cp => cp.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken)
		               ?? throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		if (!carePlan.Goals.Any(g => g.Id.Equals(request.GoalId!.Value)))
			throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		var goal = await _context.Goals
			           .Include(g => g.Executions)
			           .FirstOrDefaultAsync(g => g.Id.Equals(request.GoalId!.Value), cancellationToken)
		           ?? throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		Execution? existingExecution = null;
		if (request.ExecutionId is not null)
		{
			existingExecution = goal.Executions.FirstOrDefault(e => e.Goal.Id.Equals(goal.Id) && e.Id.Equals(request.ExecutionId.Value))
			                    ?? throw new NotFoundException(nameof(Execution), request.ExecutionId.Value);

			existingExecution.IsDone = request.IsDone!.Value;
		}

		var entity = existingExecution ?? new Execution
		{
			IsDone = request.IsDone!.Value,
			Goal = goal
		};

		if (existingExecution is null)
			_context.Executions.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ExecutionDto>(entity);
	}
}
