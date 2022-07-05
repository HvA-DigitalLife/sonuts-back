namespace Sonuts.Application.Oauth2.Models;

// OAuth 2.0 grant types specified by https://tools.ietf.org/html/rfc6749
public enum GrantType
{
    Password,
    AuthorizationCode,
    Implicit,
    ClientCredentials,
    RefreshToken
}
