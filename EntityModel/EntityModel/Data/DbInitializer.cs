using Main;
using Microsoft.EntityFrameworkCore.Internal;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EntityModel.Data
{
    class DbInitializer
    {
        public static void Initialize(EQRentalContext context)
        {
            context.Database.EnsureCreated();

            if (context.Users.Any())
            {
                return;
            }

            var categories = new Category[]
            {
                new Category{ Name = "Electronics & Computers" },
                new Category{ Name = "Kitchen & Home Appliences" },
                new Category{ Name = "Furniture" },
                new Category{ Name = "Garden & Outdoors" },
                new Category{ Name = "Sports & Outdoors" },
                new Category{ Name = "Car & Motorbike" },
                new Category{ Name = "Business, Industry & Science" },
                new Category{ Name = "Other" },
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();

            var statuses = new Status[]
            {
                new Status{ Name = "Processing" },
                new Status{ Name = "Preparing to ship" },
                new Status{ Name = "Shipped" },
                new Status{ Name = "Delivered" },
                new Status{ Name = "Canceled" },
            };

            context.Statuses.AddRange(statuses);
            context.SaveChanges();

            var payments = new Payment[]
            {
                new Payment { Name = "Cash" },
                new Payment { Name = "Remittance" },
            };

            context.Payments.AddRange(payments);
            context.SaveChanges();

            var users = new User[]
            {
                new User{Name="Carson Alexander", Email="carson@email.hu", Password="12345678", PhoneNumber="06-30-123-1234", BankAccount="10032230-04525279-06720008"},
                new User{Name="Yan Li", Email="yan@email.hu", Password="adsafdsf", PhoneNumber="06-20-157-1234", BankAccount="12342230-04111279-0623408"},
                new User{Name="Peggy Justice", Email="peggy@email.hu", Password="jjzgbznuz", PhoneNumber="06-30-787-1564", BankAccount="18732230-04556279-06720067"},
                new User{Name="Nini Olivetto", Email="nini@email.hu", Password="fg4g56h676", PhoneNumber="06-30-163-1984", BankAccount="19832230-04525279-01220087"},
            };

            context.Users.AddRange(users);
            context.SaveChanges();

            var addresses = new UserAddress[]
            {
                new UserAddress{ PostalCode=1022, City="Budapest", Street="Lévay u. 5", UserID = 1 },
                new UserAddress{ PostalCode=1047, City="Budapest", Street="Báthory u. 2", UserID = 2 },
                new UserAddress{ PostalCode=4032, City="Debrecen", Street="Babér u. 6", UserID = 3 },
                new UserAddress{ PostalCode=6757, City="Szeged", Street="Súnyog út", UserID = 4 },
            };

            context.UserAddresses.AddRange(addresses);
            context.SaveChanges();

            var equipments = new Equipment[]
            {
                new Equipment{Name="Bosch Rotak Lawnmower", PricePerDay=5000,
                    Details=@"The Bosch Rotak 32 R is a lightweight and compact lawnmower with a 32 cm cutting width making it ideal for medium sized lawns up to 150 m and sup2; in size.
                    It has a 1200 W and lsquo; Powerdrive and rsquo; motor that enables you to cut long grass with ease, as well as innovative grass combs, which allows the lawnmower to easily cut up to and over the edge of your lawn ensuring neat and tidy results.",
                    Available=true, CategoryID=4, OwnerID=1},
            };

            context.Equipments.AddRange(equipments);
            context.SaveChanges();

            var rentals = new Rental[]
            {
                new Rental{EquipmentId=1, OrderDate=new DateTime(2020, 10, 20), StartDate=new DateTime(2020, 11, 1), EndDate=new DateTime(2020, 11, 7),
                AddressID=3, PaymentID=1, StatusID=2}
            };

            context.Rentals.AddRange(rentals);
            context.SaveChanges();
        }
    }
}
