namespace Contracts.Responses;

public class RegisterToGroupResponse
{
    public bool IsSuccess { get; set; }
    public string ErrorMessage { get; set; }
    public Guid GroupInstanceId { get; set; }
}