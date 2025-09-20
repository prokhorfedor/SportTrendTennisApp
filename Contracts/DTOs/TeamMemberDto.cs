using Contracts.Enums;

namespace Contracts.DTOs;

public class TeamMemberDto
{
    public Guid TeamMemberId { get; set; }
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public MemberStatus MemberStatus { get; set; }
}