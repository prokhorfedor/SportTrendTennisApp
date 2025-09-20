using System.ComponentModel.DataAnnotations;

namespace Contracts.Requests;

public class LoginRequest
{
    /// <summary>
    /// Email or Phone
    /// </summary>
    public string Identifier  { get; set; }

    /// <summary>
    /// Hashed password
    /// </summary>
    public string PasswordHash { get; set; }
}