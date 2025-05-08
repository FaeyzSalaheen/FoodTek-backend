using System.Net;
using System.Net.Mail;
using Microsoft.Extensions.Configuration;
using SendGrid.Helpers.Mail;
using SendGrid;

public static class EmailService
{
    public static async Task SendEmail(string email , string messag , string code)
    {
        var apiKey = "SG.EjDU - irlTi - 8CIFKnGNxiQ.3ipLfxFaM7hO4mGPpGxIu1LTUghJwHOws0Q6KhjI448";
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
