using System;

namespace ClientServiceRazor.Features.Clients.Models
{
    public class ClientFinanceAccount
    {
        public Guid ClientId { get; set; }
        public Client? Client { get; set; }

        public Guid FinanceAccountId { get; set; }
        public FinanceAccount? FinanceAccount { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
    }
}
