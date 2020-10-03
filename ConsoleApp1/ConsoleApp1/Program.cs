using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace ConsoleApp1
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EQRentalContext())
            {
                // Create
                Console.WriteLine("Inserting a new user");
                db.Add(new User { Name = "Teszt Elek", 
                                  Email="t.elek@edu.bme.hu", 
                                  BankAccount= "12345678-12345678-12345678", 
                                  Password="pikachu", 
                                  PhoneNumber="06701234567" });
                try
                {
                    db.SaveChanges();
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }
                // Read
                Console.WriteLine("Querying for a user");
                var user = db.Users
                    .Select(p => p)
                    .First();
                if (user!=null)
                    Console.WriteLine($"{user.Name}, {user.Password}, {user.ID}");
                Console.ReadKey();
            }
        }
    }
}
