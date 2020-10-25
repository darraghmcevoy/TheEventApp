using MailKit.Net.Smtp;
using MimeKit;
using System.Text;
using System.Web;
namespace TheEventApp.Models
{
    public class EmailHelper
    {
        public static void SendEmail(Event @event, ApplicationUser @appUser)
        {
            string toEmail = "darraghmcevoy3@gmail.com";
            string fromEmail = "darraghmcevoy3@gmail.com";//change sender email
            string fromEmailPwd = "brunocat22"; //add sender password
            var email = new MimeMessage();
            email.From.Add(new MailboxAddress("Event Manager", fromEmail));
            email.To.Add(new MailboxAddress("Recipient", toEmail));
            var emailBody = new BodyBuilder
            {
                HtmlBody = EmailTemplate(@event, @appUser)
            };
            email.Subject = @event.Title;
            email.Body = emailBody.ToMessageBody();

            if (!string.IsNullOrEmpty(@event.InviteEmail))
            {
                if (@event.InviteEmail.Contains(","))
                {
                    foreach (var item in @event.InviteEmail.Split(','))
                    {
                        email.Bcc.Add(new MailboxAddress("Recipient", item.Trim()));
                    }
                }
                else
                {
                    email.Bcc.Add(new MailboxAddress("Recipient", @event.InviteEmail.Trim()));
                }
            }


            using (var smtp = new SmtpClient())
            {
                smtp.Connect("smtp.gmail.com", 465, true);
                smtp.Authenticate(fromEmail, fromEmailPwd);
                smtp.Send(email);
                smtp.Disconnect(true);
            }
        }

        public static string EmailTemplate(Event _event, ApplicationUser user)
        {
            try
            {
                string host = HttpContext.Current.Request.Url.Host;
                StringBuilder sb = new StringBuilder();
                sb.Append("<html>");
                sb.Append("<body>");
                sb.Append("<table><tr><td colspan='2'>");
                sb.Append("<td>" + _event.Title + "</td>");
                sb.Append("</td></tr>");
                sb.Append("<tr><td>Date</td>");
                sb.Append("<td>" + _event.Date + "</td></tr>");
                sb.Append("<tr><td>description</td>");
                sb.Append("<td>" + _event.Description + "</td></tr>");
                sb.Append("<tr><td>Invited by</td><td>" + user.UserName + "</td></tr>");
                sb.Append($"<tr><td colspan=2><a href='{host}/Events/AcceptInvitation?Id={_event.Id}'>Click here to accept</a></td></tr>");
                sb.Append("</table>");
                sb.Append("</body>");
                sb.Append("</html>");
                return sb.ToString();
            }
            catch
            {
                throw;
            }
        }
    }
}