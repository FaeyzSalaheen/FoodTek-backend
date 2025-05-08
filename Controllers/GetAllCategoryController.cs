using Foodtek.DTOs.GetCategory;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllCategoryController : ControllerBase
    {

        [HttpPost("[action]")]
        public async Task<IActionResult> GetAllCategory(GetCategoryInput input)
        {
           
            try
            {
                var GetCategoryOutput = new GetCategoryOutput();


                string connectionString = "Data Source=VAGRANT-MC0J25I\\SQLEXPRESS;Initial Catalog=Team13;User Id=admin;Password=Test@1234;Trust Server Certificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(
                    "select NameEN, NameAR , [Image] from Categories  where Id =@Id and IsActive = 1"
                    ,
                     connection
                );

                command.Parameters.AddWithValue("@Id", input.Id);                
                command.CommandType = CommandType.Text;
                connection.Open();
                int result = command.ExecuteNonQuery();
                connection.Close();

                if (result == 0)
                {
                    return NotFound("No category found with the provided ID.");
                }
                SqlDataAdapter adapter = new SqlDataAdapter(command);
                DataTable dt = new DataTable();
                adapter.Fill(dt);

                if (dt.Rows.Count == 0)
                {
                    return NotFound("No category found with the provided ID.");
                }
                GetCategoryOutput.NameEN = dt.Rows[0]["NameEN"].ToString();
                GetCategoryOutput.NameAR = dt.Rows[0]["NameAR"].ToString();
                GetCategoryOutput.Image = dt.Rows[0]["Image"].ToString();

                return Ok(GetCategoryOutput);
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }
        }

    }
}
