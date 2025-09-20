using System.ComponentModel.DataAnnotations;
using Contracts.Enums;

namespace Database.Entities;

public class User
{
    [Key]
    public Guid UserId { get; set; } = Guid.NewGuid();
    public string FirstName { get; set; }
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserType UserType { get; set; }
}