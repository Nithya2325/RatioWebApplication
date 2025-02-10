using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RatioAPIs_Fetching.Models;
namespace RatioAPIs_Fetching.Controllers
{
    public class ValuesController : ApiController
    {
        private readonly string connectionstring = "Server=DESKTOP-8280AV0\\SQLEXPRESS;Database=PANAYATHIL_2024_25;Integrated Security=True;";

        [HttpGet]
        public IHttpActionResult Get()
        {
            var ratios = new List<RatioModel>();

            try
            {
                using (var con = new SqlConnection(connectionstring))
                {
                    using (var command = new SqlCommand("Ratio_Report", con))
                    {
                        command.CommandType = CommandType.StoredProcedure;
                        con.Open();

                        using (var reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                ratios.Add(new RatioModel
                                {
                                    MonthYear = reader["MonthYear"] != DBNull.Value ? Convert.ToString(reader["MonthYear"]) : string.Empty,
                                    CurrentRatio = reader["CurrentRatio"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentRatio"]) : 0m,
                                    QuickRatio = reader["QuickRatio"] != DBNull.Value ? Convert.ToDecimal(reader["QuickRatio"]) : 0m,
                                    CashRatio = reader["CashRatio"] != DBNull.Value ? Convert.ToDecimal(reader["CashRatio"]) : 0m
                                });
                            }
                        }
                    }
                }           
            }
            catch (Exception ex)
            {

                Console.WriteLine($"Error: {ex.Message}");
                Console.WriteLine($"Stack Trace: {ex.StackTrace}");
            }
            return Ok(ratios);
        }
    }

}
