using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Goals.Commands;

public class ChangeGoalMomentCommand : IRequest<GoalDto>
{
	public Guid Id { get; set; }
	public string? CustomName { get; set; }
	public UpdateMomentCommand? Moment { get; set; }
	public TimeOnly? Reminder { get; set; }
}

public class UpdateMomentCommand
{
	public DayOfWeek? Day { get; init; }
	public TimeOnly? Time { get; init; }
	public MomentType? Type { get; init; }
	public string? EventName { get; init; }
}

public class ChangeGoalMomentCommandValidator : AbstractValidator<ChangeGoalMomentCommand>
{
	public ChangeGoalMomentCommandValidator()
	{
		RuleFor(command => command.Id)
			.NotNull();

		RuleFor(command => command.Moment)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.SetValidator(new UpdateMomentCommandValidator()!);
	}
}

public class UpdateMomentCommandValidator : AbstractValidator<UpdateMomentCommand>
{
	public UpdateMomentCommandValidator()
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

public class ChangeGoalMomentCommandHandler : IRequestHandler<ChangeGoalMomentCommand, GoalDto>
{
	private readonly IApplicationDbContext _context;
	private readonly ICurrentUserService _currentUserService;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly IGoalDao _dao;

	public ChangeGoalMomentCommandHandler(IApplicationDbContext context, ICurrentUserService currentUserService, IMapper mapper, IFhirOptions fhirOptions, IGoalDao dao)
	{
		_context = context;
		_currentUserService = currentUserService;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<GoalDto> Handle(ChangeGoalMomentCommand request, CancellationToken cancellationToken)
	{
		var currentGoal =
			await _context.Goals.Include(goal => goal.Activity).Include(goal => goal.Executions).FirstOrDefaultAsync(
				goal => goal.Id.Equals(request.Id) && goal.CarePlan.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken)
			?? throw new NotFoundException(nameof(Goal), request.Id);

		currentGoal.CustomName = request.CustomName;
		currentGoal.Moment.Day = request.Moment!.Day!.Value;
		currentGoal.Moment.Time = request.Moment!.Time;
		currentGoal.Moment.Type = request.Moment!.Type!.Value;
		currentGoal.Moment.EventName = request.Moment!.EventName;
		currentGoal.Reminder = request.Reminder;

		// FHIR query
		if (_fhirOptions.Write == true) {
			await _dao.Update(currentGoal);
		}
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<GoalDto>(currentGoal);
	}
}
