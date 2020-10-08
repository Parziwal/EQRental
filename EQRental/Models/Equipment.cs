using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace EQRental.Models
{
    public class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public int PricePerDay { get; set; }
        public bool Available { get; set; }
        public int CategoryID { get; set; }
        public string OwnerID { get; set; }
        public Category Category { get; set; }
        public ApplicationUser Owner { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
