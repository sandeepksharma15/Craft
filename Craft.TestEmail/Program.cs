using MailKit.Net.Smtp;
using MimeKit;

try
{
    var message = new MimeMessage();
    message.From.Add(new MailboxAddress("Admin", "xxx"));
    message.To.Add(new MailboxAddress("Cualifi", "cualifi@gmail.com"));
    message.Subject = "Test Email";
    message.Body = new TextPart("plain") { Text = "This is a test email from C#." };

    using (var client = new SmtpClient())
    {
        client.Connect("xxx", 465, true); // Use SSL/TLS settings as required
        client.Authenticate("xxx", "xxx");

        client.Send(message);
        client.Disconnect(true);
    }

    Console.WriteLine("Email sent successfully!");
}
catch (Exception ex)
{
    Console.WriteLine($"Error: {ex.Message}");
}
