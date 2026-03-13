using LegalPro.Domain.Entities;

namespace LegalPro.Application.Common.Interfaces;

public interface ISimulationService
{
    Task<Simulacion> IniciarSimulacionAsync(int usuarioId, string ramaDerecho, string rolUsuario, string dificultadModificador);
    Task<EventoSimulacion> ProcesarTurnoAsync(int simulacionId, string emisor, string mensaje);
    Task<Simulacion?> ObtenerSimulacionActivaObtenerAsync(int usuarioId);
}
