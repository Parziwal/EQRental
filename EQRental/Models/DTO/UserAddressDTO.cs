using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class UserAddressDTO
    {
        public int Id { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }

        public UserAddressDTO(UserAddress userAddress)
        {
            Id = userAddress.ID;
            PostalCode = userAddress.PostalCode;
            City = userAddress.City;
            Street = userAddress.Street;
        }
    }
}
