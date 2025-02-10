using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using RatioAPIs_Fetching.Models;
namespace RatioAPIs_Fetching.Controllers
{
    [RoutePrefix("api/ratios")]
    public class RatioController : ApiController
    {
 
        [HttpPost]
        [Route("GetRatios")]
        public IHttpActionResult GetRatios([FromBody] RatioRequest request)
        {
            //if (request == null || request.Branches == null || request.Branches.Count == 0)
            //    return BadRequest("Invalid input data.");

            List<RatioResult> ratios = new List<RatioResult>();

            string connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();

                using (SqlCommand command = new SqlCommand("Ratio_Report", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    // Add the parameters
                    command.Parameters.AddWithValue("@Passing_Date", request.PassingDate);
                    command.Parameters.Add(CreateBranchTableParameter(request.Branches));

                    // Execute the stored procedure
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            ratios.Add(new RatioResult
                            {
                                MonthYear = reader["MonthYear"].ToString(),
                                CurrentRatio = reader["CurrentRatio"] != DBNull.Value ? Convert.ToDecimal(reader["CurrentRatio"]) : 0,
                                QuickRatio = reader["QuickRatio"] != DBNull.Value ? Convert.ToDecimal(reader["QuickRatio"]) : 0,
                                CashRatio = reader["CashRatio"] != DBNull.Value ? Convert.ToDecimal(reader["CashRatio"]) : 0
                            });
                        }
                    }
                }
            }

            return Ok(ratios);
        }
    }
