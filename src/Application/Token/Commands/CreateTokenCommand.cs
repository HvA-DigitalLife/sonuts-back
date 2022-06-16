using FluentValidation;
using MediatR;
using Sonuts.Application.Common.Interfaces;
using Sonuts.Application.Token.Models;

namespace Sonuts.Application.Token.Commands;

public class CreateTokenCommand : IRequest<TokenVm>
{
	public string? Email { get; init; }
}

public class CreateTokenCommandValidator : AbstractValidator<CreateTokenCommand>
{
	public CreateTokenCommandValidator()
	{
		RuleFor(query => query.Email)
			.Cascade(CascadeMode.Stop)
			.NotNull()
			.EmailAddress();
	}
}

public class CCreateTokenCommandHandler : IRequestHandler<CreateTokenCommand, TokenVm>
{
	private readonly IIdentityService _identityService;

	public CCreateTokenCommandHandler(IIdentityService identityService)
	{
		_identityService = identityService;
	}

	public async Task<TokenVm> Handle(CreateTokenCommand request, CancellationToken cancellationToken)
	{
		var userId = await _identityService.GetIdAsync(request.Email!);

		return new TokenVm
		{
			AccessToken = await _identityService.CreateAccessTokenAsync(userId),
			TokenType = "Bearer",
			ExpiresIn = 604800000,
			RefreshToken = _identityService.CreateRefreshTokenAsync(userId),
			Roles = await _identityService.GetRolesAsync(userId)
		};
	}
}
