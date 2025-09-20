namespace Contracts.Requests;

public class GetGroupInstanceRequest
{
    public Guid GroupId { get; set; }
    public DateTime GroupInstanceDate { get; set; }
}