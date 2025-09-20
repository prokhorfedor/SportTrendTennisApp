namespace Contracts.Requests;

public class RegisterToGroupRequest
{
    public  Guid GroupId { get; set; }
    public Guid UserId { get; set; }
    public DateTime GroupInstanceDate { get; set; }
}