using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Interfaces.Fhir;
using Sonuts.Application.Common.Models;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using ValidationException = Sonuts.Application.Common.Exceptions.ValidationException;

namespace Sonuts.Application.Participants.Commands;

public record CreateParticipantCommand : IRequest<ParticipantDto>
{
	public string? Email { get; init; }
	public string? Password { get; init; }
	public string? FirstName { get; set; }
	public string? LastName { get; set; }
	public DateOnly? Birth { get; init; }
	public string? Gender { get; init; }
	public decimal? Weight { get; init; }
	public decimal? Height { get; init; }
	public string? MaritalStatus { get; init; }
}

public class CreateParticipantCommandValidator : AbstractValidator<CreateParticipantCommand>
{
	public CreateParticipantCommandValidator()
	{
		RuleFor(query => query.Email)
			.NotNull()
			.EmailAddress();

		RuleFor(query => query.Password)
			.NotEmpty()
			.MinimumLength(8);

		RuleFor(query => query.FirstName)
			.NotEmpty();

		RuleFor(query => query.LastName)
			.NotEmpty();
	}
}

public class CreateParticipantCommandHandler : IRequestHandler<CreateParticipantCommand, ParticipantDto>
{
	private readonly IIdentityService _identityService;
	private readonly IApplicationDbContext _context;
	private readonly IMapper _mapper;
	private readonly IFhirOptions _fhirOptions;
	private readonly IParticipantDao _dao;

	public CreateParticipantCommandHandler(IIdentityService identityService, IApplicationDbContext context, IMapper mapper, IFhirOptions fhirOptions, IParticipantDao dao)
	{
		_identityService = identityService;
		_context = context;
		_mapper = mapper;
		_fhirOptions = fhirOptions;
		_dao = dao;
	}

	public async Task<ParticipantDto> Handle(CreateParticipantCommand request, CancellationToken cancellationToken)
	{
		(Result result, string userId) = await _identityService.CreateUserAsync(request.Email!, request.Password!);

		if (!result.Succeeded)
			throw new ValidationException();

		var entity = new Participant
		{
			Id = Guid.Parse(userId),
			FirstName = request.FirstName!,
			LastName = request.LastName!,
			Birth = request.Birth,
			Gender = request.Gender,
			Weight = request.Weight,
			Height = request.Height,
			MaritalStatus = request.MaritalStatus,
			IsActive = true
		};
	
		// FHIR query	
		if (_fhirOptions.Write == true) {
			await _dao.Insert(entity);
		}	

		// ReSharper disable once MethodSupportsCancellation
		// Do not stop creating participant when user is already created
		await _context.Participants.AddAsync(entity);

		await _context.SaveChangesAsync(cancellationToken);

		return _mapper.Map<ParticipantDto>(entity);
	}
}
