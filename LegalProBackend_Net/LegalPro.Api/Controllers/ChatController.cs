using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalPro.Application.Chat.Commands;

namespace LegalPro.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ChatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ChatController(IMediator mediator) => _mediator = mediator;

    [HttpPost("enviar")]
    public async Task<IActionResult> Enviar([FromBody] EnviarMensajeChatCommand command)
    {
        var result = await _mediator.Send(command);
        return Ok(result);
    }
}
