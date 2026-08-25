namespace BikeStore.Application.DTOs.Ventas
{
    public class RegistrarVentaDto
    {
        public int IdCliente { get; set; }
        public List<DetalleSolicitudDto> Detalles { get; set; } = new();
    }

    public class DetalleSolicitudDto
    {
        public int IdBicicleta { get; set; }
        public int Cantidad { get; set; }
    }
}