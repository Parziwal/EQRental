using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EQRental.Data;
using EQRental.Models;
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

        // GET: api/<OwnEquipmentController>
        [HttpGet]
        public List<OwnEquipmentItem> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Rentals
                                where e.Address.User.Id == userId
                                select new { e.Equipment.Name, e.Equipment.Owner, e.StartDate, e.EndDate };
            List<OwnEquipmentItem> OwnEquipmentList = new List<OwnEquipmentItem>();
            foreach (var Item in ownEquipments)
            {
                OwnEquipmentItem tempItem = new OwnEquipmentItem();
                tempItem.Name = Item.Name;
                tempItem.Owner = Item.Owner.FullName;
                tempItem.StartDate = Item.StartDate;
                tempItem.EndDate = Item.EndDate;
                OwnEquipmentList.Add(tempItem);
            }
            return OwnEquipmentList;
        }
          

        // GET api/<OwnEquipmentController>/5
        [HttpGet("{id}")]
        public OwnEquipmentItem Get(int id)
        {
            //Ez nem igazán megy még... :D 
            string loginUserId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var ownEquipments = from e in context.Rentals
                                where e.Address.User.Id == loginUserId && e.EquipmentId == id
                                select new { e.Equipment.Name, e.Equipment.Owner, e.StartDate, e.EndDate };
            OwnEquipmentItem tempItem = new OwnEquipmentItem();
            tempItem.Name = ownEquipments.First().Name;
            tempItem.Owner = ownEquipments.First().Owner.FullName;
            tempItem.StartDate = ownEquipments.First().StartDate;
            tempItem.EndDate = ownEquipments.First().EndDate;
            return tempItem;
        }

        // POST api/<OwnEquipmentController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<OwnEquipmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<OwnEquipmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var deleteItem = (from item in context.Equipments
                              where item.ID == id
                              select item).FirstOrDefault();
            if (deleteItem != null)
            {
                context.Equipments.Remove(deleteItem);
                context.SaveChanges();
            }
        }
    }
}
