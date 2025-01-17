using PicSender.Models;

namespace PicSender.Services;

public class Emails : IDisposable
{
    private IEmail? _emailService;
    private PicDatabase _database;
    
    public Emails(IEmail emailService, PicDatabase database)
    {
        _emailService = emailService;
        _database = database;
    }

    public void Dispose()
    {
        _emailService = null;
    }

    public async Task SendEmailWithAttachaments(PictureGroup pictureGroup, List<SinglePicture> pictures)
    {
        var subject = $"{pictureGroup.Title}";
        const string body = "Enclosed images.";
        var options = await _database.GetAppOptionsAsync();
        // var recipients = new[] { "adam@adamjames.me.uk", "adam.james870@yahoo.co.uk" };
        var recipients = new[] { options.EmailAddress };

        
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