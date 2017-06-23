using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net;
using System.Net.Mail;

namespace HelloTutor.Models
{
    public class EmailNotificationModel
    {

        public void SendEmail(String ToAddress, String MessageSubject, String MessageBody)
        {
            var fromAddress = new MailAddress("noreply.nashvilleliteracy.org@gmail.com", "Nashville Literacy (no-reply)");
            var toAddress = new MailAddress(ToAddress, ToAddress);
            const string fromPassword = "thisisthepassword";
            
            var smtp = new SmtpClient
            {
                Host = "smtp.gmail.com",
                Port = 587,
                EnableSsl = true,
                DeliveryMethod = SmtpDeliveryMethod.Network,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential(fromAddress.Address, fromPassword)
            };
            using (var message = new MailMessage(fromAddress, toAddress)
            {
                Subject = MessageSubject,
                Body = MessageBody
            })
            {
                smtp.Send(message);
            }
        }

    }
}
