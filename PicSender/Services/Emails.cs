using PicSender.Models;

namespace PicSender.Services;

public class Emails : IDisposable
{
    private IEmail? _emailService;
    
    public Emails(IEmail emailService)
    {
        _emailService = emailService;
    }

    public void Dispose()
    {
        _emailService = null;
    }

    public async Task SendEmailWithAttachaments(PictureGroup pictureGroup, List<SinglePicture> pictures)
    {
        var subject = $"Pictures from {pictureGroup.Title}";
        const string body = "Enclosed images.";
        var recipients = new[] { "adam@adamjames.me.uk", "adam.james870@yahoo.co.uk" };

        var message = new EmailMessage
        {
            Subject = subject,
            Body = body,
            BodyFormat = EmailBodyFormat.PlainText,
            To = [..recipients]
        };
        
        foreach (var picture in pictures)
        {
            if (picture.FullPath != null) message.Attachments?.Add(new EmailAttachment(picture.FullPath));
        }

        await Email.Default.ComposeAsync(message);

    }
}