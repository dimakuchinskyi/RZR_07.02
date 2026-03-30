using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using ClientServiceRazor.Features.Data;
using ClientServiceRazor.Features.Clients.Models;
using Microsoft.EntityFrameworkCore;

namespace ClientServiceRazor.Features.Clients.Pages
{
    public class IndexModel : PageModel
    {
        private readonly ApplicationDbContext _db;

        public IndexModel(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Client> Clients { get; set; } = new();

        [BindProperty]
        public ClientServiceRazor.Features.Clients.ViewModels.ClientViewModel NewClient { get; set; } = new();

        public async Task OnGetAsync()
        {
            Clients = await _db.Clients.AsNoTracking().ToListAsync();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                // reload clients to display
                Clients = await _db.Clients.AsNoTracking().ToListAsync();
                return Page();
            }

            var client = new Client
            {
                Id = System.Guid.NewGuid(),
                Surname = NewClient.Surname,
                FirstName = NewClient.FirstName,
                Patronymic = NewClient.Patronymic,
                Email = NewClient.Email,
                BirthDate = NewClient.BirthDate,
                CreatedAt = System.DateTimeOffset.UtcNow
            };

            _db.Clients.Add(client);
            await _db.SaveChangesAsync();

            return RedirectToPage();
        }
    }
}
