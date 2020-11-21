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
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EquipmentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentOverviewDTO>>> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var equipments = from e in context.Equipments
                             where e.Owner.Id != userId && e.Available
                             select new EquipmentOverviewDTO(e, e.Category);
            var ret = await equipments.ToListAsync();
            if (ret == null)
                return NotFound("No equipments found that are not yours.");
            return ret;
        }

        [Route("category")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<EquipmentOverviewDTO>>> GetByCategory([FromQuery] string[] categories)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var equipments = from e in context.Equipments
                             where e.Owner.Id != userId && e.Available && categories.Contains(e.Category.Name) 
                             select new EquipmentOverviewDTO(e, e.Category);
            var ret = await equipments.ToListAsync();
            if (ret == null)
                return NotFound();
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EquipmentDTO>> Get(int id)
        {
            var ret = await (from e in context.Equipments
                             where e.ID == id && e.Available
                             select new EquipmentDTO(e, e.Category, e.Owner)).FirstOrDefaultAsync();
            if (ret == null)
                return NotFound("No equipments found with the given id.");
            return ret;
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostEquipment(RentalOrderDTO order)
        {
            Rental _rental = await RentalOrderToRental(order);
            if (_rental == null)
                return BadRequest("Order not valid!");
            context.Rentals.Add(_rental);
            await context.SaveChangesAsync();

            return CreatedAtAction("rentalId", _rental.ID);
        }

        private async Task<Rental> RentalOrderToRental(RentalOrderDTO order)
        {
            if (!checkRentalValidity(order))
                return null;
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var _rental = new Rental
            {
                Payment = await (from p in context.Payments
                                 where p.Name == order.PaymentMethod
                                 select p).FirstOrDefaultAsync(),
                StartDate = order.StartDate,
                EndDate = order.EndDate,
                OrderDate = DateTime.Now,
                EquipmentId = order.EquipmentId,
                Equipment = await (from e in context.Equipments
                                   where e.ID == order.EquipmentId
                                   select e).FirstOrDefaultAsync(),

                Address = await (from uaddr in context.UserAddresses
                                 where uaddr.ID == order.AddressId && uaddr.UserID == userId
                                 select uaddr).FirstOrDefaultAsync(),
                Status = await (from s in context.Statuses
                                where s.Name == "Processing"
                                select s).FirstOrDefaultAsync()
            };
            _rental.PaymentID = _rental.Payment.ID;
            _rental.StatusID = _rental.Status.ID;
            _rental.AddressID = _rental.Address.ID;
            return _rental;
        }

        private bool checkRentalValidity(RentalOrderDTO o)
        {
            if (!context.UserAddresses.Any(ua => ua.ID == o.AddressId) ||
                !context.Payments.Any(p => p.Name == o.PaymentMethod) ||
                !context.Equipments.Any(e => e.ID == o.EquipmentId))
                return false;
            var qRentals = (from r in context.Rentals
                            where r.EquipmentId == o.EquipmentId
                            select r);
            foreach (var r in qRentals)
            {
                if ((o.StartDate >= r.StartDate && o.StartDate <= r.EndDate) ||
                    (o.EndDate >= r.StartDate && o.EndDate <= r.EndDate) ||
                    (o.StartDate <= r.StartDate && o.EndDate >= r.EndDate))
                    return false;
            }
            return true;
        }
    }
}
