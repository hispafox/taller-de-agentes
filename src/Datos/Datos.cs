using Microsoft.EntityFrameworkCore;

namespace GestorIncidencias;

public class Datos(DbContextOptions<Datos> opciones) : DbContext(opciones)
{
    public DbSet<Incidencia> Incidencias => Set<Incidencia>();
    public DbSet<Zona> Zonas => Set<Zona>();

    protected override void OnModelCreating(ModelBuilder modelo)
    {
        modelo.Entity<Incidencia>(incidencia =>
        {
            incidencia.Property(i => i.Titulo).HasMaxLength(200);
            incidencia.Property(i => i.Prioridad).HasDefaultValue(PrioridadIncidencia.Normal).HasSentinel((PrioridadIncidencia)(-1));
            incidencia.Property(i => i.Estado).HasDefaultValue(EstadoIncidencia.Abierta).HasSentinel((EstadoIncidencia)(-1));
            incidencia.Property(i => i.FechaApertura).HasDefaultValueSql("CURRENT_TIMESTAMP");
        });

        modelo.Entity<Zona>().Property(z => z.Nombre).HasMaxLength(100);
    }
}
