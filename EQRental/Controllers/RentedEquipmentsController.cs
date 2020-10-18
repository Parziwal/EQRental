using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EQRental.Data;
using EQRental.Models;
using EQRental.Models.DTO;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class RentedEquipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public RentedEquipmentsController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<RentalOverviewDTO>>> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return NotFound("User not found!");
            }

            var userRentals = from r in context.Rentals
                              where r.Address.User.Id == userId
                              select new RentalOverviewDTO(r.ID, r.Equipment.Name, r.OrderDate, r.Status.Name);
            return await userRentals.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<RentalDTO>> Get(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return NotFound("User not found!");
            }

            var userRental = await (from r in context.Rentals
                              where r.Address.User.Id == userId && r.ID == id
                              select new RentalDTO(r, r.Equipment, r.Equipment.Category, r.Equipment.Owner, r.Address, r.Status, r.Payment))
                              .SingleOrDefaultAsync();

            if(userRental == null)
            {
                return NotFound("Rental not found!");
            }
            return userRental;
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, string status)
        {
            var rentalStatus = await (from s in context.Statuses
                          where s.Name == status.ToUpper()
                          select s).SingleOrDefaultAsync();
            if (rentalStatus == null)
            {
                return BadRequest($"There is no such status as {status}.");
            }

            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (userId == null)
            {
                return NotFound("User not found!");
            }

            var userRental = await (from r in context.Rentals
                             where r.Address.User.Id == userId && r.ID == id
                             select new { Rental = r, Status = r.Status }).SingleOrDefaultAsync();

            bool changed = false;
            if(rentalStatus.Name == "CANCELED" && 
                (userRental.Status.Name == "PROCESSING" || userRental.Status.Name == "PREPARING TO SHIP"))
            {
                userRental.Rental.StatusID = rentalStatus.ID;
                changed = true;
            }
            else if(rentalStatus.Name== "DELIVERED" &&
                (userRental.Status.Name.ToLower() == "UNDER DELIVERING"))
            {
                userRental.Rental.StatusID = rentalStatus.ID;
                changed = true;
            }

            if (changed)
            {
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RentalExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return BadRequest($"{status} status can not be applied to this rental.");
        }

        private bool RentalExists(int id)
        {
            return context.Payments.Any(e => e.ID == id);
        }
    }
}
