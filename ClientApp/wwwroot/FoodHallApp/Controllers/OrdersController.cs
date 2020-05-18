using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using FoodHallApp.Data;
using FoodHallApp.Models;
using FoodHallApp.Models.ViewModels;
using Microsoft.AspNetCore.Http;

namespace FoodHallApp.Controllers
{
    public class OrdersController : Controller
    {
        private readonly FoodHallAppContext _context;

        public OrdersController(FoodHallAppContext context)
        {
            _context = context;
        }

        // GET: Orders
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var model = new OrderViewModel
            {
                Orders = await _context.Order.Include(o => o.OrderedProducts).ThenInclude(o => o.Product).Include(o => o.Customer).ToListAsync()
            };
            return View(model);
        }

        public async Task<IActionResult> Processed(int id)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var order = await _context.Order.Include(o => o.OrderedProducts).FirstOrDefaultAsync(o => o.OrderID == id);
            foreach (var prod in order.OrderedProducts)
            {
                _context.OrderedProducts.Remove(prod);
            }
            _context.Order.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Order.Any(e => e.OrderID == id);
        }
    }
}
