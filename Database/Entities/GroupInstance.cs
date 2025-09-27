using System.ComponentModel.DataAnnotations;
using Contracts.Enums;

namespace Database.Entities;

public class GroupInstance
{
    [Key]
    public Guid GroupInstanceId { get; set; } = Guid.NewGuid();
    public DateTime GroupInstanceDate { get; set; }
    public Guid GroupId { get; set; }
    public Group? Group { get; set; }
    public bool IsDeleted { get; set; }
    public GroupStatus GroupStatus { get; set; } = GroupStatus.Pending;
    public List<TeamMember>? Team { get; set; }
}