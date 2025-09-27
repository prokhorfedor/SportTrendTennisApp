using Database.Entities;
using Microsoft.EntityFrameworkCore;

namespace Database.Contexts;

public interface IGroupManagementContext
{
     DbSet<Group> Groups { get; set; }
     DbSet<GroupInstance> GroupInstances { get; set; }
     DbSet<TeamMember> TeamMembers { get; set; }
     Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);
}