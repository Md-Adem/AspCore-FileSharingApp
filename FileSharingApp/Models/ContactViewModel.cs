using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models
{
    
    public class ContactViewModel
    {
        [Required]
        public string? Name { get; set; }

        [EmailAddress]
        [Required]
        public string? Email { get; set; }

        [Required]
        public string? Subject { get; set; }

        public string? Message { get; set; }
    }
}
