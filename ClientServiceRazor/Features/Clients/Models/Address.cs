using System;

namespace ClientServiceRazor.Features.Clients.Models
{
    public class Address
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public string? Country { get; set; }
        public string? Region { get; set; }
        public string? Area { get; set; }
        public string? City { get; set; }
        public string? Street { get; set; }
        public string? Building { get; set; }
        public string? Apartment { get; set; }
        public string? Entrance { get; set; }
        public string? Room { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
