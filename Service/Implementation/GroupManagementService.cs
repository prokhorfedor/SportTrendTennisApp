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
            var schedules = await _context.Groups.AsNoTracking().ToListAsync();
            var groupedSchedule = new GroupScheduleResponse()
            {
                GroupScheduleItem = schedules.GroupBy(s => s.DayOfWeek, s =>
                    new GroupTimeDto()
                    {
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
    public async Task<Guid> RegisterIntoGroupAsync(RegisterToGroupRequest request)
    {
        try
        {
            var groupInstance = await _context.GroupInstances
                .Include(g => g.Team)
                .Where(g => g.GroupId == request.GroupId && g.GroupInstanceDate == request.GroupInstanceDate.Date)
                .SingleOrDefaultAsync();

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
                MemberId = request.UserId,
                MemberStatus = MemberStatus.Pending,
                GroupInstanceId = groupInstance.GroupInstanceId
            };
            await _context.TeamMembers.AddAsync(teamMember);
            await _context.SaveChangesAsync();
            return groupInstance.GroupInstanceId;
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
            var groupInstance = await _context.GroupInstances
                .Include(g => g.Group)
                .Include(g => g.Team)!.ThenInclude(t => t.Member)
                .Where(g => g.GroupId == request.GroupId && g.GroupInstanceDate == request.GroupInstanceDate.Date)
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
            var groupInstance = await _context.GroupInstances
                .Include(g => g.Group)
                .Include(g => g.Team)!.ThenInclude(t => t.Member)
                .Where(g => g.GroupInstanceId == groupInstanceId)
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
            return new GroupInstanceResponse();
        }

        var response = new GroupInstanceResponse()
        {
            GroupInfo = new GroupTimeDto()
            {
                GroupId = groupInstance.GroupId.GetValueOrDefault(),
                GroupName = groupInstance.Group.GroupName,
                Time = groupInstance.Group.Time,
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
}