using Contracts.Requests;
using Contracts.Responses;

namespace Service.Interfaces;

public interface IGroupManagementService
{
    Task<GroupScheduleResponse> GetGroupsWithScheduleAsync();
    Task<RegisterToGroupResponse> RegisterIntoGroupAsync(RegisterToGroupRequest request, Guid userId);
    Task<GroupInstanceResponse> GetGroupInstanceAsync(GetGroupInstanceRequest request);
    Task<GroupInstanceResponse> GetGroupInstanceByIdAsync(Guid groupInstanceId);
    Task<Guid> CreateGroupAsync(CreateGroupRequest request, Guid userId);
    Task<GroupInstanceResponse> UpdateGroupInstanceStatusAsync(UpdateGroupStatusRequest request);
}