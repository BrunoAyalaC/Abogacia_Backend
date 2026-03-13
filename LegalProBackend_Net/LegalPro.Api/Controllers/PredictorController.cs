using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using LegalPro.Application.Prediccion.Queries;

namespace LegalPro.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class PredictorController : ControllerBase
{
    private readonly IMediator _mediator;

    public PredictorController(IMediator mediator) => _mediator = mediator;

    [HttpPost("predecir")]
    public async Task<IActionResult> Predecir([FromBody] PredecirResultadoQuery query)
    {
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
