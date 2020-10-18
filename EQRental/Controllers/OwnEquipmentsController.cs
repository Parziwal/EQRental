using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EQRental.Data;
using EQRental.Models;
using EQRental.Models.DTO;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnEquipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OwnEquipmentsController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public async Task<List<EquipmentOverviewDTO>> GetEquipments()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId
                                select new EquipmentOverviewDTO(e, e.Category);
            return ownEquipments.ToList();
        }

        [HttpGet("{id}")]
        public async Task<List<EquipmentDTOwRentals>> GetEquipment(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var equipmentRentals = from r in context.Rentals
                                   where r.Equipment.OwnerID == userId && r.Equipment.ID == id
                                   select new RentalDTO(r, r.Equipment, r.Equipment.Category, r.Equipment.Owner, r.Address, r.Status, r.Payment);
            var rentList = equipmentRentals.ToList();
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId && e.ID == id
                                select new EquipmentDTOwRentals(e, e.Category, e.Owner, rentList);
            return ownEquipments.ToList();
        }

        [HttpPut("{id}")]
        public async void PutRental(int id, Equipment equipment, Rental rental)
        {
            equipment.Rentals.Add(rental);
            await context.SaveChangesAsync();
        }

        [HttpPost]
        public async void PostEquipment(Equipment equipment)
        {
            context.Equipments.Add(equipment);
            await context.SaveChangesAsync();
        }

        [HttpDelete("{id}")]
        public async void DeleteEquipment(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var deleteItem = (from item in context.Equipments
                              where item.ID == id && userId == item.OwnerID
                              select item).FirstOrDefault();
            if (deleteItem != null)
            {
                context.Equipments.Remove(deleteItem);
                await context.SaveChangesAsync();
            }
        }
    }
}