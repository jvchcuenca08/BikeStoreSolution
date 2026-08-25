using BikeStore.Domain;
using BikeStore.Domain.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;

namespace BikeStore.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BicicletasController : ControllerBase
    {
        private readonly string _connectionString;

        public BicicletasController(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("BikeStoreConnection") ?? "";
        }

        // GET: api/bicicletas
        // Permite consultar todas las bicicletas y opcionalmente filtrar por categoría, marca o stock bajo
        [HttpGet]
        public IActionResult GetBicicletas([FromQuery] int? idCategoria, [FromQuery] string? marca, [FromQuery] bool? stockBajo)
        {
            List<Bicicleta> lista = new List<Bicicleta>();

            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "SELECT IdBicicleta, IdCategoria, Marca, Modelo, Precio, Stock, Estado FROM Bicicleta WHERE 1=1";

                if (idCategoria.HasValue) query += " AND IdCategoria = @IdCategoria";
                if (!string.IsNullOrEmpty(marca)) query += " AND Marca LIKE @Marca";
                if (stockBajo.HasValue && stockBajo.Value) query += " AND Stock <= 3"; // Criterio de stock bajo

                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    if (idCategoria.HasValue) cmd.Parameters.AddWithValue("@IdCategoria", idCategoria.Value);
                    if (!string.IsNullOrEmpty(marca)) cmd.Parameters.AddWithValue("@Marca", "%" + marca + "%");

                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            lista.Add(new Bicicleta
                            {
                                IdBicicleta = Convert.ToInt32(dr["IdBicicleta"]),
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Marca = dr["Marca"].ToString() ?? "",
                                Modelo = dr["Modelo"].ToString() ?? "",
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                Estado = dr["Estado"].ToString() ?? "DISPONIBLE"
                            });
                        }
                    }
                }
            }
            return Ok(lista);
        }

        // GET: api/bicicletas/5
        [HttpGet("{id}")]
        public IActionResult GetBicicleta(int id)
        {
            Bicicleta? bici = null;
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "SELECT IdBicicleta, IdCategoria, Marca, Modelo, Precio, Stock, Estado FROM Bicicleta WHERE IdBicicleta = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        if (dr.Read())
                        {
                            bici = new Bicicleta
                            {
                                IdBicicleta = Convert.ToInt32(dr["IdBicicleta"]),
                                IdCategoria = Convert.ToInt32(dr["IdCategoria"]),
                                Marca = dr["Marca"].ToString() ?? "",
                                Modelo = dr["Modelo"].ToString() ?? "",
                                Precio = Convert.ToDecimal(dr["Precio"]),
                                Stock = Convert.ToInt32(dr["Stock"]),
                                Estado = dr["Estado"].ToString() ?? "DISPONIBLE"
                            };
                        }
                    }
                }
            }
            if (bici == null) return NotFound(new { mensaje = "Bicicleta no encontrada" });
            return Ok(bici);
        }

        // POST: api/bicicletas
        [HttpPost]
        public IActionResult PostBicicleta([FromBody] Bicicleta bicicleta)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "INSERT INTO Bicicleta (IdCategoria, Marca, Modelo, Precio, Stock, Estado) VALUES (@IdCat, @Marca, @Modelo, @Precio, @Stock, @Estado)";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@IdCat", bicicleta.IdCategoria);
                    cmd.Parameters.AddWithValue("@Marca", bicicleta.Marca);
                    cmd.Parameters.AddWithValue("@Modelo", bicicleta.Modelo);
                    cmd.Parameters.AddWithValue("@Precio", bicicleta.Precio);
                    cmd.Parameters.AddWithValue("@Stock", bicicleta.Stock);
                    cmd.Parameters.AddWithValue("@Estado", string.IsNullOrEmpty(bicicleta.Estado) ? "DISPONIBLE" : bicicleta.Estado);
                    cmd.ExecuteNonQuery();
                }
            }
            return StatusCode(StatusCodes.Status201Created, new { mensaje = "Bicicleta registrada con éxito" });
        }

        // PUT: api/bicicletas/5
        [HttpPut("{id}")]
        public IActionResult PutBicicleta(int id, [FromBody] Bicicleta bicicleta)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "UPDATE Bicicleta SET IdCategoria = @IdCat, Marca = @Marca, Modelo = @Modelo, Precio = @Precio, Stock = @Stock, Estado = @Estado WHERE IdBicicleta = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    cmd.Parameters.AddWithValue("@IdCat", bicicleta.IdCategoria);
                    cmd.Parameters.AddWithValue("@Marca", bicicleta.Marca);
                    cmd.Parameters.AddWithValue("@Modelo", bicicleta.Modelo);
                    cmd.Parameters.AddWithValue("@Precio", bicicleta.Precio);
                    cmd.Parameters.AddWithValue("@Stock", bicicleta.Stock);
                    cmd.Parameters.AddWithValue("@Estado", bicicleta.Estado);

                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas == 0) return NotFound(new { mensaje = "Bicicleta no encontrada para actualizar" });
                }
            }
            return Ok(new { mensaje = "Bicicleta actualizada con éxito" });
        }

        // DELETE: api/bicicletas/5
        [HttpDelete("{id}")]
        public IActionResult DeleteBicicleta(int id)
        {
            using (SqlConnection conexion = new SqlConnection(_connectionString))
            {
                conexion.Open();
                string query = "DELETE FROM Bicicleta WHERE IdBicicleta = @Id";
                using (SqlCommand cmd = new SqlCommand(query, conexion))
                {
                    cmd.Parameters.AddWithValue("@Id", id);
                    int filasAfectadas = cmd.ExecuteNonQuery();
                    if (filasAfectadas == 0) return NotFound(new { mensaje = "Bicicleta no encontrada para eliminar" });
                }
            }
            return Ok(new { mensaje = "Bicicleta eliminada con éxito" });
        }
    }
}