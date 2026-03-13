using LegalPro.Domain.Common;

namespace LegalPro.Domain.Events;

/// <summary>Raised when a new user is registered.</summary>
public class UsuarioRegistradoEvent : DomainEvent
{
    public int UsuarioId { get; }
    public string Email { get; }
    public string Rol { get; }

    public UsuarioRegistradoEvent(int usuarioId, string email, string rol)
    {
        UsuarioId = usuarioId;
        Email = email;
        Rol = rol;
    }
}

/// <summary>Raised when a new expediente is created.</summary>
public class ExpedienteCreadoEvent : DomainEvent
{
    public int ExpedienteId { get; }
    public string Numero { get; }
    public int UsuarioId { get; }

    public ExpedienteCreadoEvent(int expedienteId, string numero, int usuarioId)
    {
        ExpedienteId = expedienteId;
        Numero = numero;
        UsuarioId = usuarioId;
    }
}

/// <summary>Raised when a simulation finishes.</summary>
public class SimulacionFinalizadaEvent : DomainEvent
{
    public int SimulacionId { get; }
    public int PuntajeFinal { get; }
    public int UsuarioId { get; }

    public SimulacionFinalizadaEvent(int simulacionId, int puntajeFinal, int usuarioId)
    {
        SimulacionId = simulacionId;
        PuntajeFinal = puntajeFinal;
        UsuarioId = usuarioId;
    }
}

/// <summary>Raised when an expediente changes state.</summary>
public class ExpedienteEstadoCambiadoEvent : DomainEvent
{
    public int ExpedienteId { get; }
    public string EstadoAnterior { get; }
    public string EstadoNuevo { get; }

    public ExpedienteEstadoCambiadoEvent(int expedienteId, string estadoAnterior, string estadoNuevo)
    {
        ExpedienteId = expedienteId;
        EstadoAnterior = estadoAnterior;
        EstadoNuevo = estadoNuevo;
    }
}
