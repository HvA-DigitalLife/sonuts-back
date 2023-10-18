using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Extensions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Validators;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.CarePlans.Commands;

public class CreateCarePlanCommand : IRequest<CarePlanDto>
{
	public List<CreateGoalsCommand> Goals { get; init; } = new();
}

public class CreateGoalsCommand
{
	public Guid? ActivityId { get; init; }
	public int? FrequencyAmount { get; set; }
	public CreateMomentCommand? Moment { get; set; }
	public TimeOnly? Reminder { get; set; }
}

public class CreateMomentCommand
{
	public DayOfWeek? Day { get; init; }
	public TimeOnly? Time { get; init; }
	public MomentType? Type { get; init; }
	public string? EventName { get; init; }
}

public class CreateCarePlanCommandValidator : AbstractValidator<CreateCarePlanCommand>
{
	public CreateCarePlanCommandValidator(IApplicationDbContext context)
	{
		RuleFor(command => command.Goals)
			.Cascade(CascadeMode.Stop)
			.NotEmpty();

		RuleForEach(command => command.Goals)
			.SetValidator(new CreateGoalsCommandValidator(context));
	}
}

public class CreateGoalsCommandValidator : AbstractValidator<CreateGoalsCommand>
{
	public CreateGoalsCommandValidator(IApplicationDbContext context)
	{
		RuleFor(command => command.ActivityId)
			.NotNull()
			.Exists(context.Activities);


		RuleFor(command => command.FrequencyAmount)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.GreaterThanOrEqualTo(0);

		RuleFor(command => command.Moment)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.SetValidator(new CreateMomentCommandValidator()!);
	}
}

public class CreateMomentCommandValidator : AbstractValidator<CreateMomentCommand>
{
	public CreateMomentCommandValidator()
	{
		RuleFor(command => command.Day)
			.NotNull();

		RuleFor(command => command.Type)
			.NotNull();

		When(command => command.EventName is not null, delegate
		{
			RuleFor(command => command.EventName)
				.NotEmpty();

			When(command => command.Type == MomentType.Specific, delegate
			{
				RuleFor(command => command.Time)
					.NotNull();
			});

			When(command => command.Type != MomentType.Specific, delegate
			{
				RuleFor(command => command.EventName)
					.NotNull();
			});
		});
	}
}

public class CreateCarePlanCommandHandler : IRequestHandler<CreateCarePlanCommand, CarePlanDto>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;

	public CreateCarePlanCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
	}

	public async Task<CarePlanDto> Handle(CreateCarePlanCommand request, CancellationToken cancellationToken)
	{
		var currentCarePlan = await _context.CarePlans.Current(Guid.Parse(_currentUserService.AuthorizedUserId), cancellationToken);

		if (currentCarePlan is not null && currentCarePlan.End > DateOnly.FromDateTime(DateTime.Now))
			throw new ForbiddenAccessException("Current care plan has not ended");

		var start = DateOnly.FromDateTime(DateTime.Today.AddDays(1));

		List<Goal> goals = new();

		foreach (var goal in request.Goals)
		{
			goals.Add(new Goal
			{
				Activity = await _context.Activities.FindOrNotFoundAsync(goal.ActivityId!.Value, cancellationToken),
				FrequencyAmount = goal.FrequencyAmount!.Value,
				Moment = new Moment
				{
					Day = goal.Moment!.Day!.Value,
					Time = goal.Moment!.Time,
					Type = goal.Moment!.Type!.Value,
					EventName = goal.Moment!.EventName
				},
				Reminder = goal.Reminder
			});
		}
		
		var carePlan = new CarePlan
		{
			Start = start,
			End = start.AddDays(20),
			Participant = await _context.Participants.FirstAsync(participant => participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken),
			Goals = goals
		};

		_context.CarePlans.Add(carePlan);
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<CarePlanDto>(carePlan);
	}
}
