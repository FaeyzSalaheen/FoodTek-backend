using Foodtek.DTOs.GetItemDetailsById;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetItemDetailsController : ControllerBase
    {
        [HttpPost("GetItemDetailsById")]
        public async Task<IActionResult> GetItemDetails(GetItemDetailsByIdInput input)
        {
            var output = new GetItemDetailsByIdOutPut();

            try
            {
                string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    SqlCommand command = new SqlCommand(
                        @"SELECT 
                                i.Id,
                                i.ItemNameEN AS Name,
                                i.ItemDescriptionEN AS Description,
                                i.Price,
                                0 AS Rate,             
                                0 AS NumberOfReview       
                            FROM 
                                [FoodTek].[dbo].[Item] i
                            WHERE 
                                i.Id = @Id; ",
                        connection
                    );
                    command.Parameters.AddWithValue("@Id", input.Id);
                    command.CommandType = CommandType.Text;

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);

                    if (dt.Rows.Count == 0)
                    {
                        return NotFound("No item found with the provided ID.");
                    }

                    output.Name = dt.Rows[0]["Name"].ToString();
                    output.Description = dt.Rows[0]["Description"].ToString();
                    output.Price = Convert.ToDecimal(dt.Rows[0]["Price"]);
                    output.Rate = Convert.ToDouble(dt.Rows[0]["Rate"]);
                    output.NumberOfReview = Convert.ToInt32(dt.Rows[0]["NumberOfReview"]);

                    return Ok(output);
                }
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }
    }
}
