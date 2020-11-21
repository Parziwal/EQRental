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
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!context.Categories.Any(c => c.Name == equipment.Category))
            {
                return BadRequest("You must define the category!");
            }

            if (!(equipment.Image.Length > 0 && equipment.Image.ContentType.Contains("image")))
                return BadRequest("Uploaded file is not image.");

            var equipmentModel = new Equipment();
            equipmentModel.Name = equipment.Name;
            equipmentModel.Details = equipment.Details;
            equipmentModel.ImagePath = await GenerateFilePath(equipment.Image);
            equipmentModel.PricePerDay = equipment.PricePerDay;
            equipmentModel.Available = true;
            equipmentModel.OwnerID = userId;
            equipmentModel.CategoryID = (await context.Categories.FirstAsync(c => c.Name == equipment.Category)).ID;

            context.Equipments.Add(equipmentModel);
            await context.SaveChangesAsync();

            var equipmentOverview = new EquipmentOverviewDTO(equipmentModel, equipmentModel.Category);
            return CreatedAtAction("GetEquipment", equipmentOverview);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEquipment(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var deleteItem = await context.Equipments.FirstOrDefaultAsync(e => e.ID == id && userId == e.OwnerID);
            if (deleteItem == null)
            {
                return NotFound("Could not find equipment with the given id\nor you do not own the equipment.");
            }

            string fullImagePath = Path.Combine(webHostEnvironment.WebRootPath, deleteItem.ImagePath);
            if (System.IO.File.Exists(fullImagePath))
                System.IO.File.Delete(fullImagePath);

            context.Equipments.Remove(deleteItem);
            await context.SaveChangesAsync();

            return Ok("Equipment successfully deleted");
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> PutEquipment(int id, [FromForm] EquipmentPostDTO equipment)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            if (!context.Categories.Any(c => c.Name == equipment.Category))
            {
                return BadRequest("You must define the category!");
            }

            var equipmentModel = await (from e in context.Equipments
                                        where e.ID == id && userId == e.OwnerID
                                        select e).SingleOrDefaultAsync();

            if (equipmentModel == null)
            {
                return NotFound("Could not find equipment with the given id\nor you do not own the equipment.");
            }

            equipmentModel.Name = equipment.Name;
            equipmentModel.Details = equipment.Details;
            equipmentModel.ImagePath = (await GenerateFilePath(equipment.Image));
            equipmentModel.PricePerDay = equipment.PricePerDay;
            equipmentModel.Available = true;
            equipmentModel.CategoryID = (await context.Categories.FirstAsync(c => c.Name == equipment.Category)).ID;

            await context.SaveChangesAsync();

            return Ok("Changes saved!");
        }

        private async Task<string> GenerateFilePath(IFormFile file)
        {
            string relativeFilePath = null;

            if (file != null)
            {
                string uploadsFolder = Path.Combine(webHostEnvironment.WebRootPath, "Images", "Equipments");
                string uniqueFileName = Path.GetFileNameWithoutExtension(file.FileName) + "_" + Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
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