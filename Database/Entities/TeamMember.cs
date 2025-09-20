using System.ComponentModel.DataAnnotations;
using Contracts.Enums;

namespace Database.Entities;

public class TeamMember
{
    [Key]
    public Guid TeamMemberId { get; set; } = Guid.NewGuid();
    public Guid? GroupInstanceId { get; set; }
    public Guid? MemberId { get; set; }
    public User? Member { get; set; }
    public MemberStatus MemberStatus { get; set; }
}