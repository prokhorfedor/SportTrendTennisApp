using Contracts.DTOs;

namespace Contracts.Responses;

public class GroupScheduleResponse : ResponseBase
{
    public Dictionary<DayOfWeek, List<GroupTimeDto>> GroupScheduleItem { get; set; }
}

