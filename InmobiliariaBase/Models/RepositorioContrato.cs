using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class RepositorioContrato : RepositorioBase
    {
        public RepositorioContrato(IConfiguration configuration) : base(configuration)
        {

        }

        public List<Contrato> ObtenerTodos()
        {
            var contratos = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo , inm.Importe, c.Estado" +
                   $" FROM Contratos c " +
                   $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id AND i.Estado = 1 " +
                   $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id AND inm.Estado = 1 " +
                   $"INNER JOIN Propietarios propietario ON inm.PropietarioId = propietario.Id AND propietario.Estado = 1 " +
                   $"WHERE c.Estado = 1";

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
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),
                            Estado = reader.GetBoolean(11),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                                Dni = reader.GetString(7)
                            },

                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Importe = reader.GetInt32(10)
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
                    command.Parameters.AddWithValue("@estado", 1);
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
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, c.Estado, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo, inm.Importe " +
                    $"FROM Contratos c " +
                    $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id AND i.Estado = 1 " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id AND inm.Estado = 1 " +

                    $" WHERE c.Id = @id AND c.Estado = 1";

                //$"INNER JOIN Propietarios propietario ON inm.PropietarioId = propietario.Id AND propietario.Estado = 1" +

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
                                Tipo = reader.GetString(10),
                                Importe = reader.GetInt32(11),
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
                    command.Parameters.AddWithValue("@id", c.Id);
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
                string sql = $"UPDATE Contratos SET Estado = 0 WHERE Id = @id";
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

        public int Activar(int id)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Contratos SET Estado = 1 WHERE Id = @id";
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




        public List<Contrato> ObtenerPorInmueble(int id)
        {
            List<Contrato> res = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, c.InquilinoId, c.InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo , inm.Importe" +
                   $" FROM Contratos c " +
                    $"INNER JOIN Inquilinos i ON c.InquilinoId = i.id " +
                    $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.id " +
                    $"WHERE InmuebleId = @id AND c.Estado = 1;";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
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
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),

                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                                Dni = reader.GetString(7),
                            },
                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Importe = reader.GetInt32(10),
                            }
                        };
                        res.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return res;
        }


        public List<Contrato> ObtenerVigentes()
        {
            var contratos = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo , inm.Importe, c.Estado" +
                   $" FROM Contratos c " +
                   $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id AND i.Estado = 1 " +
                   $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id AND inm.Estado = 1 " +
                   $"INNER JOIN Propietarios propietario ON inm.PropietarioId = propietario.Id AND propietario.Estado = 1 " +
                   $"WHERE c.Estado = 1 AND FechaHasta >= GETDATE()";

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
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),
                            Estado = reader.GetBoolean(11),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                                Dni = reader.GetString(7)
                            },

                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Importe = reader.GetInt32(10)
                            }

                        };
                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return contratos;
        }

        public List<Contrato> ObtenerBajas()
        {
            var contratos = new List<Contrato>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"SELECT c.Id, FechaDesde, FechaHasta, InquilinoId, InmuebleId, i.Nombre, i.Apellido, i.Dni, inm.Direccion, inm.Tipo , inm.Importe, c.Estado" +
                   $" FROM Contratos c " +
                   $"INNER JOIN Inquilinos i ON c.InquilinoId = i.Id AND i.Estado = 1 " +
                   $"INNER JOIN Inmuebles inm ON c.InmuebleId = inm.Id AND inm.Estado = 1 " +
                   $"INNER JOIN Propietarios propietario ON inm.PropietarioId = propietario.Id AND propietario.Estado = 1 " +
                   $"WHERE c.Estado = 0";

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
                            InquilinoId = reader.GetInt32(3),
                            InmuebleId = reader.GetInt32(4),
                            Estado = reader.GetBoolean(11),
                            Inquilino = new Inquilino
                            {
                                Id = reader.GetInt32(3),
                                Nombre = reader.GetString(5),
                                Apellido = reader.GetString(6),
                                Dni = reader.GetString(7)
                            },

                            Inmueble = new Inmueble
                            {
                                Id = reader.GetInt32(4),
                                Direccion = reader.GetString(8),
                                Tipo = reader.GetString(9),
                                Importe = reader.GetInt32(10)
                            }

                        };
                        contratos.Add(contrato);
                    }
                    connection.Close();
                }
            }
            return contratos;
        }


    }
}

