using LegalPro.Application.Common.Interfaces;
using LegalPro.Domain.Common;
using LegalPro.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace LegalPro.Infrastructure.Persistence;

/// <summary>
/// DbContext with domain event dispatching support and auto-audit timestamps.
/// </summary>
public class ApplicationDbContext : DbContext, IApplicationDbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Expediente> Expedientes => Set<Expediente>();
    public DbSet<Simulacion> Simulaciones => Set<Simulacion>();
    public DbSet<EventoSimulacion> EventosSimulacion => Set<EventoSimulacion>();
    public DbSet<BaseLegalVectorial> BaseLegalVectorial => Set<BaseLegalVectorial>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Apply ALL Fluent API configurations from the Configurations folder
        builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
    }

    /// <summary>
    /// Override SaveChangesAsync to dispatch domain events after persisting.
    /// Timestamps ya son gestionados por cada entidad en sus métodos de dominio —
    /// no se sobreescriben aquí para respetar el encapsulamiento.
    /// </summary>
    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        return await base.SaveChangesAsync(cancellationToken);
    }
}
