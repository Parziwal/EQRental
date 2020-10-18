using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using EQRental.Data;
using EQRental.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EQRental.Controllers
{
    public class UserAddressController : Controller
    {
        private readonly ApplicationDbContext context;

        public UserAddressController(ApplicationDbContext context)
        {
            this.context = context;
        }

        public async Task<IActionResult> Index()
        {
            var addresses = await (from a in context.UserAddresses
                                   select a).ToListAsync();
            return View(addresses);
        }

        [HttpPost]
        public async Task<IActionResult> AddRole(UserAddress userAddress)
        {
            if (userAddress != null)
            {
                await context.UserAddresses.AddAsync(userAddress);
            }
            return RedirectToAction("Index");
        }
    }
}
