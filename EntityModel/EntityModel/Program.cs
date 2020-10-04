using System;
using System.Collections.Generic;
using System.Linq;
using EntityModel.Data;
using Microsoft.EntityFrameworkCore;

namespace Main
{
    class Program
    {
        static void Main(string[] args)
        {
            using (var db = new EQRentalContext())
            {
                DbInitializer.Initialize(db);

                var rental = from rent in db.Rentals
                             join eq in db.Equipments on rent.EquipmentId equals eq.ID
                             join owner in db.Users on eq.OwnerID equals owner.ID
                             join address in db.UserAddresses on rent.AddressID equals address.ID
                             join rentler in db.Users on address.UserID equals rentler.ID
                             select new { Equipment = eq.Name, Owner = owner.Name, Rentler = rentler.Name };

                Console.WriteLine("Equipment: {0}, Owner: {1}, Rentler: {2}", 
                    rental.First().Equipment, rental.First().Owner, rental.First().Rentler);
                
                Console.ReadKey();
            }
        }
    }
}
