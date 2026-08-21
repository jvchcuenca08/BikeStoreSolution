using BikeStore.Application.DTOs.Categorias;

namespace BikeStore.Application.Interfaces;

public interface ICategoriaService
{
    Task<List<CategoriaDto>> ObtenerTodasAsync();

    Task<CategoriaDto?> ObtenerPorIdAsync(int id);

    Task<CategoriaDto> CrearAsync(CrearCategoriaDto categoriaDto);

    Task<bool> ActualizarAsync(int id, ActualizarCategoriaDto categoriaDto);

    Task<bool> EliminarAsync(int id);
}