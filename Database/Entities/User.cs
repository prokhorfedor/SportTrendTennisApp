using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Contracts.Enums;

namespace Database.Entities;

public class User
{
    [Key, DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public Guid UserId { get; set; }
    [Column(TypeName = "NVARCHAR(50)")]
    public string FirstName { get; set; }
    [Column(TypeName = "NVARCHAR(50)")]
    public string LastName { get; set; }
    public string Email { get; set; }
    public string Phone { get; set; }
    [Column(TypeName = "VARBINARY(250)")]
    public byte[] PasswordHash { get; set; }
    public byte[] PasswordSalt { get; set; }
    public bool IsDeleted { get; set; }
    public DateTime CreatedDate { get; set; }
    public UserType UserType { get; set; }
}