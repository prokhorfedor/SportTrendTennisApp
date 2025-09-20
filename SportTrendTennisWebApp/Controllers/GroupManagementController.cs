using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace SportTrendTennisWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class GroupManagementController : Controller
{
    private readonly IGroupManagementService _groupManagementService;
    private readonly ILogger<SignUpController> _logger;

    public GroupManagementController(IGroupManagementService groupManagementService, ILogger<SignUpController> logger)
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
            return await _groupManagementService.GetGroupsWithScheduleAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<Guid>> RegisterIntoGroup([FromBody] RegisterToGroupRequest request)
    {
        try
        {
            return await _groupManagementService.RegisterIntoGroupAsync(request);
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
            return await _groupManagementService.GetGroupInstanceAsync(request);
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
            return await _groupManagementService.GetGroupInstanceByIdAsync(groupInstanceId);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}