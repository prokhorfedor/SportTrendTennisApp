namespace Contracts.Requests;

public class CreateGroupRequest
{
    public string GroupName { get; set; }
    public DayOfWeek DayOfWeek { get; set; }
    public TimeOnly Time { get; set; }
}