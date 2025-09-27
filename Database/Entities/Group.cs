using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Enums;

namespace Database.Entities;

public class Group
{
    [Key]
    public Guid GroupId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(50)]
    [Column(TypeName = "NVARCHAR(50)")]
    public string GroupName { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly Time { get; set; }
    public GroupSchedule Schedule { get; set; } = GroupSchedule.EveryWeek;
    public DateTime? GroupDate { get; set; }
    public Guid CoachId { get; set; }
}