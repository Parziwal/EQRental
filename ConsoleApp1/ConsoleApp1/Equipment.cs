﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp1
{
    class Equipment
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string Details { get; set; }
        public int PricePerDay { get; set; }
        public bool Available { get; set; }
        public Category Category { get; set; }
        public User Owner { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}