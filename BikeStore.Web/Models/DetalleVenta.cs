namespace BikeStore.Models
{
    public class DetalleVenta
    {
        public int IdDetalle { get; set; }

        public int IdVenta { get; set; }

        public int IdBicicleta { get; set; }

        public string Bicicleta { get; set; } = string.Empty;

        public int Cantidad { get; set; }

        public decimal Precio { get; set; }

        public decimal Subtotal { get; set; }
    }
}