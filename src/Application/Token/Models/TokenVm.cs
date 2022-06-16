namespace Sonuts.Application.Token.Models;

public record TokenVm
{
	public string AccessToken { get; init; } = default!;
	public string TokenType { get; init; } = default!;
	public int ExpiresIn { get; init; } = default!;
	public string RefreshToken { get; init; } = default!;
	public IList<string> Roles { get; init; } = default!;
	public IList<string> Scope { get; init; } = default!;
}
