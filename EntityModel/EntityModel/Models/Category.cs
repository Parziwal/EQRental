using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace ConsoleApp1
{
    class Category
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public ICollection<Equipment> Equipments { get; set; }
    }
}
