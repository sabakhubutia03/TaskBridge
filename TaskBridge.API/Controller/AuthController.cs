using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TaskBridge.Application.Commands;
using TaskBridge.Application.DTOs;
using TaskBridge.Application.Interfaces;


namespace TaskBridge.Controller;
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;
    private readonly IMediator _mediator;

    public AuthController(IAuthService authService, IMediator mediator)
    {
        _authService = authService;
        _mediator = mediator;
    }

    [HttpPost("register")]
    public async Task<ActionResult> Register(RegisterDto dto)
    {
        var result = await _authService.Register(dto);
        return Ok(result);
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