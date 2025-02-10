using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Threading.Tasks;
using System.Web.Http;
using RatioWebApplication.Models;
using RatioWebApplication.Extensions; 

namespace RatioWebApplication.Controllers
{
    public class RatioController : ApiController
    {
        private readonly string connectionstring = "Server=DESKTOP-8280AV0\\SQLEXPRESS;Database=PANAYATHIL_2024_25;Integrated Security=True;";

        [HttpGet]
        [Route("api/ratio")]
        public async Task<IHttpActionResult> GetRatio()
        {
            var ratios = new List<RatioModel>();

            try
            {
                using (var con = new SqlConnection(connectionstring))
                {
                    using (var command = new SqlCommand("Ratio_Report", con))
                    {
                        command.CommandType = System.Data.CommandType.StoredProcedure; // Ensure this is set for a stored procedure
                        await con.OpenAsync();
                        using (var reader = await command.ExecuteReaderAsync())
                        {
                            do
                            {
                                while (await reader.ReadAsync())
                                {
                                    ratios.Add(new RatioModel
                                    {
                                        MonthYear = reader.HasColumn("MonthYear") && reader["MonthYear"] != DBNull.Value
                                            ? Convert.ToString(reader["MonthYear"]) : null,

                                        CurrentRatio = reader.HasColumn("CurrentRatio") && reader["CurrentRatio"] != DBNull.Value
                                            ? (decimal?)Convert.ToDecimal(reader["CurrentRatio"]) : null,

                                        QuickRatio = reader.HasColumn("QuickRatio") && reader["QuickRatio"] != DBNull.Value
                                            ? (decimal?)Convert.ToDecimal(reader["QuickRatio"]) : null,

                                        CashRatio = reader.HasColumn("CashRatio") && reader["CashRatio"] != DBNull.Value
                                            ? (decimal?)Convert.ToDecimal(reader["CashRatio"]) : null
                                    });
                                }
                            } while (await reader.NextResultAsync());
                        }
                    }
                }

                return Ok(ratios);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return InternalServerError(ex);
            }
        }
    }
}
