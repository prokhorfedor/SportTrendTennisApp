using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;
using Contracts.Authentication;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Service.Interfaces;

namespace Service.Implementation;

public class UserService : IUserService
{
    // Налаштування безпеки
    private const int SaltSize = 16; // 128 біт
    private const int KeySize = 32; // 256 біт
    private const int Iterations = 100_000; // кількість ітерацій (чим більше, тим безпечніше, але повільніше)
    private readonly IUserContext _context;
    private readonly ServiceConfiguration _appSettings;
    private readonly TokenValidationParameters _tokenValidationParameters;

    public UserService(IUserContext context, IOptions<ServiceConfiguration> appSettings, TokenValidationParameters tokenValidationParameters)
    {
        _context = context;
        _appSettings = appSettings.Value;
        _tokenValidationParameters = tokenValidationParameters;
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
                RoleName = RoleName.Member,
                PasswordHash = password,
                PasswordSalt = salt,
            };
            var role = await _context.Roles.SingleAsync(r => r.RoleName == RoleName.Member);
            user.Roles = new List<Role>() { role };
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
            LoginResponse response = new LoginResponse();
            var user = await _context.Users.Include(u => u.Roles)
                .SingleOrDefaultAsync(u => u.Email == request.Identifier.ToLower() || u.Phone == request.Identifier);

            if (user == null)
            {
                response.Result = LoginResult.NotFound;
                return response;
            }

            // var (password, salt) = this.HashPassword(request.PasswordHash);
            // user.PasswordHash = password;
            // user.PasswordSalt = salt;
            // await _context.SaveChangesAsync();
            if (!this.VerifyPassword(request.PasswordHash, Convert.ToBase64String(user.PasswordHash),
                    Convert.ToBase64String(user.PasswordSalt)))
            {
                response.Result = LoginResult.WrongPassword;
                return response;
            }

            var authenticationResult = await AuthenticateAsync(user);
            if (authenticationResult.Success)
            {
                response.Result = LoginResult.Success;
                response.Token = new TokenModel()
                    { Token = authenticationResult.Token, RefreshToken = authenticationResult.RefreshToken };
            }
            else
            {
                response.Result = LoginResult.GeneralError;
            }

            return response;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<RefreshTokenResponse> RefreshTokenAsync(RefreshTokenRequest request)
    {
        var response = new RefreshTokenResponse();
        var token = request.Token;
        try
        {
            var authResponse = await GetRefreshTokenAsync(token.Token, token.RefreshToken);
            if (!authResponse.Success)
            {
                response.IsSuccess = false;
                response.Message = string.Join(",", authResponse.Errors);
                return response;
            }

            TokenModel refreshTokenModel = new TokenModel();
            refreshTokenModel.Token = authResponse.Token;
            refreshTokenModel.RefreshToken = authResponse.RefreshToken;
            response.Token = refreshTokenModel;
            return response;
        }
        catch (Exception ex)
        {
            response.IsSuccess = false;
            response.Message = "Something went wrong!";
            return response;
        }
    }

    private async Task<AuthenticationResult> GetRefreshTokenAsync(string token, string refreshToken)
    {
        var validatedToken = await GetPrincipalFromToken(token);

        if (validatedToken == null)
        {
            return new AuthenticationResult { Errors = new[] { "Invalid Token" } };
        }

        var expiryDateUnix = long.Parse(validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Exp).Value);

