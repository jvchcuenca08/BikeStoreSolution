using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;
using System.Net.Http.Json;

namespace BikeStore.Controllers
{
    public class ClientesController : Controller
    {
        private readonly HttpClient _httpClient;

        public ClientesController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeStoreAPI");
        }

        // =====================================================
        // LISTAR CLIENTES
        // =====================================================

        // =====================================================
        // LISTAR Y BUSCAR CLIENTES
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index(
            string? cedula,
            string? apellido)
        {
            try
            {
                List<Cliente> clientes;

                if (!string.IsNullOrWhiteSpace(cedula))
                {
                    cedula = cedula.Trim();

                    var cliente = await _httpClient
                        .GetFromJsonAsync<Cliente>(
                            $"api/clientes/cedula/{Uri.EscapeDataString(cedula)}");

                    clientes = cliente != null
                        ? new List<Cliente> { cliente }
                        : new List<Cliente>();
                }
                else if (!string.IsNullOrWhiteSpace(apellido))
                {
                    apellido = apellido.Trim();

                    clientes = await _httpClient
                        .GetFromJsonAsync<List<Cliente>>(
                            $"api/clientes/apellido/{Uri.EscapeDataString(apellido)}")
                        ?? new List<Cliente>();
                }
                else
                {
                    clientes = await _httpClient
                        .GetFromJsonAsync<List<Cliente>>("api/clientes")
                        ?? new List<Cliente>();
                }

                ViewBag.Cedula = cedula;
                ViewBag.Apellido = apellido;

                return View(clientes);
            }
            catch
            {
                ViewBag.Error = "No se pudo realizar la consulta.";

                return View(new List<Cliente>());
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
        // CREAR CLIENTE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync(
                    "api/clientes",
                    cliente);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo registrar el cliente.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(cliente);
        }
        // =====================================================
        // MOSTRAR FORMULARIO DE EDITAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            try
            {
                var cliente = await _httpClient
                    .GetFromJsonAsync<Cliente>($"api/clientes/{id}");

                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }


        // =====================================================
        // ACTUALIZAR CLIENTE
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(Cliente cliente)
        {
            if (!ModelState.IsValid)
            {
                return View(cliente);
            }

            try
            {
                var respuesta = await _httpClient.PutAsJsonAsync(
                    $"api/clientes/{cliente.IdCliente}",
                    cliente);

                if (respuesta.IsSuccessStatusCode)
                {
                    return RedirectToAction(nameof(Index));
                }

                ModelState.AddModelError(
                    "",
                    "No se pudo actualizar el cliente.");
            }
            catch
            {
                ModelState.AddModelError(
                    "",
                    "No se pudo conectar con la API.");
            }

            return View(cliente);
        }
        // =====================================================
        // MOSTRAR CONFIRMACIÓN DE ELIMINAR
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var cliente = await _httpClient
                    .GetFromJsonAsync<Cliente>($"api/clientes/{id}");

                if (cliente == null)
                {
                    return NotFound();
                }

                return View(cliente);
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
                    .DeleteAsync($"api/clientes/{id}");

                if (!respuesta.IsSuccessStatusCode)
                {
                    TempData["Error"] = "No se pudo eliminar el cliente.";
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