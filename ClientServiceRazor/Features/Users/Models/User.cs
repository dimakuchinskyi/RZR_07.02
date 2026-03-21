using System;

namespace ClientServiceRazor.Features.Users.Models
{
    public class User
    {
        public Guid Id { get; set; }

        public string? Login { get; set; }
        public string? Password { get; set; }
        public string? Email { get; set; }

        public Guid? StatusId { get; set; }
        public Status? Status { get; set; }

        public Guid? RoleId { get; set; }
        public Role? Role { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
