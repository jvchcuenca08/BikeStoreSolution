using Microsoft.AspNetCore.Mvc;
using BikeStore.Models;

namespace BikeStore.Controllers
{
    public class VentasController : Controller
    {
        // =====================================================
        // HISTORIAL DE VENTAS - TEMPORAL
        // =====================================================

        [HttpGet]
        public IActionResult Index()
        {
            var ventas = new List<Venta>
            {
                new Venta
                {
                    IdVenta = 1,
                    Fecha = DateTime.Now.AddDays(-2),
                    IdCliente = 1,
                    Cliente = "Carlos Pérez",
                    Subtotal = 850.00m,
                    Iva = 127.50m,
                    Total = 977.50m
                },

                new Venta
                {
                    IdVenta = 2,
                    Fecha = DateTime.Now.AddDays(-1),
                    IdCliente = 2,
                    Cliente = "María López",
                    Subtotal = 1200.00m,
                    Iva = 180.00m,
                    Total = 1380.00m
                },

                new Venta
                {
                    IdVenta = 3,
                    Fecha = DateTime.Now,
                    IdCliente = 3,
                    Cliente = "Luis Andrade",
                    Subtotal = 450.00m,
                    Iva = 67.50m,
                    Total = 517.50m
                }
            };

            return View(ventas);
        }
        // =====================================================
        // MOSTRAR FORMULARIO DE NUEVA VENTA
        // =====================================================

        [HttpGet]
        public IActionResult Create()
        {
            ViewBag.Clientes = new List<Cliente>
    {
        new Cliente
        {
            IdCliente = 1,
            Cedula = "1002003001",
            Nombres = "Carlos Andrés",
            Apellidos = "Pérez López"
        },

        new Cliente
        {
            IdCliente = 2,
            Cedula = "1002003002",
            Nombres = "María Fernanda",
            Apellidos = "Gómez Ruiz"
        },

        new Cliente
        {
            IdCliente = 3,
            Cedula = "1002003003",
            Nombres = "Luis Alberto",
            Apellidos = "Torres Mina"
        }
    };

            ViewBag.Bicicletas = new List<Bicicleta>
    {
        new Bicicleta
        {
            IdBicicleta = 1,
            Marca = "Trek",
            Modelo = "Marlin 5",
            Precio = 850.00m,
            Stock = 10
        },

        new Bicicleta
        {
            IdBicicleta = 2,
            Marca = "Giant",
            Modelo = "Talon",
            Precio = 1200.00m,
            Stock = 5
        },

        new Bicicleta
        {
            IdBicicleta = 3,
            Marca = "Specialized",
            Modelo = "Rockhopper",
            Precio = 950.00m,
            Stock = 8
        }
    };

            return View();
        }
    }
}