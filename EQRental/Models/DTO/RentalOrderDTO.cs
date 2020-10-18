using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class RentalOrderDTO
    {
        public int EquipmentId { get; set; }
        public int AddressId { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public string PaymentMethod { get; set; }
    }
}
