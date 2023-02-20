using Microsoft.AspNetCore.Identity;
using System;


namespace FileSharingApp.Data
{
    public class Uploads
    {
        public Uploads()
        {
            Id = Guid.NewGuid().ToString();
            UploadDate = DateTime.Now;
        }

        public string Id { set; get; }
	    public string? FileName { set; get; }
	    public string? OriginalFileName { set; get; }
	    public string? ContentType { set; get; }
	    public decimal Size { set; get; }
	    public string? UserId { set; get; }
	    public DateTime UploadDate { set; get; }

        public ApplicationUser? User { get; set; }
        public long DownloadCount { get; set; }

    }
}
