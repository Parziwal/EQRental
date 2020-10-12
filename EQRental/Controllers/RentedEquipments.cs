using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using EQRental.Data;
using EQRental.Models;
using EQRental.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentedEquipments : ControllerBase
    {
        private ApplicationDbContext context;

        public RentedEquipments(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public List<RentalOverviewDTO> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userRentals = from r in context.Rentals
                              where r.Address.User.Id == userId
                              select new RentalOverviewDTO(r.ID, r.Equipment.Name, r.OrderDate, r.Status.Name);
            return userRentals.ToList(); 
        }

        [HttpGet("{id}")]
        public RentalDTO Get(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userRental = from r in context.Rentals
                              where r.Address.User.Id == userId && r.ID == id
                              select new RentalDTO(r, r.Equipment, r.Equipment.Category, r.Equipment.Owner, r.Address, r.Status, r.Payment);
            return userRental.First();
        }

        [HttpPut("{id}")]
        public void Put(int id, string status)
        {
            var rentalStatus = (from s in context.Statuses
                          where s.Name.ToLower() == status.ToLower()
                          select s).First();
            if (rentalStatus == null)
                return;

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var userRental = (from r in context.Rentals
                             where r.Address.User.Id == userId && r.ID == id
                             select new { Rental = r, Status = r.Status }).First();
            if (userRental.Status.Name.ToLower() == "canceled")
                return;

            if(rentalStatus.Name.ToLower() == "canceled" && (userRental.Status.Name.ToLower() == "processing" || userRental.Status.Name.ToLower() == "preparing to ship"))
            {
                userRental.Rental.StatusID = rentalStatus.ID;
                context.SaveChanges();
            }
            else if(rentalStatus.Name.ToLower() == "delivered" && (userRental.Status.Name.ToLower() == "under delivering"))
            {
                userRental.Rental.StatusID = rentalStatus.ID;
                context.SaveChanges();
            }
        }
    }
}
