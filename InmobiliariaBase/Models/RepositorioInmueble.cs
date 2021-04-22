using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class RepositorioInmueble : RepositorioBase
    {


        public RepositorioInmueble(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Inmueble> ObtenerTodos()
        {
            var res = new List<Inmueble>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT i.Id, Direccion, Tipo, Ambientes, Superficie, Importe, PropietarioId," +
                     " p.Nombre, p.Apellido" +
                     " FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id WHERE i.Estado = 1 AND p.Estado = 1";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Inmueble i = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Superficie = reader.GetInt32(4),
                            Importe = reader.GetInt32(5),
                            PropietarioId = reader.GetInt32(6),

                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8),
                            }

                        };
                        res.Add(i);
                    }
                    connection.Close();
                }
            }
            return res;
        }

        public int Alta(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Inmuebles (Direccion, Tipo, Ambientes, Superficie, Importe, PropietarioId) " +
                "VALUES (@direccion, @tipo, @ambientes, @superficie, @importe, @propietarioId);" +
                "SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@importe", i.Importe);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    i.Id = res;
                    connection.Close();
                }
            }
            return res;

        }

        public Inmueble ObtenerInmueble(int id)
        {
            var inmueble = new Inmueble();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT i.Id, Direccion, Tipo, Ambientes, Superficie, Importe, PropietarioId, p.Nombre, p.Apellido " +
                    $" FROM Inmuebles i INNER JOIN Propietarios p ON i.PropietarioId = p.Id" +
                    $" WHERE i.Id = @id AND i.Estado = 1 AND p.Estado = 1";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        inmueble = new Inmueble
                        {
                            Id = reader.GetInt32(0),
                            Direccion = reader.GetString(1),
                            Tipo = reader.GetString(2),
                            Ambientes = reader.GetInt32(3),
                            Superficie = reader.GetInt32(4),
                            Importe = reader.GetInt32(5),
                            PropietarioId = reader.GetInt32(6),

                            Duenio = new Propietario
                            {
                                Id = reader.GetInt32(6),
                                Nombre = reader.GetString(7),
                                Apellido = reader.GetString(8)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return inmueble;
        }

        public int Modificar(Inmueble i)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET " + "Direccion=@direccion, Tipo=@tipo, Ambientes=@ambientes, Superficie=@superficie, Importe=@importe, PropietarioId=@propietarioId " +
                    "WHERE Id = @id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@direccion", i.Direccion);
                    command.Parameters.AddWithValue("@tipo", i.Tipo);
                    command.Parameters.AddWithValue("@ambientes", i.Ambientes);
                    command.Parameters.AddWithValue("@superficie", i.Superficie);
                    command.Parameters.AddWithValue("@importe", i.Importe);
                    command.Parameters.AddWithValue("@propietarioId", i.PropietarioId);
                    command.Parameters.AddWithValue("@id", i.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();

                }
            }
            return res;
        }

        public int Baja(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Inmuebles SET Estado = 0 WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@id", id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }
    }
}
