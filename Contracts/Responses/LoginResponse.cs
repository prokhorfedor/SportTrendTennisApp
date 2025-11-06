using Contracts.Authentication;
using Contracts.Enums;

namespace Contracts.Responses;

public class LoginResponse : ResponseBase
{
    public LoginResult Result { get; set; }
    public TokenModel  Token { get; set; }
}