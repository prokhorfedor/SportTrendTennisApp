using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class GroupInstance
{
    [Key]
    public Guid GroupInstanceId { get; set; } = Guid.NewGuid();
    public DateTime GroupInstanceDate { get; set; }
    public Guid? GroupId { get; set; }
    public Group? Group { get; set; }
    public List<TeamMember>? Team { get; set; }
}