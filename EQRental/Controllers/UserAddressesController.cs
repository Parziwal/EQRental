using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using EQRental.Data;
using EQRental.Models;
using System.Security.Claims;
using EQRental.Models.DTO;
using Microsoft.AspNetCore.Authorization;

namespace EQRental.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class UserAddressesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public UserAddressesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<UserAddressDTO>>> GetUserAddresses()
        {
            string userId = User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier).Value;
            var addresses = await (from a in context.UserAddresses
                                   where a.UserID == userId
                                   select new UserAddressDTO(a)).ToListAsync();
            return addresses;
        }
    }
}
