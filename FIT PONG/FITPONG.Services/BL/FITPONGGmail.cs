using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Collections.Generic;
using System.Linq;
using FIT_PONG.Database.DTOs;
using System.Threading.Tasks;

namespace FIT_PONG.Services.BL
{
    public class FITPONGGmail : iEmailServis
    {
        public void PosaljiKonfirmacijskiMejl(string linkzaklik,string email)
        {
            PomocnaFunkcija(email, linkzaklik, "Yo brate/bratice dobrodosao na fitpong potvrdi mejl ovdje");
        }
        public void PosaljiResetPassword(string linkzaklik, string email)
        {
            PomocnaFunkcija(email, linkzaklik, "Yo brate / bratice resetuj password ovdje");
        }
        public void PosaljiMejlReport(Report novi)
        {
            var Poruka = new MimeMessage();
            Poruka.From.Add(new MailboxAddress("fitpongtest@gmail.com"));
            Poruka.To.Add(new MailboxAddress("nedim.kurtovic@edu.fit.ba"));
            Poruka.To.Add(new MailboxAddress("aldin.talic@edu.fit.ba"));
            Poruka.Subject = novi.Naslov;
            Poruka.Body = new TextPart("html")
            {
                Text = "Poruka dolazi od " + novi.Email + "<br>" +
                "Sadrzaj poruke : " + novi.Sadrzaj
            };
            Sendaj(Poruka);

        }
        public void Sendaj(MimeMessage porukica)
        {
            using (var client = new SmtpClient())
            {
                client.Connect("smtp.gmail.com", 587);
                client.Authenticate("fitpongtest@gmail.com", "F!tP0ng_2019!");
                client.Send(porukica);
                client.Disconnect(false);
            }
            
        }
        public void PomocnaFunkcija(string email, string linkzaklik,string poruka)
        {
            var Poruka = new MimeMessage();
            Poruka.From.Add(new MailboxAddress("fitpongtest@gmail.com"));
            Poruka.To.Add(new MailboxAddress(email));

            Poruka.Body = new TextPart("html")
            {
                Text = poruka + ": " + "<a href='" + linkzaklik + "'>KLIK</a>"
            };
            Sendaj(Poruka);
        }
        public void PosaljiTwoFactorCode(int code, string email)
        {
            var Poruka = new MimeMessage();
            Poruka.From.Add(new MailboxAddress("fitpongtest@gmail.com"));
            Poruka.To.Add(new MailboxAddress(email));

            Poruka.Body = new TextPart("html")
            {
                Text = "Ovo je kod za aktivaciju: " + code
            };
            Sendaj(Poruka);
        }
    }
}
