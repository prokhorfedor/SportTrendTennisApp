using Contracts.Enums;

namespace Contracts.Requests;

public class CreateGroupRequest
{
    public string GroupName { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public GroupSchedule Schedule { get; set; } = GroupSchedule.EveryWeek;
    public DateTime? GroupDate { get; set; }
    public TimeOnly Time { get; set; }
}