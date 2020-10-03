using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class User
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string BankAccount { get; set; }
        public string PhoneNumber { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
        public ICollection<UserAddress> Addresses { get; set; }
    }
}
