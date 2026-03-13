using MediatR;
using Microsoft.AspNetCore.Mvc;
using LegalPro.Application.Auth.Commands;
using LegalPro.Application.Auth.Queries;

namespace LegalPro.Api.Controllers;

// ═══════════════════════════════════════════════════════
// SRP: Controller ONLY delegates to MediatR.
// DIP: No try/catch here — ExceptionHandlingMiddleware
//      handles ALL exceptions globally (DRY + SRP).
// ═══════════════════════════════════════════════════════
[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly IMediator _mediator;

    public AuthController(IMediator mediator) => _mediator = mediator;

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterCommand command)
    {
        var token = await _mediator.Send(command);
        return Ok(new { Token = token, Mensaje = "Usuario registrado exitosamente." });
    }

    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginQuery query)
    {
        var token = await _mediator.Send(query);
        return Ok(new { Token = token });
    }
}
