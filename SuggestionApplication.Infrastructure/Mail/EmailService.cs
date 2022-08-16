using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using SuggestionApplication.Application.Exceptions;
using SuggestionApplication.Application.Infrastructure;
using SuggestionApplication.Application.Models;

namespace SuggestionApplication.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings, ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmail(Email email)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var to = new EmailAddress(email.To);
            var from = new EmailAddress
            {
                Email = _emailSettings.FromAddress,
                Name = _emailSettings.FromName
            };

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, email.Subject, email.Body, email.Body);
            var response = await client.SendEmailAsync(sendGridMessage);

            if (response.StatusCode != System.Net.HttpStatusCode.Accepted || response.StatusCode != System.Net.HttpStatusCode.OK)
            {
                throw new InternalServerErrorException($"Email sending failed to {email.To}. Status code = {response.StatusCode}.");
            }            
        }
    }
}
