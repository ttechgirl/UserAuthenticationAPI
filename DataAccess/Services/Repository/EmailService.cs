using DataAccess.Services.Interface;
using Domain.Models;
using Domain.ViewModel;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Services.Repository
{
    public class EmailService : IEmailService
    {
        private readonly EmailConfig emailConfig;
        public EmailService(EmailConfig emailConfig)
        {
            this.emailConfig = emailConfig;
        }

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmail(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmail(Message message)
        {
            var emailMessage = new MimeMessage();

            //adds a specified email address of the sender 
            emailMessage.From.Add(new MailboxAddress("email", emailConfig.From));
            //adds email address of the receivers(more than one) 
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body =new TextPart(message.Body);
            return emailMessage;
        }

        private void Send(MimeMessage message) 
        {
            //connecting to the smtp server
            var client = new SmtpClient();
            client.Connect(emailConfig.SmtpServer, emailConfig.Port, true);
            //removes authentication supported bt smtpserver
            client.AuthenticationMechanisms.Remove("");
            //adds authentication through the username and password provided in the appsettings
            client.Authenticate(emailConfig.UserName, emailConfig.Password);
            client.Send(message);
            client.Disconnect(true);
            client.Dispose();
        }
    }


}
