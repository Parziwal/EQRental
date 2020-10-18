using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class OwnEquipmentDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public int PricePerDay { get; set; }
        public bool Available { get; set; }
        public string Category { get; set; }
        public List<EquipmentRentalsDTO> Rentals { get; set; }

        public OwnEquipmentDTO(Equipment equipment, Category category, ApplicationUser user, List<EquipmentRentalsDTO> rentals)
        {
            Id = equipment.ID;
            Name = equipment.Name;
            Details = equipment.Details;
            ImagePath = equipment.ImagePath;
            PricePerDay = equipment.PricePerDay;
            Available = equipment.Available;
            Category = category.Name;
            Rentals = rentals;
        }
    }
}
