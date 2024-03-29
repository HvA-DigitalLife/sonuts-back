using System;
using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Commands;

public record CreateQuestionnaireResponseCommand : IRequest<QuestionnaireResponseDto>
{
	public Guid? QuestionnaireId { get; init; }
	public List<CreateQuestionResponse> Responses { get; init; } = new();
}

public record CreateQuestionResponse
{
	public Guid? QuestionId { get; init; }
	public string? Answer { get; init; }
}

public class CreateQuestionnaireResponseCommandValidator : AbstractValidator<CreateQuestionnaireResponseCommand>
{
	private readonly IApplicationDbContext _context;

	public CreateQuestionnaireResponseCommandValidator(IApplicationDbContext context)
	{
		_context = context;

		RuleFor(command => command.QuestionnaireId)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.Must(command => _context.Questionnaires.FirstOrDefault(questionnaire => questionnaire.Id.Equals(command!.Value)) is not null)
			.WithMessage("Questionnaire '{PropertyValue}' not found");

		RuleFor(command => command.Responses)
			.NotEmpty();

		RuleForEach(command => command.Responses).ChildRules(validator =>
		{
			validator.RuleFor(response => response)
				.NotNull();

			validator.RuleFor(response => response.QuestionId)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.Must(command => _context.Questions.FirstOrDefault(question => question.Id.Equals(command!.Value)) is not null)
				.WithMessage("Question '{PropertyValue}' not found");

			validator.RuleFor(response => response.Answer)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.MustAsync((response, _, _) => IsValidAnswerAsync(response))
				.WithMessage("'{PropertyValue}' is not a valid answer");
		});
	}

	private async Task<bool> IsValidAnswerAsync(CreateQuestionResponse response)
	{
		var question = await _context.Questions
						   .Include(question => question.AnswerOptions)
						   .FirstOrDefaultAsync(question => question.Id.Equals(response.QuestionId!.Value)) ??
					   throw new NotFoundException(nameof(Question), response.QuestionId!.Value);

		if (string.IsNullOrWhiteSpace(response.Answer) && !question.IsRequired)
			return true;

		return question.Type switch
		{
			QuestionType.Boolean =>
				!string.IsNullOrWhiteSpace(response.Answer) && (response.Answer.Equals("Yes") || response.Answer.Equals("No")),

			QuestionType.String =>
				!string.IsNullOrWhiteSpace(response.Answer),

			QuestionType.Integer =>
				int.TryParse(response.Answer, out var integerAnswer)
				&& integerAnswer >= 0
				//&& (question.Min is null || integerAnswer >= question.Min)
				&& (question.Max is null || integerAnswer <= question.Max),

			QuestionType.Decimal =>
				decimal.TryParse(response.Answer, out var decimalAnswer) && decimalAnswer >= decimal.Zero,

			QuestionType.Duration =>
				response.Answer!.Split(':').Length == 2
				&& int.TryParse(response.Answer!.Split(':')[0], out var hours)
				&& int.TryParse(response.Answer!.Split(':')[1], out var minutes),

			QuestionType.Choice =>
				question.AnswerOptions?.FirstOrDefault(option => option.Value.ToLower().Equals(response.Answer!.ToLower())) != null || response.Answer!.Equals("0"),

			QuestionType.OpenChoice =>
				!string.IsNullOrWhiteSpace(response.Answer),

			QuestionType.MultiChoice =>
				true, //TODO

			QuestionType.MultiOpenChoice =>
				true, //TODO

			_ => false
		};
	}
}

internal class CreateQuestionnaireResponseCommandHandler : IRequestHandler<CreateQuestionnaireResponseCommand, QuestionnaireResponseDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;

	public CreateQuestionnaireResponseCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
	}

	public async Task<QuestionnaireResponseDto> Handle(CreateQuestionnaireResponseCommand request, CancellationToken cancellationToken)
	{
		var lastCarePlan = await _context
			.CarePlans
			.OrderByDescending(carePlan => carePlan.Start)
			.FirstOrDefaultAsync(carePlan => carePlan.Participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken);

		if (lastCarePlan is not null && lastCarePlan.End > DateOnly.FromDateTime(DateTime.Now))
			throw new ForbiddenAccessException("Current care plan has not ended");

		var entity = new QuestionnaireResponse
		{
			Questionnaire = await _context.Questionnaires.FirstOrDefaultAsync(questionnaire => questionnaire.Id.Equals(request.QuestionnaireId!.Value), cancellationToken)
							?? throw new NotFoundException(nameof(Questionnaire), request.QuestionnaireId!),
			Participant = (await _context.Participants.FirstOrDefaultAsync(participant => participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken))!,
			Responses = request.Responses.Select(response => new QuestionResponse
			{
				Question = _context.Questions.FirstOrDefault(question => question.Id.Equals(response.QuestionId))
						   ?? throw new NotFoundException(nameof(Question), response.QuestionId!),
				Answer = response.Answer!
			}).ToList()
		};

		_context.QuestionnaireResponses.Add(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<QuestionnaireResponseDto>(entity);
	}
}
