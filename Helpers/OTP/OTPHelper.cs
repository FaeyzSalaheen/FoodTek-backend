//using MimeKit;
//using System;
//using MailKit.Net.Smtp;
//using Microsoft.Extensions.Configuration;
//using Microsoft.Data.SqlClient;

//namespace Foodtek.Helpers.OTP
//{
//    public class OTPHelper
//    {
//        private readonly IConfiguration _configuration;

//        // Inject IConfiguration to access app settings
//        public OTPHelper(IConfiguration configuration)
//        {
//            _configuration = configuration;
//        }

//        // Make GenerateOtp non-static to access configuration if needed
//        public string _GenerateOtp()
//        {
//            var otp = new Random().Next(100000, 999999).ToString();
//            return otp;
//        }

//        // Method to send OTP via email
//        public void SendOtpEmail(string recipientEmail, string otp)
//        {
//            try
//            {
//                var message = new MimeMessage();
//                message.From.Add(new MailboxAddress("Foodtek", _configuration["EmailSettings:SenderEmail"]));
//                message.To.Add(MailboxAddress.Parse(recipientEmail));
//                message.Subject = "Your OTP Code";

//                message.Body = new TextPart("plain")
//                {
//                    Text = $"Your OTP code is: {otp}"
//                };

//                using (var client = new SmtpClient())
//                {
//                    client.Connect("smtp.gmail.com", 587, MailKit.Security.SecureSocketOptions.StartTls);
//                    client.Authenticate(_configuration["EmailSettings:SenderEmail"], _configuration["EmailSettings:AppPassword"]);
//                    client.Send(message);
//                    client.Disconnect(true);
//                }
//            }
//            catch (Exception ex)
//            {
//                Console.WriteLine($"Error sending OTP email: {ex.Message}");
//            }
//        }



//        public void SaveOtpInDatabase(int userId, string otp)
//        {
//            string connectionString = _configuration.GetConnectionString("DefaultConnection");
//            string query = "INSERT INTO OTPs (UserId, OTPCode, CreatedAt, ExpiresAt, IsUsed, Attempts) " +
//                           "VALUES (@UserId, @OTPCode, @CreatedAt, @ExpiresAt, @IsUsed, @Attempts)";

//            using (var connection = new SqlConnection(connectionString))
//            {
//                var command = new SqlCommand(query, connection);
//                command.Parameters.AddWithValue("@UserId", userId);
//                command.Parameters.AddWithValue("@OTPCode", otp);
//                command.Parameters.AddWithValue("@CreatedAt", DateTime.Now);
//                command.Parameters.AddWithValue("@ExpiresAt", DateTime.Now.AddMinutes(10)); // صلاحية 10 دقائق
//                command.Parameters.AddWithValue("@IsUsed", false);
//                command.Parameters.AddWithValue("@Attempts", 0);

//                connection.Open();
//                command.ExecuteNonQuery();
//                connection.Close();
//            }
//        }









//    }
//}

