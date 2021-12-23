using System;
using System.Net.Mail;
using WorkforceManagement.Common;
using WorkforceManagement.Data.Entities;
using WorkforceManagement.Services.Interfaces;

namespace WorkforceManagement.Services.Services
{
    public class MailService : IMailService
    {
        public bool SendEmail(User sender, string toAddress, TimeOffRequest requestToBeSend)
        {
            bool result = true;

            string senderID = sender.Email;
            string senderPassword = sender.PasswordHash;

            string subject = "Time off from " + requestToBeSend.StartDate + " to " + requestToBeSend.EndDate;
            string body = requestToBeSend.Reason;

            try
            {
                SmtpClient smtp = new SmtpClient
                {
                    Host = GlobalConstants.Host,
                    Port = GlobalConstants.Port,
                    DeliveryMethod = SmtpDeliveryMethod.Network,
                    Credentials = new System.Net.NetworkCredential(senderID, senderPassword),
                    Timeout = 30000,
                };

                MailMessage message = new MailMessage(senderID, toAddress, subject, body);
                smtp.Send(message);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException(ex.Message);
            }

            return result;
        }
    }
}