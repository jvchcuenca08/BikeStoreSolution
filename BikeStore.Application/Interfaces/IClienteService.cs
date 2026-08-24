using BikeStore.Application.DTOs.Clientes;

namespace BikeStore.Application.Interfaces;

public interface IClienteService
{
    Task<List<ClienteDto>> ObtenerTodosAsync();

    Task<ClienteDto?> ObtenerPorIdAsync(int id);

    Task<ClienteDto> CrearAsync(CrearClienteDto clienteDto);

    Task<bool> ActualizarAsync(int id, ActualizarClienteDto clienteDto);

    Task<bool> EliminarAsync(int id);

    Task<ClienteDto?> BuscarPorCedulaAsync(string cedula);

    Task<List<ClienteDto>> BuscarPorApellidoAsync(string apellido);
}
