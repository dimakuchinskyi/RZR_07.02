using System;
using System.Collections.Generic;

namespace ClientServiceRazor.Features.Clients.Models
{
    public class Client
    {
        public Guid Id { get; set; }

        public string? Surname { get; set; }
        public string? FirstName { get; set; }
        public string? Patronymic { get; set; }
        public string? Email { get; set; }

        public DateTime? BirthDate { get; set; }

        public DateTimeOffset CreatedAt { get; set; }
        public DateTimeOffset? UpdatedAt { get; set; }
        
        public ICollection<Phone> Phones { get; set; } = new List<Phone>();
        
        public Address? Address { get; set; }
        
        public ICollection<ClientFinanceAccount> ClientFinanceAccounts { get; set; } = new List<ClientFinanceAccount>();
    }
}
