
using System.Net;
using System.Net.Mail;
using System.Reflection;
using GuardX.Common;
using GuardX.Enums;
using GuardX.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;

namespace GuardX.BLServices
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpHost;
        private readonly int _smtpPort;
        private readonly string _smtpMail;
        private readonly string _smtpPassword;

        //Injection of Services
        private readonly ILogger<EmailService> _logger;
        private readonly IRegistryServices _registryServices;

        public EmailService(IRegistryServices registryServices,ILogger<EmailService> logger)
        {
            _registryServices = registryServices;
            _logger = logger;

            var config = readConfig();

            _smtpHost = config["Smtp:Host"];
            _smtpPort = int.Parse(config["Smtp:Port"]);
            _smtpMail = config["Smtp:Mail"];
            _smtpPassword = config["Smtp:Password"];
        }

        private IConfigurationRoot readConfig()
        {
            string jsonContent;
            var assembly = Assembly.GetExecutingAssembly();
            //GetManifestResourceStream() helps to read the embedded files.
            using (var stream = assembly.GetManifestResourceStream("GuardX.appsettings.json"))
            {
                using (var reader = new StreamReader(stream!))
                {
                    jsonContent = reader.ReadToEnd();
                }
            }

            var config = new ConfigurationBuilder()
                            .AddJsonStream(new MemoryStream(System.Text.Encoding.UTF8.GetBytes(jsonContent)))
                            .Build();       
            return config;
        }

        /// <summary>
        /// Get the customized Email template and send it to receipent.
        /// </summary>
        /// <param name="name"></param>
        /// <param name="email"></param>
        /// <param name="otpValue"></param>
        /// <param name="emailPurpose"></param>
        /// <returns>
        /// EResult
        /// </returns>
        public EResult SendOtpEmail(string name, string email,int otpValue, EEmailPurpose emailPurpose)
        {
            EResult eResult = EResult.OK;
            try
            {
                string personalizedContent;

                //Load the email-template.
                var assembly = Assembly.GetExecutingAssembly();
                string resourceName = "GuardX.Resources.email_template.html";

                //GetManifestResourceStream() helps to read the embedded files.
                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        personalizedContent = reader.ReadToEnd();
                    }
                }

                //Replace the placeholder with actual values in customized email.
                personalizedContent = personalizedContent.Replace("{{RecipientEmail}}", name)
                    .Replace("{{OTP}}",Convert.ToString(otpValue));

                
                //Decide purpose of email. (Reset password or Setup Profile)
                if (EEmailPurpose.SetupProfile == emailPurpose)
                {
                    personalizedContent = personalizedContent.Replace("{{EmailPurpose}}", Constants.EMAIL_PURPOSE_SETUP_PROFILE);
                }
                else
                {
                    personalizedContent = personalizedContent.Replace("{{EmailPurpose}}", Constants.EMAIL_PURPOSE_RESET_PROFILE);
                }

                //Set mandatory parameters
                var subject = Constants.OTP_EMAIL_SUBJECT;
                var body = personalizedContent;
                var message = new MailMessage(_smtpMail, email, subject,body);
                message.IsBodyHtml = true;

                //Create SMTP Client
                var smtp = new SmtpClient(_smtpHost, _smtpPort)
                {
                    Credentials = new NetworkCredential(_smtpMail, _smtpPassword),
                    EnableSsl = true
                };

                //Send the Email.
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                _logger.LogError($"SENDING_EMAIL_FAIL_#_Message:{ex.Message}_#_StackTrace:{ex.StackTrace}");
                eResult = EResult.ERROR;
            }
            return eResult;
        }
    }
}
