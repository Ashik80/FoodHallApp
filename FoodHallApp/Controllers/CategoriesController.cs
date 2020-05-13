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
    public class CategoriesController : Controller
    {
        private readonly FoodHallAppContext _context;

        public CategoriesController(FoodHallAppContext context)
        {
            _context = context;
        }

        // GET: Categories
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var model = new CategoryViewModel
            {
                Categories = await _context.Category.ToListAsync()
            };
            return View(model);
        }

        // POST: Categories/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            if (ModelState.IsValid)
            {
                var category = new Category
                {
                    CategoryName = model.CategoryName,
                };
                _context.Category.Add(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Categories/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int cID, CategoryViewModel model)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var category = await _context.Category.FirstOrDefaultAsync(c => c.CategoryID == cID);
            if (category == null)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                category.CategoryName = model.CategoryName;
                _context.Category.Update(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return RedirectToAction(nameof(Index));
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int cID)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var category = await _context.Category.FirstOrDefaultAsync(c => c.CategoryID == cID);
            if(category == null)
            {
                return NotFound();
            }
            else
            {
                _context.Category.Remove(category);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private bool CategoryExists(int id)
        {
            return _context.Category.Any(e => e.CategoryID == id);
        }
    }
}
