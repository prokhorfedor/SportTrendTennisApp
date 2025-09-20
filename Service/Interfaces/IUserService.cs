using Contracts.Requests;
using Contracts.Responses;

namespace Service.Interfaces;

public interface IUserService
{
    Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request);
}