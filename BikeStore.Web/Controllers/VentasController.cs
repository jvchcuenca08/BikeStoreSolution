using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;
using System.Net.Http.Json;

namespace BikeStore.Controllers
{
    public class VentasController : Controller
    {
        private readonly HttpClient _httpClient;

        public VentasController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient("BikeStoreAPI");
        }

        // =====================================================
        // HISTORIAL DE VENTAS - API REAL
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            try
            {
                var ventas = await _httpClient
                    .GetFromJsonAsync<List<Venta>>("api/Ventas")
                    ?? new List<Venta>();

                // Obtener clientes para mostrar sus nombres
                var clientes = await _httpClient
                    .GetFromJsonAsync<List<Cliente>>("api/Clientes")
                    ?? new List<Cliente>();

                foreach (var venta in ventas)
                {
                    var cliente = clientes
                        .FirstOrDefault(c =>
                            c.IdCliente == venta.IdCliente);

                    if (cliente != null)
                    {
                        venta.Cliente =
                            $"{cliente.Nombres} {cliente.Apellidos}";
                    }
                    else
                    {
                        venta.Cliente = $"Cliente #{venta.IdCliente}";
                    }
                }

                return View(ventas);
            }
            catch
            {
                ViewBag.Error =
                    "No se pudo conectar con la API de Ventas.";

                return View(new List<Venta>());
            }
        }

        // =====================================================
        // MOSTRAR FORMULARIO DE NUEVA VENTA
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            try
            {
                // CLIENTES REALES
                ViewBag.Clientes = await _httpClient
                    .GetFromJsonAsync<List<Cliente>>("api/Clientes")
                    ?? new List<Cliente>();

                // BICICLETAS REALES
                ViewBag.Bicicletas = await _httpClient
                    .GetFromJsonAsync<List<Bicicleta>>("api/Bicicletas")
                    ?? new List<Bicicleta>();

                return View();
            }
            catch
            {
                ViewBag.Error =
                    "No se pudo cargar clientes o bicicletas desde la API.";

                ViewBag.Clientes = new List<Cliente>();
                ViewBag.Bicicletas = new List<Bicicleta>();

                return View();
            }
        }
        // =====================================================
        // REGISTRAR VENTA - POST API
        // =====================================================

        [HttpPost]
        public async Task<IActionResult> Create(
            [FromBody] RegistrarVenta venta)
        {
            if (venta == null ||
                venta.IdCliente <= 0 ||
                venta.Detalles == null ||
                venta.Detalles.Count == 0)
            {
                return BadRequest(
                    new { mensaje = "Debe seleccionar un cliente y agregar al menos una bicicleta." });
            }

            try
            {
                var respuesta = await _httpClient.PostAsJsonAsync(
                    "api/Ventas",
                    venta);

                if (respuesta.IsSuccessStatusCode)
                {
                    return Ok(new
                    {
                        mensaje = "Venta registrada correctamente."
                    });
                }

                var error = await respuesta.Content.ReadAsStringAsync();

                return BadRequest(new
                {
                    mensaje = string.IsNullOrWhiteSpace(error)
                        ? "No se pudo registrar la venta."
                        : error
                });
            }
            catch
            {
                return StatusCode(500, new
                {
                    mensaje = "No se pudo conectar con la API de Ventas."
                });
            }
        }
        // =====================================================
        // BUSCAR VENTAS POR CLIENTE
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> PorCliente(int idCliente)
        {
            try
            {
                var ventas = await _httpClient
                    .GetFromJsonAsync<List<Venta>>(
                        $"api/Ventas/cliente/{idCliente}")
                    ?? new List<Venta>();

                var clientes = await _httpClient
                    .GetFromJsonAsync<List<Cliente>>("api/Clientes")
                    ?? new List<Cliente>();

                foreach (var venta in ventas)
                {
                    var cliente = clientes
                        .FirstOrDefault(c => c.IdCliente == venta.IdCliente);

                    venta.Cliente = cliente != null
                        ? $"{cliente.Nombres} {cliente.Apellidos}"
                        : $"Cliente #{venta.IdCliente}";
                }

                return View("Index", ventas);
            }
            catch
            {
                ViewBag.Error =
                    "No se pudieron consultar las ventas del cliente.";

                return View("Index", new List<Venta>());
            }
        }
        // =====================================================
        // VER DETALLE DE UNA VENTA
        // =====================================================

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            try
            {
                var ventas = await _httpClient
                    .GetFromJsonAsync<List<Venta>>("api/Ventas")
                    ?? new List<Venta>();

                var venta = ventas
                    .FirstOrDefault(v => v.IdVenta == id);

                if (venta == null)
                {
                    return NotFound();
                }

                var clientes = await _httpClient
                    .GetFromJsonAsync<List<Cliente>>("api/Clientes")
                    ?? new List<Cliente>();

                var cliente = clientes
                    .FirstOrDefault(c => c.IdCliente == venta.IdCliente);

                venta.Cliente = cliente != null
                    ? $"{cliente.Nombres} {cliente.Apellidos}"
                    : $"Cliente #{venta.IdCliente}";

                var bicicletas = await _httpClient
                    .GetFromJsonAsync<List<Bicicleta>>("api/Bicicletas")
                    ?? new List<Bicicleta>();

                foreach (var detalle in venta.Detalles)
                {
                    var bicicleta = bicicletas
                        .FirstOrDefault(b =>
                            b.IdBicicleta == detalle.IdBicicleta);

                    detalle.Bicicleta = bicicleta != null
                        ? $"{bicicleta.Marca} {bicicleta.Modelo}"
                        : $"Bicicleta #{detalle.IdBicicleta}";
                }

                return View(venta);
            }
            catch
            {
                return RedirectToAction(nameof(Index));
            }
        }
    }
}