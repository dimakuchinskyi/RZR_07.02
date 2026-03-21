using System;

namespace ClientServiceRazor.Features.Clients.Models
{
    public class Phone
    {
        public Guid Id { get; set; }

        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public string? Number { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
    }
}
