namespace BikeStore.Models
{
    public class RegistrarVenta
    {
        public int IdCliente { get; set; }

        public List<DetalleSolicitud> Detalles { get; set; } = new();
    }

    public class DetalleSolicitud
    {
        public int IdBicicleta { get; set; }

        public int Cantidad { get; set; }
    }
}