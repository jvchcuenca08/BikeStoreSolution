using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;
using System.Net.Http.Json;

namespace BikeStore.Controllers
{
    public class CategoriasController : Controller
    {
        private readonly HttpClient _httpClient;

        public CategoriasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeStoreAPI");
        }

        // =====================================================
        // LISTAR CATEGORÍAS
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var categorias = await _httpClient
                    .GetFromJsonAsync<List<Categoria>>("api/categorias");

                return View(categorias ?? new List<Categoria>());
            }
            catch
            {
                ViewBag.Error = "No se pudo conectar con la API.";

                return View(new List<Categoria>());
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
        // CREAR CATEGORÍA
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync(
                    "api/categorias",
                    categoria);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo registrar la categoría.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(categoria);
        }

        // =====================================================
        // MOSTRAR FORMULARIO DE EDITAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var categoria = await _httpClient
                    .GetFromJsonAsync<Categoria>(
                        $"api/categorias/{id}");

                if (categoria == null)
                {
                    return NotFound();
                }

                return View(categoria);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }

        // =====================================================
        // ACTUALIZAR CATEGORÍA
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Categoria categoria)
        {
            if (!ModelState.IsValid)
            {
                return View(categoria);
            }

            try
            {
                var respuesta = await _httpClient.PutAsJsonAsync(
                    $"api/categorias/{categoria.IdCategoria}",
                    categoria);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo actualizar la categoría.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(categoria);
        }

        // =====================================================
        // MOSTRAR CONFIRMACIÓN DE ELIMINAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var categoria = await _httpClient
                    .GetFromJsonAsync<Categoria>($"api/categorias/{id}");

                if (categoria == null)
                {
                    return NotFound();
                }

                return View(categoria);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        // =====================================================
        // CONFIRMAR ELIMINACIÓN
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var respuesta = await _httpClient
                    .DeleteAsync($"api/categorias/{id}");

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo eliminar la categoría.";
                }
            }
            catch
            {
                TempData["Error"] = "No se pudo conectar con la API.";
            }

            return RedirectToAction(nameof(Index));
        }
    }
}