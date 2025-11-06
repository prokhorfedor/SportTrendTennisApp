using Contracts.Authentication;

namespace Contracts.Responses;

public class RefreshTokenResponse : ResponseBase
{
    public TokenModel  Token { get; set; }
}