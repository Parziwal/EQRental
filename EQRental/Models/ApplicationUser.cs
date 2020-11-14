using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models
{
    public class ApplicationUser : IdentityUser
    {
        [Required]
        [StringLength(100)]
        public string FullName { get; set; }
        [Required]
        public string BankAccount { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
    }
}
