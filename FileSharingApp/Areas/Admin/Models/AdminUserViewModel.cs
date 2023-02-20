namespace FileSharingApp.Areas.Admin.Models
{
    public class AdminUserViewModel
    {
        public string? Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public bool IsBlocked { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
