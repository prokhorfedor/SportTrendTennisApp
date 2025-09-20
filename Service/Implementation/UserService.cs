using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Database.Contexts;
using Database.Entities;
using Service.Interfaces;

namespace Service.Implementation;

public class UserService : IUserService
{
    private readonly GroupManagementContext _context;

    public UserService(GroupManagementContext context)
    {
        _context = context;
    }

    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedDate = DateTime.Now,
                Email = request.Email,
                Phone = request.Phone,
                UserType = UserType.Member,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new CreateUserResponse()
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                Email = user.Email,
                Phone = user.Phone,
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}