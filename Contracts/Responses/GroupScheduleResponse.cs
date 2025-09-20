using Contracts.DTOs;

namespace Contracts.Responses;

public class GroupScheduleResponse
{
    public Dictionary<DayOfWeek, List<GroupTimeDto>> GroupScheduleItem { get; set; }
}

