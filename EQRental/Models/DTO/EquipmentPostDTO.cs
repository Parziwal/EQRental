﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EQRental.Models.DTO
{
    public class EquipmentPostDTO
    {
        public string Name { get; set; }
        public string Details { get; set; }
        public string ImagePath { get; set; }
        public int PricePerDay { get; set; }
        public bool Available { get; set; }
        public string Category { get; set; }
    }
}