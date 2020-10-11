using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class RentalOverviewDTO
    {
        public int Id { get; set; }
        public string EquipmentName { get; set; }
        public DateTime OrderDate { get; set; }
        public string State { get; set; }

        public RentalOverviewDTO(int id, string equipmentName, DateTime orderDate, string state)
        {
            Id = id;
            EquipmentName = equipmentName;
            OrderDate = orderDate;
            State = state;
        }
    }
}
