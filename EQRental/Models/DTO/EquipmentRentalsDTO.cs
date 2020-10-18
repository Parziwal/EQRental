using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class EquipmentRentalsDTO
    {
        public int Id { get; set; }
        public UserDTO Rentler { get; set; }
        public string Status { get; set; }
        public UserAddressDTO Address { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }

        public EquipmentRentalsDTO(Rental rental, ApplicationUser rentler, UserAddress address, Status status, Payment payment)
        {
            Id = rental.ID;
            Rentler = new UserDTO(rentler);
            Status = status.Name;
            Address = new UserAddressDTO(address);
            StartDate = rental.StartDate;
            EndDate = rental.EndDate;
            OrderDate = rental.OrderDate;
            PaymentMethod = payment.Name;
        }
    }
}
