using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Web;

namespace DepremAnalizSistemi.Helpers
{
    public static class MailSendHelper
    {
        public static bool Gonder(string konu, string icerik, string mailAdress)
        {
            bool kontrol;
            try
            {
                MailMessage ePosta = new MailMessage();
                ePosta.From = new MailAddress("yourgmailaccount@gmail.com");
                ePosta.To.Add(mailAdress);
                ePosta.Subject = konu;
                ePosta.Body = icerik;
                SmtpClient smtp = new SmtpClient();
                smtp.Credentials = new System.Net.NetworkCredential("yourgmailaccount@gmail.com", "yourpassword");
                smtp.Port = 587;
                smtp.Host = "smtp.gmail.com";
                smtp.EnableSsl = true;
                object userState = ePosta;
                kontrol = true;
                smtp.Send(ePosta);
            }
            catch (SmtpException)
            {
                kontrol = false;
            }
            return kontrol;
        }
    }
}