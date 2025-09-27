using Contracts.DTOs;
using Contracts.Enums;
using Contracts.Requests;
using Contracts.Responses;
using Database.Contexts;
using Database.Entities;
using Microsoft.EntityFrameworkCore;
using Service.Interfaces;

namespace Service.Implementation;

public class GroupManagementService : IGroupManagementService
{
    private readonly IGroupManagementContext _context;

    public GroupManagementService(IGroupManagementContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Get groups with schedule
    /// </summary>
    /// <returns></returns>
    public async Task<GroupScheduleResponse> GetGroupsWithScheduleAsync()
    {
        try
        {
            var groups = await _context.Groups.AsNoTracking().ToListAsync();

            var startOfWeek = DateTime.Today.AddDays(-(int)DateTime.Today.DayOfWeek).Date;
            var groupIds = groups.Select(s => s.GroupId).ToList();
            var groupStatuses = (await _context.GroupInstances.AsNoTracking()
                    .Where(g => groupIds.Contains(g.GroupId) && !g.IsDeleted && g.GroupInstanceDate >= startOfWeek && g.GroupInstanceDate <= startOfWeek.AddDays(6))
                    .Select(g => new { g.GroupId, g.GroupStatus }).ToListAsync())
                .ToDictionary(g => g.GroupId, g => g.GroupStatus);

            var groupedSchedule = new GroupScheduleResponse()
            {
                GroupScheduleItem = groups.GroupBy(s => s.DayOfWeek, s =>
                    new GroupTimeDto()
                    {
                        Status = groupStatuses.ContainsKey(s.GroupId) ? groupStatuses[s.GroupId] : GroupStatus.Pending,
                        GroupId = s.GroupId,
                        Time = s.Time,
                        GroupName = s.GroupName
                    }).ToDictionary(s => s.Key, s => s.ToList())
            };

        return groupedSchedule;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    /// <summary>
    /// Register for a group
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<RegisterToGroupResponse> RegisterIntoGroupAsync(RegisterToGroupRequest request, Guid userId)
    {
        try
        {
            if (!_context.Groups.Any(g => g.GroupId == request.GroupId))
            {
                return new RegisterToGroupResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "Group does not exist",
                };
            }

            var groupInstance = await _context.GroupInstances
                .Include(g => g.Team)
                .Where(g => !g.IsDeleted && g.GroupId == request.GroupId && g.GroupInstanceDate == request.GroupInstanceDate.Date)
                .SingleOrDefaultAsync();

            if (groupInstance != null && groupInstance.GroupStatus != GroupStatus.Pending)
            {
                return new RegisterToGroupResponse()
                {
                    IsSuccess = false,
                    ErrorMessage = "Registration has already been closed",
                    GroupInstanceId = groupInstance.GroupInstanceId,
                };
            }

            if (groupInstance == null)
            {
                groupInstance = new GroupInstance()
                {
                    GroupId = request.GroupId,
                    GroupInstanceDate = request.GroupInstanceDate.Date
                };
                await _context.GroupInstances.AddAsync(groupInstance);

            }
            var teamMember = new TeamMember()
            {
                MemberId = userId,
                MemberStatus = MemberStatus.Pending,
                GroupInstanceId = groupInstance.GroupInstanceId
            };
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return new RegisterToGroupResponse()
            {
                IsSuccess = true,
                GroupInstanceId = groupInstance.GroupInstanceId
            };
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<GroupInstanceResponse> GetGroupInstanceAsync(GetGroupInstanceRequest request)
    {
        try
        {
            var groupInstance = await _context.GroupInstances.AsNoTracking()
                .Include(g => g.Group)
                .Include(g => g.Team)!.ThenInclude(t => t.Member)
                .Where(g => !g.IsDeleted && g.GroupId == request.GroupId && g.GroupInstanceDate == request.GroupInstanceDate.Date)
                .SingleOrDefaultAsync();
            return GetGroupInstanceResponse(groupInstance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<GroupInstanceResponse> GetGroupInstanceByIdAsync(Guid groupInstanceId)
    {
        try
        {
            var groupInstance = await _context.GroupInstances.AsNoTracking()
                .Include(g => g.Group)
                .Include(g => g.Team)!.ThenInclude(t => t.Member)
                .Where(g => !g.IsDeleted && g.GroupInstanceId == groupInstanceId)
                .SingleOrDefaultAsync();
            return GetGroupInstanceResponse(groupInstance);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    public async Task<Guid> CreateGroupAsync(CreateGroupRequest request, Guid userId)
    {
        try
        {
            var group = new Group()
            {
                CoachId = userId,
                Schedule = request.Schedule,
                GroupDate = request.GroupDate,
                GroupName = request.GroupName,
                DayOfWeek = request.DayOfWeek,
                Time = request.Time,
            };
            await _context.Groups.AddAsync(group);
            await _context.SaveChangesAsync();
            return group.GroupId;
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    private GroupInstanceResponse GetGroupInstanceResponse(GroupInstance? groupInstance)
    {
        if (groupInstance == null)
        {
            return new GroupInstanceResponse()
            {
                Success = false,
                Message = "Group Instance Not Found",
            };
        }

        var response = new GroupInstanceResponse()
        {
            Success = true,
            GroupInfo = new GroupTimeDto()
            {
                GroupId = groupInstance.GroupId,
                GroupName = groupInstance.Group.GroupName,
                Time = groupInstance.Group.Time,
                Status = groupInstance.GroupStatus
            },
            Members = groupInstance.Team.Select(s => new TeamMemberDto()
            {
                TeamMemberId = s.TeamMemberId,
                MemberStatus = s.MemberStatus,
                FirstName = s.Member.FirstName,
                LastName = s.Member.LastName,
            }).ToList(),
        };
        return response;
    }

    public async Task<GroupInstanceResponse> UpdateGroupInstanceStatusAsync(UpdateGroupStatusRequest request)
    {
        try
        {
            var groupInfo = request.GroupInstanceInfo;
            var groupInstance = await _context.GroupInstances
                .Include(g => g.Group)
                .Include(g => g.Team)
                .Where(g => g.GroupId == groupInfo.GroupId && g.GroupInstanceDate == groupInfo.GroupInstanceDate.Date)
                .SingleOrDefaultAsync();
            if (groupInstance == null && request.NewGroupStatus != GroupStatus.Canceled)
            {
                return new GroupInstanceResponse()
                {
                    Success = false,
                    Message = "Group Instance Not Found",
                };
            }

            if (groupInstance == null && request.NewGroupStatus == GroupStatus.Canceled)
            {
                groupInstance = new GroupInstance()
                {
                    GroupId = groupInfo.GroupId,
                    GroupInstanceDate = groupInfo.GroupInstanceDate.Date,
                    GroupStatus = GroupStatus.Canceled,
                };
                await _context.GroupInstances.AddAsync(groupInstance);
                await _context.SaveChangesAsync();
                return await this.GetGroupInstanceByIdAsync(groupInstance.GroupInstanceId);
            }

            groupInstance!.IsDeleted = true;
            var updatedGroupInstance = new GroupInstance()
            {
                GroupId = groupInstance.GroupId,
                GroupInstanceDate = groupInstance.GroupInstanceDate.Date,
                GroupStatus = request.NewGroupStatus,
            };
            await _context.GroupInstances.AddAsync(updatedGroupInstance);

            if (groupInstance.Team != null && groupInstance.Team.Any())
            {
                var newMemberStatus = updatedGroupInstance.GroupStatus switch
                {
                    GroupStatus.Pending => MemberStatus.Pending,
                    GroupStatus.Canceled => MemberStatus.Declined,
                    GroupStatus.Approved => MemberStatus.Approved,
                    _ => throw new ArgumentOutOfRangeException()
                };
                var teamMembers = groupInstance.Team.Select(s => new TeamMember()
                {
                    MemberStatus = newMemberStatus,
                    GroupInstanceId = updatedGroupInstance.GroupInstanceId,
                    MemberId = s.MemberId,
                }).ToList();
                await _context.TeamMembers.AddRangeAsync(teamMembers);
                updatedGroupInstance.Team = teamMembers;
            }
            await _context.SaveChangesAsync();
            return await this.GetGroupInstanceByIdAsync(updatedGroupInstance.GroupInstanceId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}