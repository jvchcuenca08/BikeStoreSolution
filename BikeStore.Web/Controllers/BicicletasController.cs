using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;
using System.Net.Http.Json;

namespace BikeStore.Controllers
{
    public class BicicletasController : Controller
    {
        private readonly HttpClient _httpClient;

        public BicicletasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeStoreAPI");
        }

        // =====================================================
        // LISTADO Y FILTROS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(
            string? busqueda,
            int? categoria,
            string? stockFiltro)
        {
            try
            {
                var bicicletas = await _httpClient
                    .GetFromJsonAsync<List<Bicicleta>>("api/bicicletas")
                    ?? new List<Bicicleta>();

                // BÚSQUEDA POR MARCA O MODELO
                if (!string.IsNullOrWhiteSpace(busqueda))
                {
                    busqueda = busqueda.Trim();

                    bicicletas = bicicletas
                        .Where(b =>
                            b.Marca.Contains(
                                busqueda,
                                StringComparison.OrdinalIgnoreCase)
                            ||
                            b.Modelo.Contains(
                                busqueda,
                                StringComparison.OrdinalIgnoreCase))
                        .ToList();
                }

                // FILTRO POR CATEGORÍA
                if (categoria.HasValue && categoria.Value > 0)
                {
                    bicicletas = bicicletas
                        .Where(b => b.IdCategoria == categoria.Value)
                        .ToList();
                }

                // FILTRO POR STOCK
                if (!string.IsNullOrWhiteSpace(stockFiltro))
                {
                    if (stockFiltro.Equals(
                        "bajo",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        bicicletas = bicicletas
                            .Where(b => b.Stock > 0 && b.Stock <= 3)
                            .ToList();
                    }
                    else if (stockFiltro.Equals(
                        "agotado",
                        StringComparison.OrdinalIgnoreCase))
                    {
                        bicicletas = bicicletas
                            .Where(b => b.Stock == 0)
                            .ToList();
                    }
                }

                ViewBag.Busqueda = busqueda;
                ViewBag.Categoria = categoria;
                ViewBag.StockFiltro = stockFiltro;

                return View(bicicletas);
            }
            catch
            {
                ViewBag.Error = "No se pudo conectar con la API.";

                return View(new List<Bicicleta>());
            }
        }

        // =====================================================
        // MOSTRAR FORMULARIO DE CREAR
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        // =====================================================
        // CREAR BICICLETA - POST API
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Bicicleta bicicleta)
        {
            if (!ModelState.IsValid)
            {
                return View(bicicleta);
            }

            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync(
                    "api/bicicletas",
                    bicicleta);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo registrar la bicicleta.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(bicicleta);
        }

        // =====================================================
        // MOSTRAR FORMULARIO DE EDITAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var bicicleta = await _httpClient
                    .GetFromJsonAsync<Bicicleta>(
                        $"api/bicicletas/{id}");

                if (bicicleta == null)
                {
                    return NotFound();
                }

                return View(bicicleta);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // =====================================================
        // EDITAR BICICLETA - PUT API
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Bicicleta bicicleta)
        {
            if (!ModelState.IsValid)
            {
                return View(bicicleta);
            }

            try
            {
                var respuesta = await _httpClient.PutAsJsonAsync(
                    $"api/bicicletas/{bicicleta.IdBicicleta}",
                    bicicleta);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo actualizar la bicicleta.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(bicicleta);
        }

        // =====================================================
        // MOSTRAR CONFIRMACIÓN DE ELIMINAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var bicicleta = await _httpClient
                    .GetFromJsonAsync<Bicicleta>(
                        $"api/bicicletas/{id}");

                if (bicicleta == null)
                {
                    return NotFound();
                }

                return View(bicicleta);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // =====================================================
        // ELIMINAR BICICLETA - DELETE API
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var respuesta = await _httpClient
                    .DeleteAsync($"api/bicicletas/{id}");

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] =
                        "No se pudo eliminar la bicicleta.";
                }
            }
            catch
            {
                TempData["Error"] =
                    "No se pudo conectar con la API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}