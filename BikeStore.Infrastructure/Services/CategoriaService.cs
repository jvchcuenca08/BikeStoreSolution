using BikeStore.Application.DTOs.Categorias;
using BikeStore.Application.Interfaces;
using BikeStore.Domain.Entities;
using BikeStore.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace BikeStore.Infrastructure.Services;

public class CategoriaService : ICategoriaService
{
    private readonly BikeStoreDbContext _context;

    public CategoriaService(BikeStoreDbContext context)
    {
        _context = context;
    }

    public async Task<List<CategoriaDto>> ObtenerTodasAsync()
    {
        return await _context.Categorias
            .OrderBy(c => c.IdCategoria)
            .Select(c => new CategoriaDto
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = c.Activo
            })
            .ToListAsync();
    }

    public async Task<CategoriaDto?> ObtenerPorIdAsync(int id)
    {
        return await _context.Categorias
            .Where(c => c.IdCategoria == id)
            .Select(c => new CategoriaDto
            {
                IdCategoria = c.IdCategoria,
                Nombre = c.Nombre,
                Descripcion = c.Descripcion,
                Activo = c.Activo
            })
            .FirstOrDefaultAsync();
    }

    public async Task<CategoriaDto> CrearAsync(CrearCategoriaDto categoriaDto)
    {
        bool existe = await _context.Categorias
            .AnyAsync(c => c.Nombre.ToLower() == categoriaDto.Nombre.ToLower());

        if (existe)
        {
            throw new InvalidOperationException("Ya existe una categoría con ese nombre.");
        }

        var categoria = new Categoria
        {
            Nombre = categoriaDto.Nombre.Trim(),
            Descripcion = categoriaDto.Descripcion?.Trim(),
            Activo = categoriaDto.Activo
        };

        _context.Categorias.Add(categoria);
        await _context.SaveChangesAsync();

        return new CategoriaDto
        {
            IdCategoria = categoria.IdCategoria,
            Nombre = categoria.Nombre,
            Descripcion = categoria.Descripcion,
            Activo = categoria.Activo
        };
    }

    public async Task<bool> ActualizarAsync(int id, ActualizarCategoriaDto categoriaDto)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.IdCategoria == id);

        if (categoria == null)
        {
            return false;
        }

        bool existeNombre = await _context.Categorias
            .AnyAsync(c =>
                c.IdCategoria != id &&
                c.Nombre.ToLower() == categoriaDto.Nombre.ToLower());

        if (existeNombre)
        {
            throw new InvalidOperationException("Ya existe otra categoría con ese nombre.");
        }

        categoria.Nombre = categoriaDto.Nombre.Trim();
        categoria.Descripcion = categoriaDto.Descripcion?.Trim();
        categoria.Activo = categoriaDto.Activo;

        await _context.SaveChangesAsync();

        return true;
    }

    public async Task<bool> EliminarAsync(int id)
    {
        var categoria = await _context.Categorias
            .FirstOrDefaultAsync(c => c.IdCategoria == id);

        if (categoria == null)
        {
            return false;
        }

        _context.Categorias.Remove(categoria);
        await _context.SaveChangesAsync();

        return true;
    }
}