﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Rental
    {
        public int ID { get; set; }
        public Equipment Equipment { get; set; }
        public UserAddress Address { get; set; }
        public Status Status { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public DateTime OrderDate { get; set; }
        public Payment Payment { get; set; }
    }
}