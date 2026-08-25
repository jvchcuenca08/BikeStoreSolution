using BikeStore.Application.DTOs.Ventas;
using BikeStore.Application.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace BikeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VentasController : ControllerBase
    {
        private readonly IVentaRepository _ventaRepository;

        public VentasController(IVentaRepository ventaRepository)
        {
            _ventaRepository = ventaRepository;
        }

        [HttpPost]
        public async Task<IActionResult> RegistrarVenta([FromBody] RegistrarVentaDto dto)
        {
            try
            {
                var resultado = await _ventaRepository.RegistrarVentaAsync(dto);
                return Ok(resultado);
            }
            catch (Exception ex)
            {
                var mensajeError = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                return BadRequest(new { mensaje = mensajeError });
            }
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerHistorial()
        {
            var historial = await _ventaRepository.ObtenerHistorialAsync();
            return Ok(historial);
        }

        [HttpGet("cliente/{idCliente}")]
        public async Task<IActionResult> ObtenerPorCliente(int idCliente)
        {
            var ventas = await _ventaRepository.ObtenerPorClienteAsync(idCliente);
            return Ok(ventas);
        }
    }
}