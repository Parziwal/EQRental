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
                             where e.Owner.Id != userId
                             select new EquipmentOverviewDTO(e, e.Category);
            var ret = await equipments.ToListAsync();
            if (ret == null)
                return NotFound();
            return ret;
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Equipment>> Get(int id)
        {
            var ret = await (from e in context.Equipments
                             where e.ID == id
                             select e).FirstOrDefaultAsync();
            if (ret == null)
                return NotFound();
            return ret;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpPost]
        public async Task<ActionResult<Equipment>> PostEquipment(RentalOrderDTO order)
        {
            Rental _rental = await RentalOrderToRental(order);
            context.Rentals.Add(_rental);
            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<Rental> RentalOrderToRental(RentalOrderDTO order)
        {
            var _rental = new Rental();
            _rental.Payment = await (from p in context.Payments
                                     where p.Name == order.PaymentMethod
                                     select p).FirstOrDefaultAsync();
            _rental.StartDate = order.StartDate;
            _rental.EndDate = order.EndDate;
            _rental.OrderDate = DateTime.Today;
            _rental.EquipmentId = order.EquipmentId;
            _rental.Equipment = await (from e in context.Equipments
                                       where e.ID == order.EquipmentId
                                       select e).FirstOrDefaultAsync();
            _rental.Address = await (from uaddr in context.UserAddresses
                                     where uaddr.ID == order.AddressId
                                     select uaddr).FirstOrDefaultAsync();
            _rental.Status = await (from s in context.Statuses
                                    where s.Name == "Processing"
                                    select s).FirstOrDefaultAsync();
            _rental.PaymentID = _rental.Payment.ID;
            _rental.StatusID = _rental.Status.ID;
            _rental.AddressID = _rental.Address.ID;
            return _rental;
        }
    }
}
