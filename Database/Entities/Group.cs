using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class Group
{
    [Key]
    public Guid GroupId { get; set; } = Guid.NewGuid();

    [Required]
    [StringLength(50)]
    [Column(TypeName = "NVARCHAR(50)")]
    public string GroupName { get; set; }
    public Schedule? GroupSchedule { get; set; }
    public Guid CoachId { get; set; }
}