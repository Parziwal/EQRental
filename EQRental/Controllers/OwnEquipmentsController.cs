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
                                where e.OwnerID == userId && !e.Deleted
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
            equipmentModel.Deleted = false;
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

            var deleteItem = await (from item in context.Equipments.Include(r => r.Rentals)
                                    where item.ID == id && userId == item.OwnerID
                                    select item).SingleOrDefaultAsync();
                                    
            if (deleteItem == null)
            {
                return NotFound("Could not find equipment with the given id\nor you do not own the equipment.");
            }


            if(!deleteItem.Rentals.Any())
            {
                context.Equipments.Remove(deleteItem);
            }
            else
            {
                var canceledStatus = await (from s in context.Statuses
                                          where s.Name == "CANCELED"
                                          select s).SingleOrDefaultAsync();
                foreach (Rental r in deleteItem.Rentals)
                {
                    r.Status = canceledStatus;
                }
                deleteItem.Deleted = true;
                deleteItem.Available = false;
            }

            string fullImagePath = Path.Combine(webHostEnvironment.WebRootPath, deleteItem.ImagePath);
            if (System.IO.File.Exists(fullImagePath))
                System.IO.File.Delete(fullImagePath);

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
            if (equipment.Image != null)
            {
                equipmentModel.ImagePath = GenerateFilePath(equipment.Image).Result;
            }
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

        [Route("status")]
        [HttpPut]
        public async Task<IActionResult> PutState(int id, string status)
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
                                    where r.Equipment.OwnerID == userId && r.ID == id
                                    select new { Rental = r, Status = r.Status }).SingleOrDefaultAsync();

            bool changed = false;
            if (userRental.Status.Name != "CANCELED")
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
            else
            {
                return BadRequest($"{status} status can not be applied to this rental.");
            }

            return NoContent();
        }


        private bool RentalExists(int id)
        {
            return context.Rentals.Any(e => e.ID == id);
        }
    }
}