using System.ComponentModel.DataAnnotations;

namespace Database.Entities;

public class Schedule
{
    [Key]
    public Guid ScheduleId { get; set; } = Guid.NewGuid();
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly Time { get; set; }
}