using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using Sonuts.Domain.Enums;

namespace Sonuts.Application.Logic.Questionnaires.Commands;

public record CreateQuestionnaireCommand : IRequest<QuestionnaireDto>
{
	public required string? Title { get; init; }
	public string? Description { get; init; }
	public ICollection<CreateQuestionCommand> Questions { get; init; } = new List<CreateQuestionCommand>();
}

public record CreateQuestionCommand
{
	public Guid? Id { get; init; }
	public QuestionType? Type { get; init; }
	public string? Text { get; init; }
	public string? Description { get; init; }
	public int? Order { get; init; }
	public CreateEnableWhenCommand? EnableWhen { get; init; }
	public ICollection<CreateAnswerOptionCommand>? AnswerOptions { get; init; } = new List<CreateAnswerOptionCommand>();
}

public record CreateEnableWhenCommand
{
	public Guid? DependentQuestionId { get; set; }
	public Operator? Operator { get; set; }
	public string? Answer { get; set; }
}

public record CreateAnswerOptionCommand
{
	public string? Name { get; init; }
	public string? Value { get; init; }
	public int? Order { get; init; }
}

public class CreateQuestionnaireCommandValidator : AbstractValidator<CreateQuestionnaireCommand>
{
	public CreateQuestionnaireCommandValidator()
	{
		RuleFor(command => command.Title)
			.NotEmpty();

		RuleFor(command => command.Questions.Count)
			.GreaterThan(0);

		RuleForEach(command => command.Questions)
			.SetValidator(new CreateQuestionCommandValidator());

		RuleForEach(command => command.Questions)
			.Must((command, question, _) => question.EnableWhen is null || command.Questions.FirstOrDefault(q => q.Id.Equals(question.EnableWhen.DependentQuestionId)) is not null)
			.WithMessage($"Not all QuestionsIds from {nameof(CreateQuestionCommand.EnableWhen)} are in {nameof(CreateQuestionnaireCommand.Questions)}");

		RuleForEach(command => command.Questions)
			.Must(BeOrdered)
			.WithMessage("Must have a correct order");
	}

	private static bool BeOrdered(CreateQuestionnaireCommand command, CreateQuestionCommand createQuestionCommand, ValidationContext<CreateQuestionnaireCommand> validationContext)
	{
		var i = 0;
		foreach (var question in command.Questions.OrderBy(question => question.Order))
		{
			if (question.Order is null || question.Order.Value != i)
				return false;

			i++;
		}

		return true;
	}
}

public class CreateQuestionCommandValidator : AbstractValidator<CreateQuestionCommand>
{
	public CreateQuestionCommandValidator()
	{
		RuleFor(question => question.Type)
			.NotNull()
			.IsInEnum();

		RuleFor(question => question.Text)
			.NotEmpty();

		RuleFor(question => question.Order)
			.NotNull();

		When(question => question.EnableWhen is not null, () =>
		{
			RuleFor(command => command.EnableWhen!.DependentQuestionId)
				.NotNull();

			RuleFor(command => command.EnableWhen!.Operator)
				.NotNull();

			RuleFor(command => command.EnableWhen!.Answer)
				.NotNull();

			When(command => command.EnableWhen!.Operator is Operator.GreaterThan or Operator.LessThan or Operator.GreaterOrEquals or Operator.LessOrEquals, () =>
				RuleFor(command => command.EnableWhen!.Answer).Must(answer => int.TryParse(answer, out _)));
		});

		RuleForEach(command => command.AnswerOptions)
			.SetValidator(new CreateAnswerOptionCommandValidator());
	}
}

public class CreateAnswerOptionCommandValidator : AbstractValidator<CreateAnswerOptionCommand>
{
	public CreateAnswerOptionCommandValidator()
	{
		RuleFor(command => command.Name)
			.NotEmpty();

		RuleFor(command => command.Value)
			.NotEmpty();

		RuleFor(command => command.Order)
			.NotNull();
	}
}

public class CreateQuestionnaireCommandHandler : IRequestHandler<CreateQuestionnaireCommand, QuestionnaireDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly IQuestionnaireDao _dao;

	public CreateQuestionnaireCommandHandler(IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, IQuestionnaireDao dao)
	{
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<QuestionnaireDto> Handle(CreateQuestionnaireCommand request, CancellationToken cancellationToken)
	{

		
		var questionnaire = new Questionnaire
        {
            Title = request.Title!,
            Description = request.Description,
            Questions = request.Questions.Select(question => new Question
	            {
		            Id = question.Id ?? Guid.NewGuid(),
		            Type = question.Type!.Value,
		            Text = question.Text!,
		            Description = question.Description,
		            Order = question.Order!.Value,
		            EnableWhen = question.EnableWhen is null
			            ? null
			            : new EnableWhen
			            {
				            DependentQuestionId = question.EnableWhen.DependentQuestionId!.Value,
				            Operator = question.EnableWhen.Operator!.Value,
				            Answer = question.EnableWhen.Answer!
			            },
		            AnswerOptions = question.AnswerOptions!.Select(answerOption => new AnswerOption
		            {
						Name = answerOption.Name!,
			            Value = answerOption.Value!,
			            Order = answerOption.Order!.Value,
		            }).ToList()
	            })
	            .ToList()
		};

		// FHIR query
		if (_fhirOptions.Write == true) {
			await _dao.Insert(questionnaire);
		}

		_context.Questionnaires.Add(questionnaire);
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<QuestionnaireDto>(questionnaire);
	}
}
