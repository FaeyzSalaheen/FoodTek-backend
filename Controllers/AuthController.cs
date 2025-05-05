using Foodtek.DTOs;
using Foodtek.DTOs.Login.Response;
using Foodtek.DTOs.Restpassword.RestpasswordInput;
using Foodtek.DTOs.SignIn.SignInInput;
using Foodtek.DTOs.SignIn.SignInOutput;
using Foodtek.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR.Protocol;
using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using MyTasks.Helpers.Validations;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Data.Common;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Net;
using System.Security.Claims;
using System.Text;
using static System.Runtime.InteropServices.JavaScript.JSType;
using Foodtek.DTOs.OTP;
using Microsoft.Extensions.Caching.Memory;
using static System.Net.WebRequestMethods;

namespace Foodtek.Controllers
{
    [Route("api/[controller]")]
    [ApiController]

    public class AuthController : ControllerBase
  {

        private readonly IConfiguration _configuration;
        private readonly IMemoryCache _cache;
        public AuthController(IConfiguration configuration, IMemoryCache cache)
        {
            _configuration = configuration;
            _cache = cache;
        }

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
                command.Parameters.AddWithValue("@role", 1);
                command.CommandType = CommandType.Text;

                    connection.Open();
                    int result = command.ExecuteNonQuery();
                    connection.Close();

                if (result > 0)
                {
                    

                    return StatusCode(201,new
                    {
                    
                    });
                }
                else
                {
                    return StatusCode(400, "Failed to create account");
                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"An error occurred: {ex.Message}");
            }
        }

        [HttpPost("[action]")]
        public async Task<IActionResult> Login(SignInInputDTO input)
        {

            var SignInOutput = new SignInOutputDTO();
            try
            {
                if (!ValidationHelper.IsValidEmail(input.Email) || !ValidationHelper.IsValidPassword(input.password))
                    throw new Exception("Invalid format for Email or Password");


                string connectionString = "Data Source=DESKTOP-E8UDJO1;Initial Catalog=FoodTek;Integrated Security=True;Trust Server Certificate=True";
                    SqlConnection connection = new SqlConnection(connectionString);
                string query = $"SELECT Id ,role,IsActive,Fullname  FROM Users WHERE Email = @Email AND Password =@password";
                SqlCommand command = new SqlCommand(query, connection);
                    command.Parameters.AddWithValue("@Email", input.Email);
                    command.Parameters.AddWithValue("@Password", input.password);
                    connection.Open();
                    command.CommandType = CommandType.Text;
                    SqlDataAdapter adapter = new SqlDataAdapter(command);
                    DataTable dt = new DataTable();
                    adapter.Fill(dt);
                //otp   needed  
                   
                if(dt.Rows.Count == 0 )
                    return StatusCode(401, "Invalid Email or Password");

                if (dt.Rows.Count >1 ) 
                        throw new Exception("Somthing wrong");

                var claims = new[]
      {
            new Claim(JwtRegisteredClaimNames.Sub, input.Email),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };

                var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
                var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

                var token = new JwtSecurityToken(
                    issuer: _configuration["Jwt:Issuer"],
                    audience: _configuration["Jwt:Audience"],
                    claims: claims,
                    expires: DateTime.Now.AddHours(1),
                    signingCredentials: creds
                );

                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                SignInOutput.Id = Convert.ToInt32(dt.Rows[0]["Id"]);
                SignInOutput.Name = Convert.ToString(dt.Rows[0]["Fullname"]);


                foreach (DataRow row in dt.Rows)
                {
                    SignInOutput.Id= Convert.ToInt32( row["Id"]);

                    SignInOutput.Name = Convert.ToString(row["fullname"]);
                    }

                var Token = JwtHelper.GenerateJwtToken(dt.Rows, _configuration);

                return Ok(new { User = SignInOutput, Token = Token });
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

        [HttpPost("send-otp")]
        public async Task<IActionResult> SendOtp([FromBody] SendOtpDTO request)
        {
            string email = request.Email;
            string otp = new Random().Next(100000, 999999).ToString();

            var memoryCache = new MemoryCache(new MemoryCacheOptions());
            memoryCache.Set(email, otp, TimeSpan.FromMinutes(5));

            string subject = "Your OTP Code";
            string body = $"Your one-time password is: {otp}. It will expire in 5 minutes.";

            var smtpClient = new SmtpClient(_configuration["Email:SmtpServer"])
            {
                Port = int.Parse(_configuration["Email:Port"]),
                Credentials = new NetworkCredential(_configuration["Email:Username"], _configuration["Email:Password"]),
                EnableSsl = true
            };

            var mailMessage = new MailMessage
            {
                From = new MailAddress(_configuration["Email:From"]),
                Subject = subject,
                Body = body,
                IsBodyHtml = false
            };

            try
            {
                mailMessage.To.Add(email);
                await smtpClient.SendMailAsync(mailMessage);

                return Ok("OTP has been sent to your email.");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Failed to send OTP code: {ex.Message}");
            }
        }

        [HttpPost("verify-otp")]
        public IActionResult VerifyOtp([FromBody] OtpVerifyDTO input)
        {
            if (!_cache.TryGetValue(input.Email, out string storedOtp))
            {
                return BadRequest("OTP is invalid or expired");
            }

            if (storedOtp != input.Otp)
            {
                return BadRequest("OTP does not match");
            }

            _cache.Remove(input.Email);

            return Ok("OTP verified successfully");
        }

    }
}
