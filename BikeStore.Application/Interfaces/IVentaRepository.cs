using BikeStore.Application.DTOs.Ventas;

namespace BikeStore.Application.Interfaces
{
    public interface IVentaRepository
    {
        Task<ResumenVentaDto> RegistrarVentaAsync(RegistrarVentaDto dto);
        Task<IEnumerable<ResumenVentaDto>> ObtenerHistorialAsync();
        Task<IEnumerable<ResumenVentaDto>> ObtenerPorClienteAsync(int idCliente);
    }
}