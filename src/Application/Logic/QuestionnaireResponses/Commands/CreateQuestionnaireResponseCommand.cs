using AutoMapper;
using FluentValidation;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Sonuts.Application.Common.Exceptions;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using static System.Decimal;
using QuestionType = Sonuts.Domain.Enums.QuestionType;

namespace Sonuts.Application.Logic.QuestionnaireResponses.Commands;

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
			validator.RuleFor(response => response)
				.NotNull();

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
		var question = await _context.Questions
			               .Include(question => question.AnswerOptions)
			               .FirstOrDefaultAsync(question => question.Id.Equals(response.QuestionId!.Value)) ??
		               throw new NotFoundException(nameof(Question), response.QuestionId!.Value);

		return question.Type switch
		{
			QuestionType.Boolean => !string.IsNullOrWhiteSpace(response.Answer) && (response.Answer.Equals("Yes") || response.Answer.Equals("No")),
			QuestionType.String => !string.IsNullOrWhiteSpace(response.Answer),
			QuestionType.Integer => int.TryParse(response.Answer, out int integerAnswer) && integerAnswer >= 0,
			QuestionType.Decimal => TryParse(response.Answer, out decimal decimalAnswer) && decimalAnswer >= Zero,
			QuestionType.Choice => question.AnswerOptions?.FirstOrDefault(option => option.Value.ToLower().Equals(response.Answer!.ToLower())) != null,
			QuestionType.OpenChoice => !string.IsNullOrWhiteSpace(response.Answer),
			QuestionType.MultiChoice => true, //TODO
			QuestionType.MultiOpenChoice => true, //TODO
			_ => false
		};
	}
}

public class CreateQuestionnaireResponseCommandHandler : IRequestHandler<CreateQuestionnaireResponseCommand, QuestionnaireResponseDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly ICurrentUserService _currentUserService;
	private readonly IFhirOptions _fhirOptions;
	private readonly IQuestionnaireResponseDao _dao;

	public CreateQuestionnaireResponseCommandHandler(IApplicationDbContext context, IMapper mapper, ICurrentUserService currentUserService, IFhirOptions fhirOptions, IQuestionnaireResponseDao dao)
	{
		_context = context;
		_mapper = mapper;
		_currentUserService = currentUserService;
		_fhirOptions = fhirOptions;
		_dao = dao;
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
				Question = _context.Questions.FirstOrDefault(question => question.Id.Equals(response.QuestionId)) ??
				           throw new NotFoundException(nameof(Question), response.QuestionId!),
				Answer = response.Answer!
			}).ToList()
		};

		// FHIR query
		if (_fhirOptions.Write == true) {
			await _dao.Insert(entity);
		}		

		await _context.QuestionnaireResponses.AddAsync(entity, cancellationToken);
		
		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<QuestionnaireResponseDto>(entity);
	}
}
