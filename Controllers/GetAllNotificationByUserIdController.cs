using Foodtek.DTOs.GetAllNotificationByUserId;
using Foodtek.DTOs.GetItemByCategoryId;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Data;
using static System.Runtime.InteropServices.JavaScript.JSType;
using System.Threading;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GetAllNotificationByUserIdController : ControllerBase
    {
  
        [HttpPost(" GetAllNotificationByUserId")]
        public async Task<IActionResult> GetAllNotificationByUserId(GetNotificationInput input)
        {

            var OutPut = new OutPut();
            try
            {

                string connectionString = "Data Source=VAGRANT-MC0J25I\\SQLEXPRESS;Initial Catalog=Team13;User Id=admin;Password=Test@1234;Trust Server Certificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                SqlCommand command = new SqlCommand(
                "SELECT n.Id, n.Title, n.Description AS Content, n.CreationDate AS[Date], un.is_read AS IsRead FROM Notifications n JOIN user_notifications un ON n.Id = un.notification_id WHERE un.user_id =@user_id ORDER BY n.CreationDate DESC;"
                    ,
                     connection
                );
                command.Parameters.AddWithValue("@user_id", input.Id);
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
                OutPut.Title = dt.Rows[0]["Title"].ToString();
                OutPut.Content = dt.Rows[0]["Content"].ToString();
                OutPut.Date = dt.Rows[0]["Date"].ToString();
                OutPut.IsRead = Convert.ToBoolean(dt.Rows[0]["IsRead"]);



                return Ok(new
                {
                    message = "Item fetched successfully!",
                    data = OutPut
                });

            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Internal server error: {ex.Message}");
            }

        }
    }
}

