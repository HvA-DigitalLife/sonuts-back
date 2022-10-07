using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Executions.Commands;

public record CreateExecutionCommand : IRequest<ExecutionDto>
{
	public Guid? GoalId { get; init; }
	public bool? IsDone { get; init; }
}

public class CreateExecutionCommandValidator : AbstractValidator<CreateExecutionCommand>
{
	public CreateExecutionCommandValidator()
	{
		RuleFor(query => query.GoalId)
			.NotEmpty();

		RuleFor(query => query.IsDone)
			.NotNull();
	}
}

public class CreateExecutionCommandHandler : IRequestHandler<CreateExecutionCommand, ExecutionDto>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public CreateExecutionCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<ExecutionDto> Handle(CreateExecutionCommand request, CancellationToken cancellationToken)
	{
		var carePlan = await _context.CarePlans
			               .Include(plan => plan.Goals)
			               .FirstOrDefaultAsync(cp => cp.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken)
		               ?? throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		if (!carePlan.Goals.Any(g => g.Id.Equals(request.GoalId!.Value)))
			throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		var goal = await _context.Goals.Include(g => g.Executions).FirstOrDefaultAsync(g => g.Id.Equals(request.GoalId!.Value), cancellationToken) ??
		           throw new NotFoundException(nameof(Goal), request.GoalId!.Value);

		var currentExecution = goal.Executions.FirstOrDefault(e => e.Goal.Id.Equals(goal.Id) && e.CreatedAt.Date.Equals(DateTime.Now.Date)); //TODO Check week number instead of day
		
		if (currentExecution is not null)
			currentExecution.IsDone = request.IsDone!.Value;

		var entity = currentExecution ?? new Execution
		{
			IsDone = request.IsDone!.Value,
			Goal = goal
		};

		if (currentExecution is null)
			_context.Executions.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ExecutionDto>(entity);
	}
}
