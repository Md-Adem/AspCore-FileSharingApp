namespace FileSharingApp.Areas.Admin.Models
{
    public class ContactUsViewModel
    {

        public int Id { get; set; }

        public string? Name { get; set; }

        public string? Email { get; set; }

        public string? Subject { get; set; }

        public string? Message { get; set; }


        public bool status { get; set; }

        public DateTime SentDate { get; set; }

    }
}
