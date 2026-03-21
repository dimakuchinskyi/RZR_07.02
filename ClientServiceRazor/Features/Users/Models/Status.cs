using System;
using System.Collections.Generic;

namespace ClientServiceRazor.Features.Users.Models
{
    public class Status
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }

        public ICollection<User> Users { get; set; } = new List<User>();
    }
}
