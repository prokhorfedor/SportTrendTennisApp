using Contracts.Authentication;

namespace Contracts.Requests;

public class RefreshTokenRequest
{
    public TokenModel  Token { get; set; }
}