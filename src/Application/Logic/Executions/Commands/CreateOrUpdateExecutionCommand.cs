using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;

namespace Sonuts.Application.Logic.Executions.Commands;

public record CreateOrUpdateExecutionCommand : IRequest<ExecutionWithMotivationalMessageVm>
{
	public Guid? GoalId { get; init; }

	public bool? IsDone { get; init; }

	public int? Amount { get; init; }

	public string? Reason { get; init; }

	public Guid? ExecutionId { get; init; }

	public DateOnly? PastDate { get; init; }
}

public class CreateOrUpdateExecutionCommandValidator : AbstractValidator<CreateOrUpdateExecutionCommand>
{
	public CreateOrUpdateExecutionCommandValidator()
	{
		RuleFor(query => query.GoalId)
			.NotEmpty();

		RuleFor(query => query.IsDone)
			.NotNull();

		RuleFor(query => query.Amount)
			.NotNull()
			.InclusiveBetween(0, 100);

		RuleFor(query => query.PastDate)
			.LessThan(DateOnly.FromDateTime(DateTime.Now));
	}
}

internal class CreateOrUpdateExecutionCommandHandler : IRequestHandler<CreateOrUpdateExecutionCommand, ExecutionWithMotivationalMessageVm>
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

	public async Task<ExecutionWithMotivationalMessageVm> Handle(CreateOrUpdateExecutionCommand request, CancellationToken cancellationToken)
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
			existingExecution.Amount = request.Amount!.Value;
			existingExecution.Reason = request.Reason;
		}

		var entity = existingExecution ?? new Execution
		{
			IsDone = request.IsDone!.Value,
			Amount = request.Amount!.Value,
			Reason = request.Reason,
			Goal = goal,
			CreatedAt = request.PastDate ?? DateOnly.FromDateTime(DateTime.Now)
		};

		if (existingExecution is null)
			_context.Executions.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		MotivationalMessage[] motivationalMessages;
		//Het gaat om een deels behaalde execution (minder dan 100%).
		if (entity.Amount < 100)
		{
			//Bepaal of we in week 3 van het Careplan zitten
			//Wanneer de einddatum van het careplan kleiner is dan de datum waarop de execution gemaakt is + 7, dan zitten we in de laatste week.
			if (carePlan.End < entity.CreatedAt.AddDays(7))
			{
				//Als we in week 3 zitten:
				//meer dan 60% gemiddeld -> positieve message
				var averageAmountExecutions = goal.Executions.Average(x => x.Amount);
				if (averageAmountExecutions >= 60.0)
				{
					//Hier wordt de motivationalmessage er eentje die positief is en boven de 50% heeft in de DB.
					motivationalMessages = await _context.MotivationalMessages.Where(mm => mm.MaxPercentage > 50).ToArrayAsync(cancellationToken);
				}
				//Het gemiddelde behaalde percentage is minder dan 60%, de motivational message wordt weer bepaald op basis van het percentage van het huidige doel.
				else
				{
					motivationalMessages = entity.Amount < 50 ? await _context.MotivationalMessages.Where(mm => mm.MinPercentage == 400 && mm.MaxPercentage == 400).ToArrayAsync(cancellationToken)
					: await _context.MotivationalMessages.Where(mm => mm.MinPercentage == 450 && mm.MaxPercentage == 450).ToArrayAsync(cancellationToken);
				}
			}
			else
			{
				//We zitten in week 1 of 2
				motivationalMessages = await _context.MotivationalMessages.Where(mm => mm.MinPercentage == 200 && mm.MaxPercentage == 200).ToArrayAsync(cancellationToken);
			}
		}
		//Het gaat om een geheel gehaalde execution (100%)
		else
		{
			int streakCounter = 0;
			if (goal.Executions != null)
			{

				var execList = goal.Executions.OrderBy(exec => exec.CreatedAt).ToArray();

				int counter = 0;

				foreach (var exec in execList)
				{
					//logica voor 3 winning streak
					//lopen door loop van executions (gesorteerd op datum met created At)
					//wanneer laatste 3 100% zijn is er een winning streak
					if (counter >= (execList.Length - 2))
					{
						if (exec.Amount == 100)
						{
							streakCounter++;
						}
					}
					counter++;
				}
			}

			motivationalMessages = streakCounter >= 2
				? await _context.MotivationalMessages.Where(mm => mm.MinPercentage == 300 && mm.MaxPercentage == 300).ToArrayAsync(cancellationToken)
				: await _context.MotivationalMessages.Where(mm => mm.MinPercentage <= request.Amount && mm.MaxPercentage >= request.Amount).ToArrayAsync(cancellationToken);
		}

		if (motivationalMessages == null)
		{
			motivationalMessages = await _context.MotivationalMessages.Where(mm => mm.MinPercentage <= request.Amount && mm.MaxPercentage >= request.Amount).ToArrayAsync(cancellationToken);
		}

		return new ExecutionWithMotivationalMessageVm
		{
			Execution = _mapper.Map<ExecutionDto>(entity),
			MotivationalMessage = _mapper.Map<MotivationalMessageDto>(motivationalMessages.MinBy(_ => Guid.NewGuid()))
		};
	}
}

public record ExecutionWithMotivationalMessageVm
{
	public required ExecutionDto Execution { get; init; }
	public required MotivationalMessageDto? MotivationalMessage { get; init; }
}
