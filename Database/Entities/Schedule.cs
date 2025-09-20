using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Database.Entities;

public class Schedule
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid ScheduleId { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly Time { get; set; }
}