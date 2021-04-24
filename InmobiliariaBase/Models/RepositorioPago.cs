using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Threading.Tasks;

namespace InmobiliariaBase.Models
{
    public class RepositorioPago : RepositorioBase
    {
        public RepositorioPago(IConfiguration configuration) : base(configuration)
        {
                
        }


        public int Alta(Pago pago)
        {
            int res = -1;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"INSERT INTO Pagos (ContratoId, FechaPago, Importe)" +
                             $"VALUES (@contratoId, @fechaPago, @importe)" +
                             "SELECT SCOPE_IDENTITY();"; 

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;


                    command.Parameters.AddWithValue("@contratoId", pago.IdContrato);
                    command.Parameters.AddWithValue("@fechaPago", pago.FechaPago);
                    command.Parameters.AddWithValue("@importe", pago.Importe);


                    connection.Open();

                    res = Convert.ToInt32(command.ExecuteScalar()); 
                    pago.Id = res;

                    connection.Close();
                }
            }
            return res;
        }




        public List<Pago> ObtenerTodos(int id)
        {
            var res = new List<Pago>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = "SELECT p.Id, ContratoId, FechaPago, p.Importe," +
                     " i.Id , i.Importe, inq.Nombre, inq.Apellido" +
                     " FROM Pagos p INNER JOIN Contratos c ON p.ContratoId = c.Id INNER JOIN Inmuebles i ON c.InmuebleId = i.Id INNER JOIN Inquilinos inq ON inq.Id = c.InquilinoId WHERE c.id = @id AND p.Estado = 1";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    command.Parameters.Add("@id", SqlDbType.Int).Value = id;
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),
                            Importe = reader.GetInt32(3),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(1),

                                Inmueble = new Inmueble
                                {
                                    Id = reader.GetInt32(4),
                                    Importe = reader.GetInt32(5)
                                },

                                Inquilino = new Inquilino
                                {
                                    Nombre = reader.GetString(6),
                                    Apellido = reader.GetString(7)
                                }
                            }

                           

                        };
                        res.Add(p);
                    }
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
                string sql = $"UPDATE Pagos SET Estado = 0 WHERE Id = @id";
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

        public int Modificar(Pago p)
        {
            int res = -1;
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                string sql = $"UPDATE Pagos SET FechaPago=@fechaPago, ContratoId=@contratoId, Estado=@estado," +
                    $"WHERE Id = @id";
                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    command.Parameters.AddWithValue("@fechaPago", p.FechaPago);
                    command.Parameters.AddWithValue("@apellido", p.IdContrato);
                    command.Parameters.AddWithValue("@dni", p.Estado);
                    command.Parameters.AddWithValue("@id", p.Id);
                    connection.Open();
                    res = command.ExecuteNonQuery();
                    connection.Close();
                }
            }
            return res;
        }

    }
}
