using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts;

public class GroupManagementContext : DbContext
{
    public DbSet<User> Users { get; set; }
    public DbSet<Group> Groups { get; set; }
    public DbSet<GroupInstance> GroupInstances { get; set; }
    public DbSet<Schedule> Schedules { get; set; }
    public DbSet<TeamMember> TeamMembers { get; set; }

    public GroupManagementContext(DbContextOptions<GroupManagementContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<User>().Property(x => x.UserType).HasConversion<int>();
        modelBuilder.Entity<Group>().HasOne<Schedule>(x => x.GroupSchedule);
        modelBuilder.Entity<Group>().HasMany<GroupInstance>().WithOne(gi => gi.Group).HasForeignKey(x => x.GroupId);
        modelBuilder.Entity<GroupInstance>().HasMany<TeamMember>(g => g.Team).WithOne()
            .HasForeignKey(t => t.GroupInstanceId);
        modelBuilder.Entity<TeamMember>().HasOne<User>(x => x.Member).WithOne()
            .HasForeignKey<TeamMember>(t => t.MemberId);
        modelBuilder.Entity<Schedule>().Property(x => x.DayOfWeek).HasConversion<int>();
        modelBuilder.Entity<TeamMember>().Property(x => x.MemberStatus).HasConversion<int>();
        base.OnModelCreating(modelBuilder);
    }
}