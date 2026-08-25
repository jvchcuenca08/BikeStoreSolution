using System;
using System.Collections.Generic;
using System.Text;

namespace BikeStore.Domain
{
    public class Bicicleta



    {

        public int IdBicicleta { get; set; }
        public int IdCategoria { get; set; }
        public string Marca { get; set; } = string.Empty;
        public string Modelo { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public string Estado { get; set; } = "DISPONIBLE";
    }
}
