using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EQRental.Models
{
    public class UserAddress
    {
        public int ID { get; set; }
        [Required]
        [Range(1000, 10000)] 
        public int PostalCode { get; set; }
        [Required]
        [StringLength(100)]
        public string City { get; set; }
        [Required]
        [StringLength(100)] 
        public string Street { get; set; }
        public string UserID { get; set; }
        [Required]
        public ApplicationUser User { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
