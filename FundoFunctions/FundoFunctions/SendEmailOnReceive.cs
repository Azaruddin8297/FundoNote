using System;
using System.Net.Mail;
using System.Net;
using Microsoft.Azure.WebJobs;
using Microsoft.Azure.WebJobs.Host;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace FundoFunctions
{
    public class SendEmailOnReceive
    {
        private readonly IConfiguration _config;
        private readonly string _senderEmail;
        private readonly string _senderAppPassword;

        private const int _port = 587;
        private const string _host = "smtp.gmail.com";

        public SendEmailOnReceive(IConfiguration config)
        {
            _config = config;

            _senderEmail = _config["SenderEmail"];
            _senderAppPassword = _config["SenderAppPassword"];
        }

        [FunctionName("SendEmailOnReceive")]
        public void Run([ServiceBusTrigger("forget-password", Connection = "ServiceBusConnection")] string messageBody, string To)
        {
            string resetToken = messageBody;
            string subject = "Fundoo Notes Reset Token";
            string body = $"\nTo reset your fundoo-notes password, copy and paste the following code : \n\n\n\t {resetToken}";

            using (SmtpClient smtpClient = new SmtpClient(_host))
            {
                smtpClient.Port = _port;
                smtpClient.EnableSsl = true;
                smtpClient.Credentials = new NetworkCredential(_senderEmail, _senderAppPassword);
                smtpClient.Send(_senderEmail, To, subject, body);
            }
        }
    }
}
