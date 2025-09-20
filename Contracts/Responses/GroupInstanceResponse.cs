using Contracts.DTOs;

namespace Contracts.Responses;

public class GroupInstanceResponse
{
    public GroupTimeDto GroupInfo { get; set; }
    public List<TeamMemberDto> Members { get; set; }
}