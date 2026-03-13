using LegalPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace LegalPro.Application.Common.Interfaces;

public interface IApplicationDbContext
{
    DbSet<Usuario> Usuarios { get; }
    DbSet<Expediente> Expedientes { get; }
    DbSet<Simulacion> Simulaciones { get; }
    DbSet<EventoSimulacion> EventosSimulacion { get; }
    DbSet<BaseLegalVectorial> BaseLegalVectorial { get; }
    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
