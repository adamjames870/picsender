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
        
        

        await Email.Default.ComposeAsync(message);
        
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
            message.Attachments.Add(new EmailAttachment(picture.FullPath));
        }

        await Email.Default.ComposeAsync(message);

    }
}