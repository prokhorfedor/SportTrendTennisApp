using Contracts.Enums;

namespace Contracts.DTOs;

public class GroupTimeDto
{
    public Guid GroupId { get; set; }
    public GroupStatus Status { get; set; }
    public TimeOnly Time { get; set; }
    public string GroupName { get; set; }
}