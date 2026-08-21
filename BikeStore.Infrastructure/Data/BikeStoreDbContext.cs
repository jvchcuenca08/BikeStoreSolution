using BikeStore.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Infrastructure.Data;

public class BikeStoreDbContext : DbContext
{
    public BikeStoreDbContext(DbContextOptions<BikeStoreDbContext> options)
        : base(options)
    {
    }

    public DbSet<Categoria> Categorias => Set<Categoria>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Categoria>(entity =>
        {
            entity.ToTable("Categoria");

            entity.HasKey(e => e.IdCategoria);

            entity.Property(e => e.Nombre)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Descripcion)
                .HasMaxLength(250);

            entity.Property(e => e.Activo)
                .IsRequired();
        });
    }
}