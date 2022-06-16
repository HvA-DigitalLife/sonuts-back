using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Domain.Entities;
using static System.Decimal;
using QuestionType = Sonuts.Domain.Enums.QuestionType;

namespace Sonuts.Application.QuestionnaireResponses.Commands;

public record CreateQuestionnaireResponseCommand : IRequest<QuestionnaireResponseDto>
{
	public Guid? QuestionnaireId { get; set; }
	public ICollection<CreateQuestionResponse> Responses { get; set; } = new List<CreateQuestionResponse>();
}

public record CreateQuestionResponse
{
	public Guid? QuestionId { get; set; }
	public string? Answer { get; set; }
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
			.WithMessage($"{nameof(CreateQuestionnaireResponseCommand.QuestionnaireId)} not found");

		RuleFor(command => command.Responses)
			.NotEmpty();

		RuleForEach(command => command.Responses).ChildRules(validator =>
		{
			validator.RuleFor(response => response.QuestionId)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.Must(command => _context.Questions.FirstOrDefault(question => question.Id.Equals(command!.Value)) is not null)
				.WithMessage("'{PropertyName}' not found");

			validator.RuleFor(response => response.Answer)
				.Cascade(CascadeMode.Stop)
				.NotNull()
				.MustAsync((response, _, _) => IsValidAnswer(response))
				.WithMessage("'{PropertyName}' is not valid");
		});

	}

	private async Task<bool> IsValidAnswer(CreateQuestionResponse response)
	{
		var question = await _context.Questions.FirstAsync(question => question.Id.Equals(response.QuestionId));
		return question.Type switch
		{
			QuestionType.Activity => false,
			QuestionType.Open => !string.IsNullOrWhiteSpace(response.Answer),
			QuestionType.Range => int.TryParse(response.Answer, out int rangeAnswer) && rangeAnswer is >= 0 and <= 10,
			QuestionType.Integer => int.TryParse(response.Answer, out int integerAnswer) && integerAnswer >= 0,
			QuestionType.Decimal => TryParse(response.Answer, out decimal decimalAnswer) && decimalAnswer >= Zero,
			QuestionType.MultipleChoice => question.AnswerOptions?.FirstOrDefault(option => option.Text.ToLower().Equals(response.Answer!.ToLower())) != null,
			QuestionType.MultipleOpen => response.Answer!.Split(';').Length <= question.MaxAnswers!.Value,
			_ => false
		};
	}
}

public class CreateQuestionnaireResponseCommandHandler : IRequestHandler<CreateQuestionnaireResponseCommand, QuestionnaireResponseDto>
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
		var entity = new QuestionnaireResponse
		{
			Questionnaire = await _context.Questionnaires.FirstAsync(questionnaire => questionnaire.Id.Equals(request.QuestionnaireId!.Value), cancellationToken),
			Participant = await _context.Participants.FirstAsync(participant => participant.Id.Equals(Guid.Parse(_currentUserService.AuthorizedUserId)), cancellationToken),
			Responses = request.Responses.Select(response => new QuestionResponse
			{
				Question = _context.Questions.FirstOrDefault(question => question.Id.Equals(response.QuestionId)) ?? throw new NotFoundException(nameof(Question), response.QuestionId!),
				Answer = response.Answer!
			}).ToList()
		};

		await _context.QuestionnaireResponses.AddAsync(entity, cancellationToken);
		
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<QuestionnaireResponseDto>(entity);
	}
}
