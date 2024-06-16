using CreativePhotography.Content;
using CreativePhotography.IService;
using CreativePhotography.Models;
using System.Net;
using System.Net.Mail;

namespace CreativePhotography.Service
{
    public class MailService : IMailService
    {
        private readonly IConfiguration _configuration;
        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public async Task<string> SendMailToAdmin(ContactUsModel userInfo)
        {
            try
            {
                using (SmtpClient smtpClient = new SmtpClient())
                {
                    var smtpSettings = _configuration.GetSection("SmtpSettings");
                    smtpClient.Host = smtpSettings["Host"]??String.Empty;
                    smtpClient.Port = Convert.ToInt32(smtpSettings["Port"]);
                    smtpClient.UseDefaultCredentials = false;
                    smtpClient.EnableSsl = true;
                    smtpClient.Credentials = new NetworkCredential(smtpSettings["UserName"], smtpSettings["Password"]);

                    MailMessage mailMessage = new MailMessage();
                    mailMessage.From = new MailAddress(smtpSettings["UserName"]??String.Empty);
                    mailMessage.To.Add("hitanshu2449@gmail.com"??String.Empty);
                    mailMessage.Subject = EmailOperations.emailSubject;

                    // Read the HTML email template from the file
                    string templatePath = @"C:\Projects\Photograph_Project\CreativePhotography\CreativePhotography\EmailTemplates\AdminNotificationEmailTemplate.html";
                    string emailBody = await ReadTemplateAsync(templatePath);

                    // Replace placeholders in the template with actual values
                    emailBody = ReplacePlaceholders(emailBody, userInfo);

                    mailMessage.Body = emailBody;
                    mailMessage.IsBodyHtml = true; // Set to true for HTML content

                    // Set email headers
                    mailMessage.Headers.Add("X-Mailer", "CreativePhotography"); // Set your application name

                    // Send email asynchronously
                    await smtpClient.SendMailAsync(mailMessage);
                }
                return EmailOperations.successSendEmail;
            }
            catch (Exception ex)
            {
                return EmailOperations.failureSendEmail;
            }
        }

        private async Task<string> ReadTemplateAsync(string templatePath)
        {
            using (StreamReader reader = new StreamReader(templatePath))
            {
                return await reader.ReadToEndAsync();
            }
        }

        private string ReplacePlaceholders(string emailBody, ContactUsModel userInfo)
        {
            return emailBody.Replace("{0}", userInfo.FirstName)
                            .Replace("{1}", userInfo.LastName)
                            .Replace("{2}", userInfo.Contact)
                            .Replace("{3}", userInfo.Email)
                            .Replace("{4}", userInfo.Subject)
                            .Replace("{5}", userInfo.Message);
        }

    }
}
