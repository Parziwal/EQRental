using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class UserDTO
    {
        public string Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string BankAccount { get; set; }

        public UserDTO(ApplicationUser user)
        {
            Id = user.Id;
            Name = user.FullName;
            Email = user.Email;
            PhoneNumber = user.PhoneNumber;
            BankAccount = user.BankAccount;
        }
    }
}
