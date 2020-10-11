using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Controllers
{
    public class OwnEquipmentItem
    {
        public string Name { get; set; }
        public string Owner { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
    }
}
