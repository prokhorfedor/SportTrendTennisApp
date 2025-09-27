using Contracts.Enums;

namespace Contracts.Requests;

public class UpdateGroupStatusRequest
{
    public GroupStatus NewGroupStatus { get; set; }
    public GetGroupInstanceRequest GroupInstanceInfo { get; set; }
}