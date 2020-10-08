using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public string BankAccount { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
    }
}
