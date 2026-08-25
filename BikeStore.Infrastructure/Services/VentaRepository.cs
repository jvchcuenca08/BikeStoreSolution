using BikeStore.Application.DTOs.Ventas;
using BikeStore.Application.Interfaces;
using BikeStore.Domain.Entities;
using BikeStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Infrastructure.Services
{
    public class VentaRepository : IVentaRepository
    {
        private readonly BikeStoreDbContext _context;

        public VentaRepository(BikeStoreDbContext context)
        {
            _context = context;
        }

        public async Task<ResumenVentaDto> RegistrarVentaAsync(RegistrarVentaDto dto)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                decimal subtotalAcumulado = 0;
                var detallesVenta = new List<DetalleVenta>();
                var detallesResumen = new List<DetalleResumenDto>();

                foreach (var item in dto.Detalles)
                {
                    var bici = await _context.Bicicletas.FindAsync(item.IdBicicleta);
                    if (bici == null)
                        throw new Exception($"La bicicleta con ID {item.IdBicicleta} no existe.");

                    if (bici.Stock < item.Cantidad)
                        throw new Exception($"Stock insuficiente para la bicicleta {bici.Modelo}. Stock actual: {bici.Stock}.");

                    bici.Stock -= item.Cantidad;
                    _context.Bicicletas.Update(bici);

                    decimal subtotalItem = bici.Precio * item.Cantidad;
                    subtotalAcumulado += subtotalItem;

                    detallesVenta.Add(new DetalleVenta
                    {
                        IdBicicleta = item.IdBicicleta,
                        Cantidad = item.Cantidad,
                        Precio = bici.Precio,
                        Subtotal = subtotalItem
                    });

                    detallesResumen.Add(new DetalleResumenDto
                    {
                        IdBicicleta = item.IdBicicleta,
                        Cantidad = item.Cantidad,
                        PrecioUnitario = bici.Precio,
                        Subtotal = subtotalItem
                    });
                }

                decimal iva = subtotalAcumulado * 0.15m;
                decimal total = subtotalAcumulado + iva;

                var nuevaVenta = new Venta
                {
                    Fecha = DateTime.Now,
                    IdCliente = dto.IdCliente,
                    Subtotal = subtotalAcumulado,
                    Iva = iva,
                    Total = total,
                    Detalles = detallesVenta
                };

                _context.Ventas.Add(nuevaVenta);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();

                return new ResumenVentaDto
                {
                    IdVenta = nuevaVenta.IdVenta,
                    Fecha = nuevaVenta.Fecha,
                    IdCliente = nuevaVenta.IdCliente,
                    Subtotal = nuevaVenta.Subtotal,
                    Iva = nuevaVenta.Iva,
                    Total = nuevaVenta.Total,
                    Detalles = detallesResumen
                };
            }
            catch (Exception)
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        public async Task<IEnumerable<ResumenVentaDto>> ObtenerHistorialAsync()
        {
            return await _context.Ventas
                .Include(v => v.Detalles)
                .Select(v => new ResumenVentaDto
                {
                    IdVenta = v.IdVenta,
                    Fecha = v.Fecha,
                    IdCliente = v.IdCliente,
                    Subtotal = v.Subtotal,
                    Iva = v.Iva,
                    Total = v.Total,
                    Detalles = v.Detalles.Select(d => new DetalleResumenDto
                    {
                        IdBicicleta = d.IdBicicleta,
                        Cantidad = d.Cantidad,
                        PrecioUnitario = d.Precio,
                        Subtotal = d.Subtotal
                    }).ToList()
                }).ToListAsync();
        }

        public async Task<IEnumerable<ResumenVentaDto>> ObtenerPorClienteAsync(int idCliente)
        {
            var historial = await ObtenerHistorialAsync();
            return historial.Where(v => v.IdCliente == idCliente);
        }
    }
}