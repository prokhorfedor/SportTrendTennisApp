using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts;

public interface IUserContext
{
    DbSet<RefreshToken> RefreshTokens { get; set; }
    DbSet<User> Users { get; set; }
    DbSet<Role> Roles { get; set; }

    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}