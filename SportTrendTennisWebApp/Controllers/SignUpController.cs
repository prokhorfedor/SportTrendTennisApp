using Contracts.Requests;
using Contracts.Responses;
using Microsoft.AspNetCore.Mvc;
using Service.Interfaces;

namespace SportTrendTennisWebApp.Controllers;

[ApiController]
[Route("[controller]")]
public class SignUpController : ControllerBase
{
    private readonly IUserService _userService;
    private readonly ILogger<SignUpController> _logger;

    public SignUpController(IUserService userService, ILogger<SignUpController> logger)
    {
        _userService = userService;
        _logger = logger;
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<ActionResult<CreateUserResponse>> CreateUser(CreateUserRequest request)
    {
        try
        {
            var user = await _userService.CreateUserAsync(request);
            return this.Ok(user);
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
    }
}