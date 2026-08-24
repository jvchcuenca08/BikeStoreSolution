using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;

namespace BikeStore.Controllers
{
    public class BicicletasController : Controller
    {
        // =====================================================
        // LISTADO Y FILTROS
        // =====================================================

        [HttpGet]
        public IActionResult Index(
            string? busqueda,
            int? categoria,
            string? stockFiltro)
        {
            var bicicletas = new List<Bicicleta>
            {
                new Bicicleta
                {
                    IdBicicleta = 1,
                    IdCategoria = 1,
                    Marca = "Trek",
                    Modelo = "Marlin 5",
                    Precio = 850,
                    Stock = 9,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 2,
                    IdCategoria = 1,
                    Marca = "Giant",
                    Modelo = "Talon 3",
                    Precio = 780,
                    Stock = 5,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 3,
                    IdCategoria = 2,
                    Marca = "Specialized",
                    Modelo = "Allez",
                    Precio = 1200,
                    Stock = 4,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 4,
                    IdCategoria = 2,
                    Marca = "Cannondale",
                    Modelo = "Synapse",
                    Precio = 1450,
                    Stock = 2,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 5,
                    IdCategoria = 3,
                    Marca = "GT",
                    Modelo = "Performer BMX",
                    Precio = 450,
                    Stock = 8,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 6,
                    IdCategoria = 4,
                    Marca = "Trek",
                    Modelo = "Verve+ 2",
                    Precio = 2400,
                    Stock = 3,
                    Estado = "DISPONIBLE"
                },

                new Bicicleta
                {
                    IdBicicleta = 7,
                    IdCategoria = 5,
                    Marca = "GW",
                    Modelo = "Kids Bike 20",
                    Precio = 220,
                    Stock = 12,
                    Estado = "DISPONIBLE"
                }
            };


            // =====================================================
            // FILTRO POR MARCA O MODELO
            // =====================================================

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


            // =====================================================
            // FILTRO POR CATEGORÍA
            // =====================================================

            if (categoria.HasValue && categoria.Value > 0)
            {
                bicicletas = bicicletas
                    .Where(b => b.IdCategoria == categoria.Value)
                    .ToList();
            }


            // =====================================================
            // FILTRO POR STOCK
            // =====================================================

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


            // =====================================================
            // GUARDAR FILTROS PARA LA VISTA
            // =====================================================

            ViewBag.Busqueda = busqueda;
            ViewBag.Categoria = categoria;
            ViewBag.StockFiltro = stockFiltro;


            return View(bicicletas);
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
        // RECIBIR FORMULARIO DE CREAR
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Bicicleta bicicleta)
        {
            if (!ModelState.IsValid)
            {
                return View(bicicleta);
            }

            // Posteriormente se realizará el POST hacia la API.

            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // MOSTRAR FORMULARIO DE EDITAR
        // =====================================================

        [HttpGet]
        public IActionResult Edit(int id)
        {
            // Temporal mientras se conecta la API.
            var bicicleta = new Bicicleta
            {
                IdBicicleta = id,
                IdCategoria = 1,
                Marca = "Trek",
                Modelo = "Marlin 5",
                Precio = 850,
                Stock = 9,
                Estado = "DISPONIBLE"
            };

            return View(bicicleta);
        }


        // =====================================================
        // RECIBIR FORMULARIO DE EDITAR
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(Bicicleta bicicleta)
        {
            if (!ModelState.IsValid)
            {
                return View(bicicleta);
            }

            // Posteriormente se realizará el PUT hacia la API.

            return RedirectToAction(nameof(Index));
        }


        // =====================================================
        // MOSTRAR CONFIRMACIÓN DE ELIMINAR
        // =====================================================

        [HttpGet]
        public IActionResult Delete(int id)
        {
            // Temporal mientras se conecta la API.
            var bicicleta = new Bicicleta
            {
                IdBicicleta = id,
                IdCategoria = 1,
                Marca = "Trek",
                Modelo = "Marlin 5",
                Precio = 850,
                Stock = 9,
                Estado = "DISPONIBLE"
            };

            return View(bicicleta);
        }


        // =====================================================
        // CONFIRMAR ELIMINACIÓN
        // =====================================================

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(int id)
        {
            // Posteriormente se realizará el DELETE hacia la API.

            return RedirectToAction(nameof(Index));
        }
    }
}
