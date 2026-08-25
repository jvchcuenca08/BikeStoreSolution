namespace BikeStore.Application.DTOs.Ventas
{
    public class ResumenVentaDto
    {
        public int IdVenta { get; set; }
        public DateTime Fecha { get; set; }
        public int IdCliente { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Iva { get; set; }
        public decimal Total { get; set; }
        public List<DetalleResumenDto> Detalles { get; set; } = new();
    }

    public class DetalleResumenDto
    {
        public int IdBicicleta { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
    }
}