using System.ComponentModel.DataAnnotations;
using Contracts.Enums;

namespace Database.Entities;

public class Role
{
    [Key]
    public Guid RoleId { get; set; }
    public RoleName RoleName { get; set; }
    public DateTime CreatedDate { get; set; }
    public DateTime? ModifiedDate { get; set; }
}