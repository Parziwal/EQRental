using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace EQRental.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        [Required]
        [StringLength(200)]
        public string Name { get; set; }
        [Required]
        [StringLength(4000)]
        public string Details { get; set; }
        public string ImagePath { get; set; }
        [Required] 
        public int PricePerDay { get; set; }
        [Required] 
        public bool Available { get; set; }
        public int CategoryID { get; set; }
        public string OwnerID { get; set; }
        public bool Deleted { get; set; }
        public Category Category { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
