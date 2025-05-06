using Foodtek.DTOs.GetItemByCategoryId;
using Foodtek.DTOs.GetTopItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetItemByCategoryIdController : ControllerBase
    {
      
            [HttpPost("GetTopItem")]
            public async Task<IActionResult> GetItemByCategoryId(Input   input)
            {

                var Output = new Output();
                try
                {
                //string connectionString = "Data Source=VAGRANT-MC0J25I\\SQLEXPRESS;Initial Catalog=Team13;User Id=admin;Password=Test@1234;Trust Server Certificate=True";

                string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand(
                        "SELECT TOP 10 [ItemNameEN],[ItemNameAR] ,[ItemImage] ,[ItemDescriptionEN] ,[ItemDescriptionAR]  ,[Price] FROM Item WHERE IsActive = 1 and CategoryId =@CategoryId " //ORDER BY Rating DESC"
                        ,
                         connection
                    );
                    command.Parameters.AddWithValue("@CategoryId", input.CategoryId);
                    command.CommandType = CommandType.Text;
                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                    if (dt.Rows.Count == 0)
                    {
                        return NotFound("Item Not found with the provided ID.");
                    }
                       Output.ItemNameEN = dt.Rows[0]["ItemNameEN"].ToString();
                       Output.ItemNameAR = dt.Rows[0][$"ItemNameAR"].ToString();
                       Output.ItemImage = dt.Rows[0]["ItemImage"].ToString();
                       Output.ItemDescriptionEN = dt.Rows[0]["ItemDescriptionEN"].ToString();
                       Output.ItemDescriptionAR = dt.Rows[0]["ItemDescriptionAR"].ToString();
                       Output.Price = Convert.ToInt32(dt.Rows[0]["Price"]);


                return Ok(new
                {
                    message = "Item fetched successfully!",
                    data = Output
                });

            }
                catch (Exception ex)
                {
                    return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
                }

            }
        }
    }

