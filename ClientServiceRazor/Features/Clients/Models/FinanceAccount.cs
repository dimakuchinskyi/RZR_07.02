using System;
using System.Collections.Generic;

namespace ClientServiceRazor.Features.Clients.Models
{
    public class FinanceAccount
    {
        public Guid Id { get; set; }

        public decimal Balance { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        
        public ICollection<ClientFinanceAccount> ClientFinanceAccounts { get; set; } = new List<ClientFinanceAccount>();
    }
}
