namespace Sonuts.Application.Logic.Oauth2.Models;

// OAuth 2.0 token response specified in https://tools.ietf.org/html/rfc6749#section-5.1
public class TokenVm
{
	public required string AccessToken { get; set; }
	public required TokenType TokenType { get; set; }
	public int ExpiresIn { get; set; }
	public required string RefreshToken { get; set; }
	public IList<string> Scope { get; set; } = new List<string>();
	public IList<string> Roles { get; set; } = new List<string>();
}
