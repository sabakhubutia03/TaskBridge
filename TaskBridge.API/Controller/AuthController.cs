using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;


namespace TaskBridge.Controller;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpPost("register")]

    public async Task<ActionResult> Register([FromBody] RegisterCommand command)
    {
        var registerResult = await _mediator.Send(command);
        return Ok(registerResult);
    }

    [HttpPost("login")]
    public async Task<ActionResult> Login(LoginCommand command)
    {
        var login = await _mediator.Send(command);
        return Ok(login);
    }

    [Authorize]
    [HttpGet("Me")]
    public ActionResult Me()
    {
        return Ok("You are authenticated!");
    }
}