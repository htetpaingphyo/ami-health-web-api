using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Ami.Health.Core.Servies
{
    public class EmailService
    {
        public string Host { get; set; }
        public int Port { get; set; }
        public string FromAddress { get; set; }
        public string ToAddress { get; set; }
        public string Subject { get; set; }
        public string Body { get; set; }

        public void SendMail()
        {
            try
            {
                if (string.IsNullOrEmpty(Host)) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(FromAddress)) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(ToAddress)) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(Subject)) throw new ArgumentNullException();
                if (string.IsNullOrEmpty(Body)) throw new ArgumentNullException();

                SendMail(Host, Port, FromAddress, ToAddress, Subject, Body);
            }
            catch(Exception ex)
            {
                throw ex;
            }
        }

        private void SendMail(string host, int port, string from, string to, string subj, string msg)
        {
            SmtpClient client = new SmtpClient(host, port);
            MailMessage message = new MailMessage();

            try
            {
                MailAddress fromAddress = new MailAddress(from, "Sender");
                MailAddress toAddress = new MailAddress(to, "Recipient");

                message.From = fromAddress;
                message.To.Add(toAddress);
                message.Subject = subj;
                message.Body = msg;

                client.UseDefaultCredentials = true;
                NetworkCredential networkCredential = CredentialCache.DefaultNetworkCredentials;
                client.Credentials = networkCredential.GetCredential(host, port, "Basic");

                client.SendAsync(message, "Send completed!");
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
