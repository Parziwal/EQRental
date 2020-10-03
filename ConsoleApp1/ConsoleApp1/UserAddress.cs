﻿using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class UserAddress
    {
        public int ID { get; set; }
        public int PostalCode { get; set; }
        public string City { get; set; }
        public string Street { get; set; }
        public User User { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
