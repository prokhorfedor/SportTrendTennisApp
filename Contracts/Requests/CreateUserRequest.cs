using System.ComponentModel.DataAnnotations;

namespace Contracts.Requests;

public class CreateUserRequest
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
    [EmailAddress]
    public string Email { get; set; }
    [Phone]
    public string Phone { get; set; }
}