using FileSharingApp.Resources;
using FileSharingApp.Resources.Models;
using Microsoft.AspNetCore.Http;
using System.ComponentModel.DataAnnotations;

namespace FileSharingApp.Models
{
    public class UploadsViewModel
    {
        public string? Id { set; get; }

        [Required]
        public string? FileName { get; set; }

        //[Display(Name = "Original File Name", ResourceType = typeof(UploadsViewModelResource))]
        [Display(Name = "Original File Name")]
        public string? OriginalFileName { set; get; }

        [Display(Name = "Size")]
        public decimal Size { get; set; }

        [Display(Name = "Content Type")]
        public string? ContentType { get; set; }
        public string? UserId { set; get; }

        [Display(Name = "Upload Date")]
        public DateTime UploadDate { set; get; }

        [Display(Name = "Download Count")]
        public long DownloadCount { get; set; }
    }
}
