using System.ComponentModel.DataAnnotations;

namespace BikeStore.Models
{
    public class Bicicleta
    {
        public int IdBicicleta { get; set; }

        [Required(ErrorMessage = "Debe seleccionar una categoría.")]
        [Range(1, int.MaxValue, ErrorMessage = "Debe seleccionar una categoría válida.")]
        public int IdCategoria { get; set; }

        [Required(ErrorMessage = "La marca es obligatoria.")]
        [StringLength(100, ErrorMessage = "La marca no puede superar los 100 caracteres.")]
        public string Marca { get; set; } = string.Empty;

        [Required(ErrorMessage = "El modelo es obligatorio.")]
        [StringLength(100, ErrorMessage = "El modelo no puede superar los 100 caracteres.")]
        public string Modelo { get; set; } = string.Empty;

        [Range(0, 99999999.99, ErrorMessage = "El precio debe ser mayor o igual a 0.")]
        public decimal Precio { get; set; }

        [Range(0, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        [Required(ErrorMessage = "Debe seleccionar un estado.")]
        [RegularExpression(
            "DISPONIBLE|AGOTADO|INACTIVO",
            ErrorMessage = "El estado seleccionado no es válido."
        )]
        public string Estado { get; set; } = string.Empty;
    }
}
