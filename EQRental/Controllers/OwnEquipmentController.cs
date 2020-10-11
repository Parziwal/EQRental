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

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class OwnEquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OwnEquipmentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        [HttpGet]
        public List<EquipmentOverviewDTO> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId
                                select new EquipmentOverviewDTO(e, e.Category);

            return ownEquipments.ToList();
        }
          
        [HttpGet("{id}")]
        public EquipmentDTO Get(int id)
        {
            //Ez nem igazán megy még... :D 
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Equipments
                                where e.OwnerID == userId && e.ID == id
                                select new EquipmentDTO(e, e.Category, e.Owner);

            return ownEquipments.FirstOrDefault();
        }

        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var deleteItem = (from item in context.Equipments
                              where item.ID == id && userId == item.OwnerID
                              select item).FirstOrDefault();
            if (deleteItem != null)
            {
                context.Equipments.Remove(deleteItem);
                context.SaveChanges();
            }
        }
    }
}
