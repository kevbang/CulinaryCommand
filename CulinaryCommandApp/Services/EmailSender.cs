namespace CulinaryCommand.Services
{
    public class EmailSender : IEmailSender
    {
        public async Task SendEmailAsync(string toEmail, string subject, string body)
        {
            // TODO: Replace with real SMTP or SendGrid logic
            Console.WriteLine("=== EMAIL SENT ===");
            Console.WriteLine($"To: {toEmail}");
            Console.WriteLine($"Subject: {subject}");
            Console.WriteLine(body);

            await Task.CompletedTask;
        }
    }
}