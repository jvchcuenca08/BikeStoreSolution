using BikeStore.Application.DTOs.Clientes;
using BikeStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;



namespace BikeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ClientesController : ControllerBase
{
    private readonly IClienteService _clienteService;

    public ClientesController(IClienteService clienteService)
    {
        _clienteService = clienteService;
    }



    [HttpGet]
    public async Task<ActionResult<List<ClienteDto>>> ObtenerTodos()
    {
        var clientes = await _clienteService.ObtenerTodosAsync();
        return Ok(clientes);
    }



    [HttpGet("{id:int}")]
    public async Task<ActionResult<ClienteDto>> ObtenerPorId(int id)
    {
        var cliente = await _clienteService.ObtenerPorIdAsync(id);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensaje = "El cliente solicitado no existe."
            });
        }

        return Ok(cliente);
    }


    [HttpPost]
    public async Task<ActionResult<ClienteDto>> Crear(
    [FromBody] CrearClienteDto clienteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var clienteCreado = await _clienteService.CrearAsync(clienteDto);

            return CreatedAtAction(
                nameof(ObtenerPorId),
                new { id = clienteCreado.IdCliente },
                clienteCreado
            );
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }


    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
    int id,
    [FromBody] ActualizarClienteDto clienteDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        try
        {
            var actualizado = await _clienteService.ActualizarAsync(id, clienteDto);

            if (!actualizado)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el cliente que desea actualizar."
                });
            }

            return NoContent();
        }
        catch (InvalidOperationException ex)
        {
            return Conflict(new
            {
                mensaje = ex.Message
            });
        }
    }



    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        try
        {
            var eliminado = await _clienteService.EliminarAsync(id);

            if (!eliminado)
            {
                return NotFound(new
                {
                    mensaje = "No se encontró el cliente que desea eliminar."
                });
            }

            return NoContent();
        }
        catch (DbUpdateException)
        {
            return Conflict(new
            {
                mensaje = "No se puede eliminar el cliente porque tiene ventas asociadas."
            });
        }
    }

    [HttpGet("cedula/{cedula}")]
    public async Task<ActionResult<ClienteDto>> BuscarPorCedula(string cedula)
    {
        var cliente = await _clienteService.BuscarPorCedulaAsync(cedula);

        if (cliente == null)
        {
            return NotFound(new
            {
                mensaje = "No se encontró un cliente con esa cédula."
            });
        }

        return Ok(cliente);
    }



    [HttpGet("apellido/{apellido}")]
    public async Task<ActionResult<List<ClienteDto>>> BuscarPorApellido(string apellido)
    {
        var clientes = await _clienteService.BuscarPorApellidoAsync(apellido);

        if (clientes.Count == 0)
        {
            return NotFound(new
            {
                mensaje = "No se encontraron clientes con ese apellido."
            });
        }

        return Ok(clientes);
    }

}