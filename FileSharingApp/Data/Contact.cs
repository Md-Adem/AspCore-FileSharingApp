using Microsoft.AspNetCore.Identity;
using Microsoft.Build.Framework;
using System.ComponentModel.DataAnnotations.Schema;

namespace FileSharingApp.Data
{
	public class Contact
	{

        public Contact()
        {
            SentDate = DateTime.Now;
        }

        public int Id { get; set; }

		public string? Name { get; set; }

		public string? Email { get; set; }

		public string? Subject { get; set; }

		public string? Message { get; set; }

		public bool status { get; set; }

		public DateTime SentDate { get; set; }

		
		[ForeignKey("User")]
		public string? UserId { get; set; }


		public virtual ApplicationUser? User { get; set; }

	}
}
