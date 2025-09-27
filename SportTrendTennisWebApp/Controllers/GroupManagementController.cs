using System.Security.Claims;
using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace SportTrendTennisWebApp.Controllers;

[ApiController]
[Authorize]
[Route("[controller]")]
public class GroupManagementController : Controller
{
    private readonly IGroupManagementService _groupManagementService;
    private readonly ILogger<UserController> _logger;

    public GroupManagementController(IGroupManagementService groupManagementService, ILogger<UserController> logger)
    {
        _groupManagementService = groupManagementService;
        _logger = logger;
    }


    [HttpGet]
    [Route("[action]")]
    public async Task<ActionResult<GroupScheduleResponse>> GetGroupsWithSchedule()
    {
        try
        {
            return Ok(await _groupManagementService.GetGroupsWithScheduleAsync());
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<RegisterToGroupResponse>> RegisterIntoGroup([FromBody] RegisterToGroupRequest request)
    {
        try
        {
            var userId = GetUserIdFromToken();
            return Ok(await _groupManagementService.RegisterIntoGroupAsync(request,userId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<GroupInstanceResponse>> GetGroupInstance([FromBody] GetGroupInstanceRequest request)
    {
        try
        {
            return Ok(await _groupManagementService.GetGroupInstanceAsync(request));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpGet]
    [Route("[action]/{groupInstanceId}")]
    public async Task<ActionResult<GroupInstanceResponse>> GetGroupInstanceById([FromRoute] Guid groupInstanceId)
    {
        try
        {
            return Ok(await _groupManagementService.GetGroupInstanceByIdAsync(groupInstanceId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<Guid>> AddGroup([FromBody] CreateGroupRequest request)
    {
        try
        {
            var userId = GetUserIdFromToken();
            return Ok(await _groupManagementService.CreateGroupAsync(request, userId));
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<GroupInstanceResponse>> UpdateGroupStatus([FromBody] UpdateGroupStatusRequest request)
    {
        try
        {
            var response = await _groupManagementService.UpdateGroupInstanceStatusAsync(request);
            return Ok(response);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    protected Guid GetUserIdFromToken()
    {
        Guid UserId = Guid.Empty;
        try
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var identity = HttpContext.User.Identity as ClaimsIdentity;
                if (identity != null)
                {
                    IEnumerable<Claim> claims = identity.Claims;
                    string strUserId = identity.FindFirst("UserId").Value;
                    Guid.TryParse(strUserId, out UserId);
                }
            }
            return UserId;
        }
        catch
        {
            return UserId;
        }
    }
}