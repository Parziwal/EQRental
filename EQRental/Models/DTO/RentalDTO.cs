using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class RentalDTO
    {
        public int Id { get; set; }
        public EquipmentOverviewDTO Equipment { get; set; }
        public UserDTO Owner { get; set; }
        public UserAddressDTO Address { get; set; }
        public string Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OrderDate { get; set; }
        public string PaymentMethod { get; set; }

        public RentalDTO(Rental rental, Equipment equipment, Category category, ApplicationUser owner, UserAddress address, Status status, Payment payment)
        {
            Id = rental.ID;
            Equipment = new EquipmentOverviewDTO(equipment, category);
            Owner = new UserDTO(owner);
            Status = status.Name;
            Address = new UserAddressDTO(address);
            StartDate = rental.StartDate;
            EndDate = rental.EndDate;
            OrderDate = rental.OrderDate;
            PaymentMethod = payment.Name;
        }
    }
}
