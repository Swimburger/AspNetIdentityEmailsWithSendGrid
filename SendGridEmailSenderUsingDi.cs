using SendGrid;
using SendGrid.Helpers.Mail;
using Microsoft.AspNetCore.Identity.UI.Services;

namespace AspNetIdentityEmailsWithSendGrid;

public class SendGridEmailSenderUsingDi : IEmailSender
{
    private readonly ISendGridClient sendGridClient;
    private readonly ILogger logger;

    public SendGridEmailSenderUsingDi(ISendGridClient sendGridClient, ILogger<SendGridEmailSenderUsingDi> logger)
    {
        this.sendGridClient = sendGridClient;
        this.logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string message)
    {
        var msg = new SendGridMessage()
        {
            From = new EmailAddress("nielsswimberghe@gmail.com", "[YOUR_WEBSITE_NAME]"),
            Subject = subject,
            PlainTextContent = message,
            HtmlContent = message
        };
        msg.AddTo(new EmailAddress(toEmail));

        var response = await sendGridClient.SendEmailAsync(msg);
        if (response.IsSuccessStatusCode)
        {
            logger.LogInformation("Email queued successfully");
        }
        else
        {
            logger.LogError("Failed send email");
            // Adding more information related to the failed email could be helpful in debugging failure,
            // but be careful about logging PII, as it increases the chance of leaking PII
        }
    }
}