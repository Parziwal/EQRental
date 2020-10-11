using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class EquipmentOverviewDTO
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Category { get; set; }
        public int PricePerDay { get; set; }

        public EquipmentOverviewDTO(Equipment equipment, Category category)
        {
            Id = equipment.ID;
            Name = equipment.Name;
            Category = category.Name;
            PricePerDay = equipment.PricePerDay;
        }
    }
}
