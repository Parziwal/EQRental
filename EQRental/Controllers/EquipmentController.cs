using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using EQRental.Data;
using EQRental.Models;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace EQRental.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EquipmentController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public EquipmentController(ApplicationDbContext _context)
        {
            context = _context;
        }

        // GET: api/<EquipmentController>
        [HttpGet]
        public List<EquipmentDto> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var x = from e in context.Equipments
                    where e.Owner.Id != userId
                    select e;

            List<EquipmentDto> or = new List<EquipmentDto>();
            foreach (var it in x)
            {
                EquipmentDto o = new EquipmentDto();
                o.ID = it.ID;
                o.Name = it.Name;
                o.ImagePath = it.ImagePath;
                o.PricePerDay = it.PricePerDay;
                or.Add(o);
            }
            return or;
        }

        // GET api/<EquipmentController>/5
        [HttpGet("{id}")]
        public Equipment Get(int id)
        {
            var ret = (from e in context.Equipments
                       where e.ID == id
                       select e).FirstOrDefault();
            return ret;
        }

        // POST api/<EquipmentController>
        [HttpPost]
        public void Post([FromBody] Equipment equipment)
        {
            context.Equipments.Add(equipment);
            context.SaveChanges();
        }

        // PUT api/<EquipmentController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<EquipmentController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
            var toBeDeleted = (from x in context.Equipments
                              where x.ID == id
                              select x).FirstOrDefault();
            if (toBeDeleted != null)
            {
                context.Equipments.Remove(toBeDeleted);
                context.SaveChanges();
            }
        }
    }
}
