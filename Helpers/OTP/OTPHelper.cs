using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;

public static class EmailService
{
    public static async Task SendEmail(string email , string messag , string code)
    {
        var apiKey = "Ae8V3VA7TW6r-nQ3ou3eOg";
        var client = new SendGridClient(apiKey);
        var from = new EmailAddress("foodtekservis@gmail.com", "FoodTek");
        var subject = "Sending with SendGrid is Fun";
        var to = new EmailAddress(email, "e");
        var plainTextContent = $"Dear user{messag} this is you OTB {code}";
        //var htmlContent = "<strong>and easy to do anywhere, even with C#</strong>";
        var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent,"");
        var response = await client.SendEmailAsync(msg);
    }
}
