namespace JetLogistics.Identity.API.Models
{
    public class ApplicationUser
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Username { get; set; } = default!;
        public string PasswordHash { get; set; } = default!;
        public string? Email { get; set; }
        public string? FullName { get; set; }
    }
}
