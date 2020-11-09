using EQRental.Models;
using Microsoft.AspNetCore.Connections;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Internal;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace EQRental.Data
{
    class DbInitializer
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = serviceProvider.GetRequiredService<ApplicationDbContext>())
            {
                context.Database.EnsureDeleted();
                context.Database.EnsureCreated();

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
                    new Status{ Name = "PROCESSING" },
                    new Status{ Name = "PREPARING TO SHIP" },
                    new Status{ Name = "UNDER DELIVERING" },
                    new Status{ Name = "DELIVERED" },
                    new Status{ Name = "CANCELED" },
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

                var users = new ApplicationUser[]
                {
                    new ApplicationUser{UserName="alexander33", FullName="Carson Alexander", Email="carson@email.hu", PhoneNumber="06-30-123-1234", BankAccount="10032230-04525279-06720008"},
                    new ApplicationUser{UserName="yanli99", FullName="Yan Li", Email="yan@email.hu", PhoneNumber="06-20-157-1234", BankAccount="12342230-04111279-0623408"},
                    new ApplicationUser{UserName="peggy95", FullName="Peggy Justice", Email="peggy@email.hu", PhoneNumber="06-30-787-1564", BankAccount="18732230-04556279-06720067"},
                    new ApplicationUser{UserName="nini45",FullName="Nini Olivetto", Email="nini@email.hu", PhoneNumber="06-30-163-1984", BankAccount="19832230-04525279-01220087"},
                };

                PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
                foreach (ApplicationUser user in users)
                {
                    user.PasswordHash = passwordHasher.HashPassword(user, "Test.54321");
                    user.NormalizedUserName = user.UserName.ToUpper();
                    user.NormalizedEmail = user.Email.ToUpper();
                }
                
                context.Users.AddRange(users);
                context.SaveChanges();

                var addresses = new UserAddress[]
                {
                    new UserAddress{ PostalCode=1022, City="Budapest", Street="Lévay u. 5", User = users[0] },
                    new UserAddress{ PostalCode=1047, City="Budapest", Street="Báthory u. 2", User = users[1] },
                    new UserAddress{ PostalCode=4032, City="Debrecen", Street="Babér u. 6", User = users[2] },
                    new UserAddress{ PostalCode=6757, City="Szeged", Street="Súnyog út", User = users[3] },
                };

                context.UserAddresses.AddRange(addresses);
                context.SaveChanges();

                var equipments = new Equipment[]
                {
                    new Equipment{Name="Bosch Rotak Lawnmower", PricePerDay=5000,
                        Details=@"The Bosch Rotak 32 R is a lightweight and compact lawnmower with a 32 cm cutting width making it ideal for medium sized lawns up to 150 m and sup2; in size.
                        It has a 1200 W and lsquo; Powerdrive and rsquo; motor that enables you to cut long grass with ease, as well as innovative grass combs, which allows the lawnmower to easily cut up to and over the edge of your lawn ensuring neat and tidy results.",
                        Available=true, Category=categories[3], Owner=users[0]},
                    new Equipment{Name="Nasjac BBQ Barbecue Tool Set", PricePerDay=2000,
                        Details=@"20 pieces of barbecue set, including 8 food fork, 7 skewer, 1 wire cleaning brush, 1 oil brush, 1 grill clamp, 1 roasted spatula, 1 fork grill. The kit contains all the tools you need for a barbecue.",
                        Available=true, Category=categories[3], Owner=users[1]},
                    new Equipment{Name="McCulloch MC1385 Deluxe Canister Steam Cleaner", PricePerDay=10000,
                        Details=@"Naturally deep clean and sanitize without the use of harsh chemicals using hot, pressured steam to eliminate grease, grime, stains, and mold from a wide range of surfaces including ceramic tile, grout, granite, sealed wood flooring, laminate, appliances, grills, autos, and more.",
                        Available=true, Category=categories[1], Owner=users[2]},
                    new Equipment{Name="14 Inch Metal Bed Frame / No Box Spring Needed / Sturdy Steel Frame", PricePerDay=5000,
                        Details=@"Strong and Durable Steel, Imported Stuff.",
                        Available=true, Category=categories[2], Owner=users[2]},
                    new Equipment{Name="HomePop Velvet Swoop Arm Accent Chair, Teal", PricePerDay=14000,
                        Details=@"This elegant accent chair features a slightly curved back and side swoop arms for extra comfort and added design",
                        Available=true, Category=categories[2], Owner=users[2]},
                    new Equipment{Name="Coleman Powersports CT100U Gas Powered Trail Mini-Bike", PricePerDay=2000,
                        Details=@"POWERFUL & EFFICIENT: The 4 stroke OHV 1 cylinder engine with 98cc/3.0HP, will power you through the trails all day with plenty of muscle while still being fuel efficient | Max Speed: 20 mph",
                        Available=true, Category=categories[5], Owner=users[3]}
                };

                context.Equipments.AddRange(equipments);
                context.SaveChanges();

                var rentals = new Rental[]
                {
                    new Rental{ Equipment=equipments[0], OrderDate=new DateTime(2020, 10, 20, 12, 40, 2), StartDate=new DateTime(2020, 11, 1), EndDate=new DateTime(2020, 11, 7),
                    Address=addresses[2], Payment=payments[0], Status=statuses[1] },
                    new Rental{ Equipment=equipments[1], OrderDate=new DateTime(2020, 10, 22, 17, 55, 2), StartDate=new DateTime(2020, 11, 10), EndDate=new DateTime(2020, 11, 13),
                    Address=addresses[2], Payment=payments[1], Status=statuses[0] }
                };

                context.Rentals.AddRange(rentals);
                context.SaveChanges();
            }
        }
    }
}
