using System;
using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;
using Sonuts.Application.Dtos;
using Sonuts.Application.Logic.Participants.Commands;
using Sonuts.Application.Logic.TinyHabits.Commands;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.TinyHabits.Commands
{
	public record CreateTinyHabitCommand : IRequest<TinyHabitDto>
	{
		public DateOnly? CreatedAt { get; init; } = DateOnly.FromDateTime(DateTime.Now);
		public required Guid? ParticipantId { get; set; }
		public required string? TinyHabitText { get; set; }
	}
}

public class CreateTinyHabitCommandValidator : AbstractValidator<CreateTinyHabitCommand>
{
	public CreateTinyHabitCommandValidator(IIdentityService identityService)
	{
		RuleFor(query => query.CreatedAt)
			.NotNull();

		RuleFor(query => query.ParticipantId)
			.NotNull();

		RuleFor(query => query.TinyHabitText)
			.NotEmpty();
	}
}

public class CreateTinyHabitCommandHandler : IRequestHandler<CreateTinyHabitCommand, TinyHabitDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;

	public CreateTinyHabitCommandHandler(IApplicationDbContext context, IMapper mapper)
	{
		_context = context;
		_mapper = mapper;
	}

	public async Task<TinyHabitDto> Handle(CreateTinyHabitCommand request, CancellationToken cancellationToken)
	{
		var entity = new TinyHabit
		{
			CreatedAt = request?.CreatedAt,
			ParticipantId = request?.ParticipantId,
			TinyHabitText = request?.TinyHabitText
		};

		_context.TinyHabit.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<TinyHabitDto>(entity);
	}
}