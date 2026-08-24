using BikeStore.Application.DTOs.Clientes;
using BikeStore.Application.Interfaces;
using BikeStore.Domain.Entities;
using BikeStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Infrastructure.Services;

public class ClienteService : IClienteService
{
    private readonly BikeStoreDbContext _context;

    public ClienteService(BikeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<ClienteDto>> ObtenerTodosAsync()
    {
        return await _context.Clientes
            .OrderBy(c => c.IdCliente)
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Cedula = c.Cedula,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Telefono = c.Telefono,
                Correo = c.Correo
            })
            .ToListAsync();
    }

    public async Task<ClienteDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Clientes
            .Where(c => c.IdCliente == id)
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Cedula = c.Cedula,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Telefono = c.Telefono,
                Correo = c.Correo
            })
            .FirstOrDefaultAsync();
    }



    public async Task<ClienteDto> CrearAsync(CrearClienteDto clienteDto)
    {
        string cedula = clienteDto.Cedula.Trim();

        bool existeCedula = await _context.Clientes
            .AnyAsync(c => c.Cedula == cedula);

        if (existeCedula)
        {
            throw new InvalidOperationException("Ya existe un cliente con esa cédula.");
        }

        var cliente = new Cliente
        {
            Cedula = cedula,
            Nombres = clienteDto.Nombres.Trim(),
            Apellidos = clienteDto.Apellidos.Trim(),
            Telefono = clienteDto.Telefono?.Trim(),
            Correo = clienteDto.Correo?.Trim()
        };

        _context.Clientes.Add(cliente);
        await _context.SaveChangesAsync();

        return new ClienteDto
        {
            IdCliente = cliente.IdCliente,
            Cedula = cliente.Cedula,
            Nombres = cliente.Nombres,
            Apellidos = cliente.Apellidos,
            Telefono = cliente.Telefono,
            Correo = cliente.Correo
        };
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarClienteDto clienteDto)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id);

        if (cliente == null)
        {
            return false;
        }

        string cedula = clienteDto.Cedula.Trim();

        bool existeCedula = await _context.Clientes
            .AnyAsync(c =>
                c.IdCliente != id &&
                c.Cedula == cedula);

        if (existeCedula)
        {
            throw new InvalidOperationException(
                "Ya existe otro cliente con esa cédula.");
        }

        cliente.Cedula = cedula;
        cliente.Nombres = clienteDto.Nombres.Trim();
        cliente.Apellidos = clienteDto.Apellidos.Trim();
        cliente.Telefono = clienteDto.Telefono?.Trim();
        cliente.Correo = clienteDto.Correo?.Trim();

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var cliente = await _context.Clientes
            .FirstOrDefaultAsync(c => c.IdCliente == id);

        if (cliente == null)
        {
            return false;
        }

        _context.Clientes.Remove(cliente);
        await _context.SaveChangesAsync();

        return true;
    }




    public async Task<ClienteDto?> BuscarPorCedulaAsync(string cedula)
    {
        cedula = cedula.Trim();

        return await _context.Clientes
            .Where(c => c.Cedula == cedula)
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Cedula = c.Cedula,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Telefono = c.Telefono,
                Correo = c.Correo
            })
            .FirstOrDefaultAsync();
    }



    public async Task<List<ClienteDto>> BuscarPorApellidoAsync(string apellido)
    {
        apellido = apellido.Trim();

        return await _context.Clientes
            .Where(c => c.Apellidos.Contains(apellido))
            .OrderBy(c => c.Apellidos)
            .Select(c => new ClienteDto
            {
                IdCliente = c.IdCliente,
                Cedula = c.Cedula,
                Nombres = c.Nombres,
                Apellidos = c.Apellidos,
                Telefono = c.Telefono,
                Correo = c.Correo
            })
            .ToListAsync();
    }
}
