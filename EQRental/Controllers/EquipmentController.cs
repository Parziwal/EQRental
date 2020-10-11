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
        public List<EquipmentOverviewDTO> Get()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var equipments = from e in context.Equipments
                    where e.Owner.Id != userId
                    select new EquipmentOverviewDTO(e, e.Category);

            return equipments.ToList();
        }

        [HttpGet("{id}")]
        public Equipment Get(int id)
        {
            var ret = (from e in context.Equipments
                       where e.ID == id
                       select e).FirstOrDefault();
            return ret;
        }

        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }
    }
}
