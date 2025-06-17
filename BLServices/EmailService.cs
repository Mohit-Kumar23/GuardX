using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.Extensions.Logging;

namespace GuardX.BLServices
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpMail;
        private readonly string _smtpPassword;
        
        private readonly ILogger<EmailService> _logger;
        private readonly IRegistryServices _registryServices;

        public EmailService(IRegistryServices registryServices,ILogger<EmailService> logger)
        {
            _registryServices = registryServices;
            _logger = logger;

            _smtpHost = ConfigurationManager.AppSettings["YahooSmtpHost"];
            _smtpPort = Convert.ToInt32(ConfigurationManager.AppSettings["YahooSmtpPort"]);
            _smtpMail = ConfigurationManager.AppSettings["YahooMail"];
            _smtpPassword = ConfigurationManager.AppSettings["YahooAppPassword"];
        }

        public EResult SendOtpEmail(string name, string email,int otpValue, EEmailPurpose emailPurpose)
        {
            EResult eResult = EResult.OK;
            try
            {
                string personalizedContent;

                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = "GuardX.Resources.email_template.html";

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                using (StreamReader reader = new StreamReader(stream))
                {
                    personalizedContent = reader.ReadToEnd();
                }                

                personalizedContent = personalizedContent.Replace("{{RecipientEmail}}", name)
                    .Replace("{{OTP}}",Convert.ToString(otpValue));


                if(EEmailPurpose.SetupProfile == emailPurpose)
                {
                    personalizedContent = personalizedContent.Replace("{{EmailPurpose}}", Constants.EMAIL_PURPOSE_SETUP_PROFILE);
                }
                else
                {
                    personalizedContent = personalizedContent.Replace("{{EmailPurpose}}", Constants.EMAIL_PURPOSE_RESET_PROFILE);
                }


                var subject = Constants.OTP_EMAIL_SUBJECT;
                var body = personalizedContent;
                var message = new MailMessage(_smtpMail, email, subject,body);
                message.IsBodyHtml = true;

                var smtp = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpMail, _smtpPassword),
                    EnableSsl = true
                };

                smtp.Send(message);
            }
            catch (Exception ex)
            {
                //TODO: Log Here
                eResult = EResult.ERROR;
            }
            return eResult;
        }
    }
}
