using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EQRental.Models
{
    public class Status
    {
        public int ID { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
