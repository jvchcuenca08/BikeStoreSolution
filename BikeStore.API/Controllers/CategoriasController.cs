using BikeStore.Application.DTOs.Categorias;
using BikeStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.API.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CategoriasController : ControllerBase
{
    private readonly ICategoriaService _categoriaService;

    public CategoriasController(ICategoriaService categoriaService)
    {
        _categoriaService = categoriaService;
    }

    [HttpGet]
    public async Task<ActionResult<List<CategoriaDto>>> ObtenerTodas()
    {
        var categorias = await _categoriaService.ObtenerTodasAsync();
        return Ok(categorias);
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<CategoriaDto>> ObtenerPorId(int id)
    {
        var categoria = await _categoriaService.ObtenerPorIdAsync(id);

        if (categoria == null)
        {
            return NotFound(new
            {
                mensaje = "La categoría solicitada no existe."
            });
        }

        return Ok(categoria);
    }

    [HttpPost]
    public async Task<ActionResult<CategoriaDto>> Crear([FromBody] CrearCategoriaDto categoriaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var categoriaCreada = await _categoriaService.CrearAsync(categoriaDto);

        return CreatedAtAction(
            nameof(ObtenerPorId),
            new { id = categoriaCreada.IdCategoria },
            categoriaCreada
        );
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Actualizar(
        int id,
        [FromBody] ActualizarCategoriaDto categoriaDto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        var actualizado = await _categoriaService.ActualizarAsync(id, categoriaDto);

        if (!actualizado)
        {
            return NotFound(new
            {
                mensaje = "No se encontró la categoría que desea actualizar."
            });
        }

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Eliminar(int id)
    {
        var eliminado = await _categoriaService.EliminarAsync(id);

        if (!eliminado)
        {
            return NotFound(new
            {
                mensaje = "No se encontró la categoría que desea eliminar."
            });
        }

        return NoContent();
    }
}