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
    public DbSet<Cliente> Clientes => Set<Cliente>();
    

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

        modelBuilder.Entity<Cliente>(entity =>
        {
            entity.ToTable("Cliente");

            entity.HasKey(e => e.IdCliente);

            entity.Property(e => e.Cedula)
                .HasMaxLength(10)
                .IsRequired();

            entity.HasIndex(e => e.Cedula)
                .IsUnique();

            entity.Property(e => e.Nombres)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Apellidos)
                .HasMaxLength(100)
                .IsRequired();

            entity.Property(e => e.Telefono)
                .HasMaxLength(20);

            entity.Property(e => e.Correo)
                .HasMaxLength(150);
        });
    }
}