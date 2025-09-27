using Contracts.Requests;
using Contracts.Responses;

namespace Service.Interfaces;

public interface IUserService
{
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
    Task<LoginResponse> LoginAsync(LoginRequest request);
    Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request);
}