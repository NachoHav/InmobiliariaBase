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
                string sql = $"INSERT INTO Pagos (ContratoId, FechaPago)" +
                             $"VALUES (@contratoId, @fechaPago)" +
                             "SELECT SCOPE_IDENTITY();"; 

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;


                    command.Parameters.AddWithValue("@contratoId", pago.IdContrato);
                    command.Parameters.AddWithValue("@fechaPago", pago.FechaPago);


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
                string sql = "SELECT p.Id, ContratoId, FechaPago," +
                     " i.Importe" +
                     " FROM Pagos p INNER JOIN Contratos c ON p.ContratoId = c.Id INNER JOIN Inmuebles i ON c.InmuebleId = i.Id WHERE c.id = @id";

                using (SqlCommand command = new SqlCommand(sql, connection))
                {
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    var reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        Pago p = new Pago
                        {
                            Id = reader.GetInt32(0),
                            IdContrato = reader.GetInt32(1),
                            FechaPago = reader.GetDateTime(2),

                            Contrato = new Contrato
                            {
                                Id = reader.GetInt32(2),

                                Inmueble = new Inmueble
                                {
                                   Id = reader.GetInt32(3),
                                   Importe = reader.GetInt32(4)
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
    }
}
