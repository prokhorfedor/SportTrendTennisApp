using System.Security.Cryptography;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementation;

public class UserService : IUserService
{
    // Налаштування безпеки
    private const int SaltSize = 16;   // 128 біт
    private const int KeySize = 32;    // 256 біт
    private const int Iterations = 100_000; // кількість ітерацій (чим більше, тим безпечніше, але повільніше)
    private readonly GroupManagementContext _context;

    public UserService(GroupManagementContext context)
    {
        _context = context;
    }

    public async Task<CreateUserResponse> CreateUserAsync(CreateUserRequest request)
    {
        try
        {
            var (password, salt) = this.HashPassword(request.PasswordHash);
            var user = new User()
            {
                FirstName = request.FirstName,
                LastName = request.LastName,
                CreatedDate = DateTime.Now,
                Email = request.Email.ToLower(),
                Phone = request.Phone,
                UserType = UserType.Member,
                PasswordHash = password,
                PasswordSalt = salt,
            };
            await _context.Users.AddAsync(user);
            await _context.SaveChangesAsync();
            return new CreateUserResponse()
            {
                UserId = user.UserId,
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

    public async Task<LoginResponse> LoginAsync(LoginRequest request)
    {
        try
        {
            var user = await _context.Users
                .SingleOrDefaultAsync(u => u.Email == request.Identifier.ToLower() || u.Phone == request.Identifier);

            if (user == null)
            {
                return new LoginResponse()
                {
                    Result = LoginResult.NotFound
                };
            }
            // var (password, salt) = this.HashPassword(request.PasswordHash);
            // user.PasswordHash = password;
            // user.PasswordSalt = salt;
            // await _context.SaveChangesAsync();
            if (!this.VerifyPassword(request.PasswordHash, Convert.ToBase64String(user.PasswordHash),
                    Convert.ToBase64String(user.PasswordSalt)))
            {
                return new LoginResponse()
                {
                    Result = LoginResult.WrongPassword
                };
            }

            return new LoginResponse()
            {
                Result = LoginResult.Success
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private (byte[] hash, byte[] salt) HashPassword(string password)
    {
        // Генеруємо випадкову сіль
        byte[] saltBytes = RandomNumberGenerator.GetBytes(SaltSize);

        // Хешуємо пароль + сіль через PBKDF2
        var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] key = pbkdf2.GetBytes(KeySize);

        return (key, saltBytes);
    }

    private bool VerifyPassword(string password, string storedHash, string storedSalt)
    {
        byte[] saltBytes = Convert.FromBase64String(storedSalt);

        var pbkdf2 = new Rfc2898DeriveBytes(password, saltBytes, Iterations, HashAlgorithmName.SHA256);
        byte[] key = pbkdf2.GetBytes(KeySize);
        string computedHash = Convert.ToBase64String(key);

        // Порівнюємо хеші
        return computedHash == storedHash;
    }
}