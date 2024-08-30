using AutoMapper;
using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Common.Models;
using Sonuts.Application.Dtos;
using Sonuts.Domain.Entities;
using ValidationException = Sonuts.Application.Common.Exceptions.ValidationException;

namespace Sonuts.Application.Logic.Participants.Commands;

public record UpdatePasswordForParticipantCommand : IRequest<ChangePasswordDto>
{
	public string? Email { get; set; }
	public string? OldPassword { get; set; }
	public string? NewPassword { get; set; }
}

public class UpdatePasswordCommandValidator : AbstractValidator<UpdatePasswordForParticipantCommand>
{
	public UpdatePasswordCommandValidator(IIdentityService identityService)
	{
		RuleFor(query => query.Email)
			.NotNull()
			.EmailAddress();
		//.MustAsync(async (email, _) => await identityService.GetIdAsync(email) != null).WithMessage("Email does not exist.");

		RuleFor(query => query.OldPassword)
			.NotEmpty()
			.MinimumLength(8);

		RuleFor(query => query.NewPassword)
			.NotEmpty()
			.MinimumLength(8);
	}
}

public class UpdatePasswordForParticipantQueryHandler : IRequestHandler<UpdatePasswordForParticipantCommand, ChangePasswordDto>
{
	private readonly IApplicationDbContext _context;
	private readonly IIdentityService _identityService;
	private readonly IMapper _mapper;

	public UpdatePasswordForParticipantQueryHandler(IApplicationDbContext context, IIdentityService identityService, IMapper mapper)
	{
		_context = context;
		_identityService = identityService;
		_mapper = mapper;
	}

	public async Task<ChangePasswordDto> Handle(UpdatePasswordForParticipantCommand request, CancellationToken cancellationToken)
	{
		(Result result, string userId) = await _identityService.UpdatePasswordAsync(request.Email!, request.OldPassword!, request.NewPassword!);

		Console.WriteLine("user id: " + userId);
		if (!result.Succeeded)
			throw new ValidationException();

		var entity = new ChangePassword
		{
			Email = request.Email,
			OldPassword = request.OldPassword,
			NewPassword = request.NewPassword
		};
		return _mapper.Map<ChangePasswordDto>(entity);
	}

	
}

