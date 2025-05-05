using Foodtek.DTOs.GetAllofferDTOs;
using Foodtek.DTOs.GetCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllofferController : ControllerBase
    {
        [HttpPost("[action]")]
        public async Task<IActionResult> GetAlloffer(GetAllofferInput input) 
        {
            var GetAllofferInputOutput = new GetAllofferInputOutput();
            try
            {
                string connectionString = "Data Source=DESKTOP-TAISUD8\\SQL2017;Initial Catalog=FoodTek;Integrated Security=True;Encrypt=True;Trust Server Certificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(
                    "select TitleAR,TitleEN, [Image] , DescriptionAR , DescriptionEN  from DiscountsOffers where Id =@Id"
                    ,
                     connection
                );
                command.Parameters.AddWithValue("@Id", input.Id);
                command.CommandType = CommandType.Text;
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();
               
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                if (dt.Rows.Count == 0)
                {
                    return NotFound("No category found with the provided ID.");
                }
                GetAllofferInputOutput.TitleEN = dt.Rows[0]["TitleAR"].ToString();
                GetAllofferInputOutput.TitleAR = dt.Rows[0]["TitleEN"].ToString();

                GetAllofferInputOutput.DescriptionAR = dt.Rows[0]["DescriptionAR"].ToString();
                GetAllofferInputOutput.DescriptionEN = dt.Rows[0]["DescriptionEN"].ToString();

                GetAllofferInputOutput.Image = dt.Rows[0]["Image"].ToString();
                return Ok(GetAllofferInputOutput);

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }
    }
}
