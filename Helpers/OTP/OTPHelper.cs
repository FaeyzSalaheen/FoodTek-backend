using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;

public static class EmailService
{
    public static async Task SendEmail(string email , string messag , string code)
    {  
          
        var apiKey = "";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("foodtekservis@gmail.com", "FoodTek");
        var subject = "FoodTek";
        var to = new EmailAddress(email, "e");
        var plainTextContent = $"{messag}{code}";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,"");
        var response = await client.SendEmailAsync(msg);
    }
}
