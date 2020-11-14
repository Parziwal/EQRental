using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EQRental.Models
{
    public class Rental
    {
        public int ID { get; set; }
        [Required]
        public Equipment Equipment { get; set; }
        [Required]
        public UserAddress Address { get; set; }
        [Required]
        public Status Status { get; set; }
        [Required]
        public DateTime StartDate { get; set; }
        [Required]
        public DateTime EndDate { get; set; }
        [Required]
        public DateTime OrderDate { get; set; }
        [Required]
        public Payment Payment { get; set; }
        public int EquipmentId { get; set; }
        public int AddressID { get; set; }
        public int StatusID { get; set; }
        public int PaymentID { get; set; }
    }
}
