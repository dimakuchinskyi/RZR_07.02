using System;
using System.ComponentModel.DataAnnotations;

namespace ClientServiceRazor.Features.Clients.ViewModels
{
    public class ClientViewModel
    {
        [Required]
        [StringLength(50)]
        public string? Surname { get; set; }

        [StringLength(50)]
        public string? FirstName { get; set; }

        [StringLength(50)]
        public string? Patronymic { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [DataType(DataType.Date)]
        public DateTime? BirthDate { get; set; }

        // Просте поле для одного номера телефону у формі
        [StringLength(20)]
        public string? PhoneNumber { get; set; }
    }
}
