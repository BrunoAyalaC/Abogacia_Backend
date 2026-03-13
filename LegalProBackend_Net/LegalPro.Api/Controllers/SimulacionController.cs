using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace LegalPro.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class SimulacionController : ControllerBase
{
    [HttpPost("iniciar")]
    public IActionResult IniciarSimulacion([FromBody] object request)
    {
        return Ok(new
        {
            simulacionId = Guid.NewGuid().ToString(),
            estado = "En progreso",
            mensajeJuez = "La sesión ha comenzado. Señor abogado, tiene la palabra.",
            puntajeInicial = 100
        });
    }

    [HttpPost("turno")]
    public IActionResult TurnoSimulacion([FromBody] object request)
    {
        return Ok(new
        {
            mensajeOponente = "El Ministerio Público se opone al planteamiento de la defensa por carecer de sustento fáctico.",
            puntajeActual = 95,
            isFin = false
        });
    }
}
