using System;
using System.Collections.Generic;
using System.Text;

namespace ConsoleApp1
{
    class Status
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Rental> Rentals { get; set; }
    }
}
