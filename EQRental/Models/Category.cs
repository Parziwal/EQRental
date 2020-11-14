using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models
{
    public class Category
    {
        public int ID { get; set; }
        [Required]
        [StringLength(40)]
        public string Name { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
    }
}
