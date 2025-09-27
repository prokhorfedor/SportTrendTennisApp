using Contracts.DTOs;

namespace Contracts.Responses;

public class GroupInstanceResponse
{
    public bool Success { get; set; }
    public string Message { get; set; }
    public GroupTimeDto GroupInfo { get; set; }
    public List<TeamMemberDto> Members { get; set; }
}