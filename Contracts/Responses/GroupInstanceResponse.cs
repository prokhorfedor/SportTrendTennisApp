using Contracts.DTOs;

namespace Contracts.Responses;

public class GroupInstanceResponse : ResponseBase
{
    public GroupTimeDto GroupInfo { get; set; }
    public List<TeamMemberDto> Members { get; set; }
}