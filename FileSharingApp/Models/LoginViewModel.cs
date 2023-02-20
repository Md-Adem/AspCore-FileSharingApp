using System.ComponentModel.DataAnnotations;
using FileSharingApp.Resources;

namespace FileSharingApp.Models
{
    public class LoginViewModel
    {
        [EmailAddress (ErrorMessageResourceName = "Email", ErrorMessageResourceType = typeof(SharedResource)) ]
        [Required (ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource)) ]
        public string? Email { get; set; }

        [Required (ErrorMessageResourceName = "Required", ErrorMessageResourceType = typeof(SharedResource)) ]
        public string? Password { get; set; }
    }
}