        var expiryDateTimeUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc)
            .AddSeconds(expiryDateUnix);

        if (expiryDateTimeUtc > DateTime.UtcNow)
        {
            return new AuthenticationResult { Errors = new[] { "This token hasn't expired yet" } };
        }

        var jti = validatedToken.Claims.Single(x => x.Type == JwtRegisteredClaimNames.Jti).Value;

        var storedRefreshToken = await _context.RefreshTokens.FirstOrDefaultAsync(x => x.Token == refreshToken);

        if (storedRefreshToken == null)
        {
            return new AuthenticationResult { Errors = new[] { "This refresh token does not exist" } };
        }

        if (DateTime.UtcNow > storedRefreshToken.ExpiryDate)
        {
            return new AuthenticationResult { Errors = new[] { "This refresh token has expired" } };
        }

        if (storedRefreshToken.Used)
        {
            return new AuthenticationResult { Errors = new[] { "This refresh token has been used" } };
        }

        if (storedRefreshToken.JwtId != jti)
        {
            return new AuthenticationResult { Errors = new[] { "This refresh token does not match this JWT" } };
        }

        storedRefreshToken.Used = true;
        _context.RefreshTokens.Update(storedRefreshToken);
        await _context.SaveChangesAsync();
        var strUserId = validatedToken.Claims.SingleOrDefault(x => x.Type == "UserId")?.Value;
        if (!Guid.TryParse(strUserId, out Guid userId))
        {
            return new AuthenticationResult { Errors = new[] { "User Not Found" } };
        }

        var user = await _context.Users.Include(x=>x.Roles).FirstOrDefaultAsync(c => c.UserId == userId);
        if (user == null)
        {
            return new AuthenticationResult { Errors = new[] { "User Not Found" } };
        }

        return await AuthenticateAsync(user);
    }

    private async Task<ClaimsIdentity?> GetPrincipalFromToken(string token)
    {
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var tokenValidationParameters = _tokenValidationParameters.Clone();
            tokenValidationParameters.ValidateLifetime = false;
            var tokenValidationResult = await tokenHandler.ValidateTokenAsync(token, tokenValidationParameters);
            if (!IsJwtWithValidSecurityAlgorithm(tokenValidationResult.SecurityToken))
            {
                return null;
            }

            return tokenValidationResult.ClaimsIdentity;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            throw;
        }
    }

    private bool IsJwtWithValidSecurityAlgorithm(SecurityToken validatedToken)
    {
        return (validatedToken is JwtSecurityToken jwtSecurityToken) &&
               jwtSecurityToken.Header.Alg.Equals(SecurityAlgorithms.HmacSha256,
                   StringComparison.InvariantCultureIgnoreCase);
    }

        private async Task<AuthenticationResult> AuthenticateAsync(User user)
    {
        // authentication successful so generate jwt token
        AuthenticationResult authenticationResult = new AuthenticationResult();
        var tokenHandler = new JwtSecurityTokenHandler();

        try
        {
            var key = Encoding.ASCII.GetBytes(_appSettings.JwtSettings.Secret);

            ClaimsIdentity Subject = new ClaimsIdentity(new Claim[]
            {
                new Claim("UserId", user.UserId.ToString()),
                new Claim("FirstName", user.FirstName),
                new Claim("LastName", user.LastName),
                new Claim("Email", string.IsNullOrWhiteSpace(user.Email) ? "" : user.Email),
                new Claim("Phone", string.IsNullOrWhiteSpace(user.Phone) ? "" : user.Phone),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            });
            foreach (var role in user.Roles)
            {
                Subject.AddClaim(new Claim(ClaimTypes.Role, role.RoleName.ToString()));
            }

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = Subject,
                Expires = DateTime.UtcNow.Add(_appSettings.JwtSettings.TokenLifetime),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key),
                    SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            authenticationResult.Token = tokenHandler.WriteToken(token);
            var refreshToken = new RefreshToken
            {
                Token = Guid.NewGuid().ToString(),
                JwtId = token.Id,
                UserId = user.UserId,
                CreatedDate = DateTime.UtcNow,
                ExpiryDate = DateTime.UtcNow.AddMonths(6)
            };
            await _context.RefreshTokens.AddAsync(refreshToken);
            await _context.SaveChangesAsync();
            authenticationResult.RefreshToken = refreshToken.Token;
            authenticationResult.Success = true;
            return authenticationResult;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
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