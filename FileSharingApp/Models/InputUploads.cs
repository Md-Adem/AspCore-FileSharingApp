using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models
{
    public class InputUploads
    {
        [Required]
        public IFormFile? File { get; set; }
    }
}
