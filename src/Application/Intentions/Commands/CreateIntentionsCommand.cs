using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Intentions.Commands;

public class CreateIntentionsCommand : IRequest<IntentionDto>
{
	public Guid? ActivityId { get; init; }
	public int? FrequencyAmount { get; init; }
	public CreateMoment? Moment { get; set; }
	public TimeOnly? Reminder { get; set; }
}

public class CreateMoment
{
	public bool? OnMonday { get; set; }
	public bool? OnTuesday { get; set; }
	public bool? OnWednesday { get; set; }
	public bool? OnThursday { get; set; }
	public bool? OnFriday { get; set; }
	public bool? OnSaturday { get; set; }
	public bool? OnSunday { get; set; }
	public DateTime? Time { get; set; }
	public MomentType? Type { get; set; }
	public string? EventName { get; set; }
}

public class CreateIntentionsCommandValidator : AbstractValidator<CreateIntentionsCommand>
{
	public CreateIntentionsCommandValidator()
	{
		RuleFor(command => command.ActivityId)
			.NotNull();

		RuleFor(command => command.FrequencyAmount)
			.NotEmpty();

		RuleFor(command => command.Moment)
			.NotEmpty();

		When(command => command.Moment is not null, () =>
		{
			RuleFor(command => command.Moment!.OnMonday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnTuesday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnWednesday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnThursday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnFriday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnSaturday)
				.NotEmpty();

			RuleFor(command => command.Moment!.OnSunday)
				.NotEmpty();

			RuleFor(command => command.Moment!.Time)
				.NotEmpty();

			RuleFor(command => command.Moment!.Type)
				.NotEmpty();
		});
	}
}

public class CreateIntentionsCommandHandler : IRequestHandler<CreateIntentionsCommand, IntentionDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public CreateIntentionsCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<IntentionDto> Handle(CreateIntentionsCommand request, CancellationToken cancellationToken)
	{
		Goal entity = new()
		{
			Activity = (await _context.Activities.FirstOrDefaultAsync(activity => activity.Id.Equals(request.ActivityId!.Value), cancellationToken)) ??
			           throw new NotFoundException(nameof(Activity), request.ActivityId!.Value),
			FrequencyAmount = request.FrequencyAmount!.Value,
			Moment = new Moment //TODO: Fix moment
			{
				//OnMonday = request.Moment!.OnMonday!.Value,
				//OnTuesday = request.Moment.OnTuesday!.Value,
				//OnWednesday = request.Moment.OnWednesday!.Value,
				//OnThursday = request.Moment.OnThursday!.Value,
				//OnFriday = request.Moment.OnFriday!.Value,
				//OnSaturday = request.Moment.OnSaturday!.Value,
				//OnSunday = request.Moment.OnSunday!.Value,
				Time = request.Moment!.Time!.Value,
				Type = request.Moment.Type!.Value,
				EventName = request.Moment.EventName
			},
			Reminder = request.Reminder,
			//Participant = await _context.Participants.FirstOrDefaultAsync(participant => participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken) ??
			//              throw new UnauthorizedAccessException()
		};

		//await _context.Intentions.AddAsync(entity, cancellationToken);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<IntentionDto>(entity);
	}
}
