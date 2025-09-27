using Contracts.Authentication;

namespace Contracts.Responses;

public class RefreshTokenResponse
{
    public bool IsSuccess { get; set; }
    public string Message { get; set; }
    public TokenModel  Token { get; set; }
}