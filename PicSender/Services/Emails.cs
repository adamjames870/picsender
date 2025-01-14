using PicSender.Models;

namespace PicSender.Services;

public class Emails : IDisposable
{
    private IEmail? _emailService;
    
    public Emails(IEmail emailService)
    {
        _emailService = emailService;
    }

    public async Task SendSampleEmailAsync(PictureGroup pictureGroup)
    {
        var subject = $"Pictures from {pictureGroup.Title}";
        const string body = "It was great to see you last weekend.";
        var recipients = new[] { "adam@adamjames.me.uk", "adam.james870@yahoo.co.uk" };

        var message = new EmailMessage
        {
            Subject = subject,
            Body = body,
            BodyFormat = EmailBodyFormat.PlainText,
            To = [..recipients]
        };

        await Email.Default.ComposeAsync(message);    }

    public void Dispose()
    {
        _emailService = null;
    }
}