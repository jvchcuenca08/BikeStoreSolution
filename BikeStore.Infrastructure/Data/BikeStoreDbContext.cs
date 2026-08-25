using BikeStore.Domain;
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
    public DbSet<Bicicleta> Bicicletas => Set<Bicicleta>();
    public DbSet<Venta> Ventas => Set<Venta>();
    public DbSet<DetalleVenta> DetalleVentas => Set<DetalleVenta>();

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

        modelBuilder.Entity<Bicicleta>(entity =>
        {
            entity.ToTable("Bicicleta");
            entity.HasKey(e => e.IdBicicleta);
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<Venta>(entity =>
        {
            entity.ToTable("Venta");
            entity.HasKey(e => e.IdVenta);
            entity.Property(e => e.IdVenta).ValueGeneratedOnAdd();
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Iva).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Total).HasColumnType("decimal(18,2)");
        });

        modelBuilder.Entity<DetalleVenta>(entity =>
        {
            entity.ToTable("DetalleVenta");
            entity.HasKey(e => e.IdDetalle);
            entity.Property(e => e.IdDetalle).ValueGeneratedOnAdd();
            entity.Property(e => e.Precio).HasColumnType("decimal(18,2)");
            entity.Property(e => e.Subtotal).HasColumnType("decimal(18,2)");

            entity.HasOne<Venta>()
                  .WithMany(v => v.Detalles)
                  .HasForeignKey(d => d.IdVenta);
        });
    }
}