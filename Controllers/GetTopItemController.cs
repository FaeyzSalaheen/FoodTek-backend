using Foodtek.DTOs.GetTopItem;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetTopItemController : ControllerBase
    {
        [HttpPost("GetTopItem")]
        public async Task<IActionResult> GetTopItem()
        {
            
                var GetTopItemOutput = new GetTopItemOutput();
            try
            {
                string connectionString = "Data Source=VAGRANT-MC0J25I\\SQLEXPRESS;Initial Catalog=Team13;User Id=admin;Password=Test@1234;Trust Server Certificate=True";

                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(
                    "SELECT TOP 10 [ItemNameEN],[ItemNameAR] ,[ItemImage] ,[ItemDescriptionEN] ,[ItemDescriptionAR]  ,[Price] FROM Item WHERE IsActive = 1 and  ORDER BY Rating DESC"
                    ,
                     connection
                );
                
                command.CommandType = CommandType.Text;
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                   
                GetTopItemOutput.ItemNameEN = dt.Rows[0]["ItemNameEN"].ToString();
                GetTopItemOutput.ItemNameAR = dt.Rows[0]["ItemNameAR"].ToString();
                GetTopItemOutput.ItemImage = dt.Rows[0]["ItemImage"].ToString();
                GetTopItemOutput.ItemDescriptionEN = dt.Rows[0]["ItemDescriptionEN"].ToString();
                GetTopItemOutput.ItemDescriptionAR = dt.Rows[0]["ItemDescriptionAR"].ToString();
                GetTopItemOutput.Price = Convert.ToInt32(dt.Rows[0]["Price"]);


                return Ok(GetTopItemOutput);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }
    }
}
