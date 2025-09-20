using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class Group
{
    [Key]
    public Guid GroupId { get; set; } = Guid.NewGuid();
    public string GroupName { get; set; }
    public Schedule? GroupSchedule { get; set; }
    public Guid CoachId { get; set; }
}