using Contracts.Requests;
using Contracts.Responses;

namespace Service.Interfaces;

public interface IGroupManagementService
{
    Task<GroupScheduleResponse> GetGroupsWithScheduleAsync();
    Task<Guid> RegisterIntoGroupAsync(RegisterToGroupRequest request);
    Task<GroupInstanceResponse> GetGroupInstanceAsync(GetGroupInstanceRequest request);
    Task<GroupInstanceResponse> GetGroupInstanceByIdAsync(Guid groupInstanceId);
}