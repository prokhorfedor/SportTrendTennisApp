using Contracts.DTOs;
using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts;

public class ApplicationContext : DbContext, IUserContext, IGroupManagementContext
{
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupInstance> GroupInstances { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }

    public ApplicationContext(DbContextOptions<ApplicationContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Group>().HasMany<GroupInstance>().WithOne(gi => gi.Group).HasForeignKey(x => x.GroupId);
        modelBuilder.Entity<Group>().Property(x=>x.DayOfWeek).HasConversion<int>();
        modelBuilder.Entity<GroupInstance>().HasMany<TeamMember>(g => g.Team).WithOne()
            .HasForeignKey(t => t.GroupInstanceId);
        modelBuilder.Entity<TeamMember>().Property(x => x.MemberStatus).HasConversion<int>();
        modelBuilder.Entity<User>().Property(x => x.UserId).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<TeamMember>().HasOne<User>(x => x.Member).WithOne()
            .HasForeignKey<TeamMember>(t => t.MemberId);
        modelBuilder.Entity<User>().Property(x => x.RoleName).HasConversion<int>();
        modelBuilder.Entity<Role>().Property(x => x.RoleName).HasConversion<int>();
        modelBuilder.Entity<Role>().HasIndex(x => x.RoleName).IsUnique();
        modelBuilder.Entity<User>().HasMany<RefreshToken>().WithOne().HasForeignKey(x => x.UserId);
        modelBuilder.Entity<User>().HasMany<Role>(r => r.Roles).WithMany();
        modelBuilder.Entity<Group>().Property(x => x.GroupId).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<GroupInstance>().Property(x => x.GroupInstanceId).HasDefaultValueSql("NEWID()");
        modelBuilder.Entity<TeamMember>().Property(x => x.TeamMemberId).HasDefaultValueSql("NEWID()");
        base.OnModelCreating(modelBuilder);
    }
}