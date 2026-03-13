using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace LegalPro.Api.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class JurisprudenciaController : ControllerBase
{
    [HttpPost("buscar")]
    public IActionResult Buscar([FromBody] object request)
    {
        var dummyResult = new
        {
            queryOriginal = "Casación Penal",
            resultados = new List<object>
            {
                new {
                    id = "CAS-123",
                    titulo = "Casación 123-2022 Puno",
                    resumen = "Establece doctrina legal sobre la prisión preventiva.",
                    sala = "Sala Penal Permanente",
                    fechaResolucion = "2023-01-15",
                    nivelRelevancia = "Alta",
                    urlDocumento = "https://example.com"
                }
            },
            contextoGenerado = "La Corte Suprema ha establecido pautas estrictas para la prisión preventiva."
        };
        return Ok(dummyResult);
    }
}
