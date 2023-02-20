using System.Net;
using System.Net.Mail;
using System.Text;

namespace FileSharingApp.Helpers.Mail
{
	public class MailHelper : IMailHelper
	{
		private readonly IConfiguration _config;

		public MailHelper(IConfiguration config)
		{
			_config = config;
		}

		public void SendMail(InputEmailMessage model)
		{
			using (SmtpClient client = new SmtpClient(_config.GetValue<string>("Mail:Host"), _config.GetValue<int>("Mail:Port")))
			{
				var msg = new MailMessage();

				msg.To.Add(model.Email);
				msg.From = new MailAddress(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:Sender"),Encoding.UTF8);

				msg.Subject = model.Subject;
				msg.Body = model.Body;


				client.UseDefaultCredentials = false;
				client.EnableSsl = true;

				client.Credentials = new NetworkCredential(_config.GetValue<string>("Mail:From"), _config.GetValue<string>("Mail:PWD"));

				client.Send(msg);

			}
		}
	}
}
