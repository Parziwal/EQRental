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
using Microsoft.Extensions.Logging;
using System.IO;
using Microsoft.AspNetCore.Hosting;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnEquipmentsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment webHostEnvironment;

        public OwnEquipmentsController(ApplicationDbContext _context, IWebHostEnvironment _webHostEnvironment)
        {
            context = _context;
            webHostEnvironment = _webHostEnvironment;
        }

        [HttpGet]
        public async Task<IEnumerable<EquipmentOverviewDTO>> GetEquipments()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId
                                select new EquipmentOverviewDTO(e, e.Category);
            return await ownEquipments.ToListAsync();
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<OwnEquipmentDTO>> GetEquipment(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var equipmentRentals = from r in context.Rentals
                                   where r.Equipment.OwnerID == userId && r.Equipment.ID == id
                                   select new EquipmentRentalsDTO(r, r.Address.User, r.Address, r.Status, r.Payment);
            var rentList = await equipmentRentals.ToListAsync();
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId && e.ID == id
                                select new OwnEquipmentDTO(e, e.Category, e.Owner, rentList);
            return await ownEquipments.SingleOrDefaultAsync();
        }

        [HttpPost]
        public async Task<ActionResult<int>> PostEquipment([FromForm] EquipmentPostDTO equipment)
        {
            var category = await (from c in context.Categories
                                  where c.Name == equipment.Category
                                  select c).SingleOrDefaultAsync();
            if (category == null)
            {
                return BadRequest();
            }
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;

            var equipmentModel = new Equipment();
            equipmentModel.Name = equipment.Name;
            equipmentModel.Details = equipment.Details;
            equipmentModel.ImagePath = GenerateFilePath(equipment.Image).Result;
            equipmentModel.PricePerDay = equipment.PricePerDay;
            equipmentModel.Available = true;
            equipmentModel.OwnerID = userId;
            equipmentModel.CategoryID = category.ID;

            context.Equipments.Add(equipmentModel);
            await context.SaveChangesAsync();

            return CreatedAtAction("equipmentId", equipmentModel.ID);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var deleteItem = await (from item in context.Equipments
                                    where item.ID == id && userId == item.OwnerID
                                    select item).SingleOrDefaultAsync();

            if (deleteItem == null)
            {
                return NotFound();
            }

            context.Equipments.Remove(deleteItem);
            await context.SaveChangesAsync();

            return NoContent();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, [FromForm] EquipmentPostDTO equipment)
        {
            var category = await (from c in context.Categories
                                  where c.Name == equipment.Category
                                  select c).SingleOrDefaultAsync();
            if (category == null)
            {
                return BadRequest();
            }

            var equipmentModel = await (from e in context.Equipments
                                        where e.ID == id
                                        select e).SingleOrDefaultAsync();

            if (equipmentModel == null)
            {
                return NotFound();
            }

            equipmentModel.Name = equipment.Name;
            equipmentModel.Details = equipment.Details;
            equipmentModel.ImagePath = GenerateFilePath(equipment.Image).Result;
            equipmentModel.PricePerDay = equipment.PricePerDay;
            equipmentModel.Available = true;
            equipmentModel.CategoryID = category.ID;

            await context.SaveChangesAsync();

            return NoContent();
        }

        private async Task<string> GenerateFilePath(IFormFile file)
        {
            string uniqueFileName = null;
            string relativeFilePath = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", "Equipments");
                uniqueFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                relativeFilePath = Path.Combine("Images", "Equipments", uniqueFileName);
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }
            }
            return relativeFilePath;
        }
    }
}