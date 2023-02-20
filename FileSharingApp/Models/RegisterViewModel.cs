using FileSharingApp.Resources;
using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models
{
    public class RegisterViewModel
    {
        [EmailAddress (ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource))]
        [Required (ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string? Email { get; set; }

        [Required (ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string? Password { get; set; }

        [Compare ("Password")]
        [Display (Name = "Confirm Password")]
        [Required (ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource))]
        public string? ConfirmPassword { get; set; }
    }
}
