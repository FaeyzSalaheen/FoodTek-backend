using Foodtek.DTOs;
using Foodtek.DTOs.Restpassword.RestpasswordInput;
using Foodtek.DTOs.SignIn.SignInInput;
using Foodtek.DTOs.SignIn.SignInOutput;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;
using MyTasks.Helpers.Validations;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;

using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
              

        [HttpPost]
        [Route("[action]")]
        public async Task<IActionResult> SignUp(SignUpInputDTO input)
        {
            try
            {
                if (!(ValidationHelper.IsValidEmail(input.Email) || ValidationHelper.IsValidPassword(input.password)))
                         throw new Exception("Invalid Email or Password");
                

                {

                    string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                    SqlCommand command = new SqlCommand(
                    "INSERT INTO Users (FullName, Email, Password, Username, CreatedBy, UpdatedBy, birthdate, role, PhoneNumber, CreatedAt, UpdatedAt) " +
                    "VALUES (@FullName, @Email, @Password, @Username, @CreatedBy, @UpdatedBy, @birthdate, @role, @PhoneNumber, @CreatedAt, @UpdatedAt)",
                     connection
                      );

                    command.Parameters.AddWithValue("@FullName", input.fullname);
                    command.Parameters.AddWithValue("@Email", input.Email);
                    command.Parameters.AddWithValue("@PhoneNumber", input.PhoneNumber);
                    command.Parameters.AddWithValue("@Password", input.password);
                    command.Parameters.AddWithValue("@Username", input.Username);
                    command.Parameters.AddWithValue("@CreatedBy", "system");
                    command.Parameters.AddWithValue("@UpdatedBy", "system");
                    command.Parameters.AddWithValue("@birthdate", input.birthdate);
                    command.Parameters.AddWithValue("@CreatedAt", input.CreatedAt);
                    command.Parameters.AddWithValue("@UpdatedAt", input.UpdatedAt);
                    command.Parameters.AddWithValue("@role", input.role);


                    command.CommandType = CommandType.Text;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();
                
                    if (result > 0) 
                        return StatusCode(201, "Account Created Successfully");
                       else
                            return StatusCode(400, "Failed to create account");
                    
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An Error Was Occurred: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(SignInInputDTO input)
        {

            var SignInOutput = new SignInOutputDTO();
            try
            {
                if (!ValidationHelper.IsValidEmail(input.Email) || !ValidationHelper.IsValidPassword(input.password))
                    throw new Exception("Invalid Email or Password");


                string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                string query = $"SELECT Id ,fullname  FROM Users WHERE Email = @Email AND Password =@password";
                SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", input.Email);
                    command.Parameters.AddWithValue("@Password", input.password);
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                //otp 
                   
                if(dt.Rows.Count == 0 )
                        throw new Exception("Invalid Email or Password");
                    
                if (dt.Rows.Count >1 ) 
                        throw new Exception("Somthing wrong");

                foreach (DataRow row in dt.Rows)
                {
                    SignInOutput.Id= Convert.ToInt32( row["Id"]);

                    SignInOutput.Name = Convert.ToString(row["fullname"]);
                    }
                    return Ok (SignInOutput);   
                
                // resopns must be JWT
            }
            catch (Exception ex)
            {

                return StatusCode(500, $"An Error Was Occured {ex.Message}");
            }
        }
        [HttpPost("restpassword")]
        public async Task<IActionResult> restpassword(RestPasswordInputDTO input)
        {
            try
            {
                if (!ValidationHelper.IsValidPassword(input.Password)|| input.Password != input.ConfirmPassword)
                    return BadRequest(new { Message = "Invalid Password or Confirm Password" });

                string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                SqlConnection connection = new SqlConnection(connectionString);
                string query = $"UPDATE Users SET Password = @Password WHERE Email = @Email";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Email", input.Email);
                command.Parameters.AddWithValue("@Password", input.Password);
                connection.Open();
                command.CommandType = CommandType.Text;
                int result = command.ExecuteNonQuery();
                connection.Close();
                if (result == 0)
                    return StatusCode(400, "Failed to update password");
                if (result > 1)
                    return StatusCode(400, "Somthing wrong");
                if (result == 1)
                    return StatusCode(200, "Password Updated Successfully");







                return Ok(new { Message = "Password Updated Successfully" });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = $"An error has been detected: {ex.Message}" });
            }
        }
       
       



    }
}
