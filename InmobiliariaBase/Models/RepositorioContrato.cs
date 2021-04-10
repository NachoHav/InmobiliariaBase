using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class RepositorioContrato
    {
        private readonly string connectionString;

        private readonly IConfiguration configuration;

        public RepositorioContrato(IConfiguration configuration)
        {
            connectionString = configuration["ConnectionStrings:DefaultConnection"];
            this.configuration = configuration;
        }

        public List<Contrato> ObtenerTodos()
        {
            var contratos = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, Estado, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo " +
                   $" FROM Contratos c " +
                   $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id " +
                   $"INNER JOIN Inmuebles inm ON inm.InmuebleId = p.Id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Contrato contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            Estado = reader.GetBoolean(3),
                            InquilinoId = reader.GetInt32(4),
                            InmuebleId = reader.GetInt32(5),

                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                                Dni = reader.GetString(8)
                            },

                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(5),
                                Direccion = reader.GetString(9),
                                Tipo = reader.GetString(10)
                            }

                        };
                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

        public int Alta(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Contratos (FechaDesde, FechaHasta, Estado, InquilinoId, InmuebleId ) " +
                "VALUES (@fechaDesde, @fechaHasta, @estado, @inquilinoId, @inmuebleId);" +
                "SELECT SCOPE_IDENTITY();";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaDesde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@estado", c.Estado);
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);

                    connection.Open();
                    res = Convert.ToInt32(command.ExecuteScalar());
                    c.Id = res;
                    connection.Close();
                }
            }
            return res;

        }

        public Contrato ObtenerContrato(int id)
        {
            var contrato = new Contrato();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, Estado, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo " +
                    $" FROM Contratos c " +
                    $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id " +
                    $"INNER JOIN Inmuebles inm ON inm.InmuebleId = p.Id" +
                    $" WHERE c.Id = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    if (reader.Read())
                    {
                        contrato = new Contrato
                        {
                            Id = reader.GetInt32(0),
                            FechaDesde = reader.GetDateTime(1),
                            FechaHasta = reader.GetDateTime(2),
                            Estado = reader.GetBoolean(3),
                            InquilinoId = reader.GetInt32(4),
                            InmuebleId = reader.GetInt32(5),

                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(4),
                                Nombre = reader.GetString(6),
                                Apellido = reader.GetString(7),
                                Dni = reader.GetString(8)
                            },

                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(5),
                                Direccion = reader.GetString(9),
                                Tipo = reader.GetString(10)
                            }
                        };
                    }
                    connection.Close();
                }
            }
            return contrato;
        }

        public int Modificar(Contrato c)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contratos SET " + "FechaDesde=@fechaDesde, FechaHasta=@fechaHasta, Estado=@estado, InquilinoId=@inquilinoId, InmuebleId=@inmuebleId " +
                    "WHERE Id = @id";


                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaDesde", c.FechaDesde);
                    command.Parameters.AddWithValue("@fechaHasta", c.FechaHasta);
                    command.Parameters.AddWithValue("@estado", c.Estado);
                    command.Parameters.AddWithValue("@inquilinoId", c.InquilinoId);
                    command.Parameters.AddWithValue("@inmuebleId", c.InmuebleId);
   
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
                string sql = $"DELETE FROM Contratos WHERE Id = @id";
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

