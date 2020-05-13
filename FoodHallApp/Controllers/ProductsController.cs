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
using Microsoft.AspNetCore.Hosting;
using System.IO;

namespace FoodHallApp.Controllers
{
    public class ProductsController : Controller
    {
        private readonly FoodHallAppContext _context;
        private readonly IWebHostEnvironment _environment;

        public ProductsController(FoodHallAppContext context, IWebHostEnvironment environment)
        {
            _context = context;
            _environment = environment;
        }

        // GET: Products
        public async Task<IActionResult> Index(int? ProductCategory)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var model = new ProductViewModel();
            ViewData["category"] = new SelectList(_context.Category, "CategoryID", "CategoryName", ProductCategory);

            model.Products =await _context.Product.ToListAsync();
            if (ProductCategory != null)
            {
                model.Products = await _context.Product.Where(p => p.CategoryID == ProductCategory).ToListAsync();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(ProductViewModel model)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            if (ModelState.IsValid)
            {
                string filename = UploadedFile(model.Photo);
                var product = new Product
                {
                    ProductName = model.ProductName,
                    Price = model.Price,
                    CategoryID = model.ProductCategory,
                    Photo = filename
                    
                };
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        private string UploadedFile(IFormFile photo)
        {
            string file = "Pabo";
            if (photo != null)
            {
                string folderPath = Path.Combine(_environment.WebRootPath, "img");
                file = Guid.NewGuid().ToString() + "_" + photo.FileName;
                string filePath = Path.Combine(folderPath, file);
                using(var filestream = new FileStream(filePath, FileMode.Create))
                {
                    photo.CopyTo(filestream);
                }
            }
            return file;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int pID, ProductViewModel model)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var product = await _context.Product.FindAsync(pID);
            if (product == null)
            {
                return NotFound();
            }

            DeleteFile(product.Photo);

            if (ModelState.IsValid)
            {
                string filePath = UploadedFile(model.Photo);
                product.ProductName = model.ProductName;
                product.Price = model.Price;
                product.CategoryID = model.ProductCategory;
                product.Photo = filePath;
                _context.Product.Update(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // POST: Products/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int pID)
        {
            if (HttpContext.Session.GetString("adminname") == null)
            {
                return RedirectToAction("Login", "Home", new { msg = "Login first!" });
            }
            var product = await _context.Product.FindAsync(pID);
            if(product == null)
            {
                return NotFound();
            }
            else
            {
                DeleteFile(product.Photo);
                _context.Product.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
        }

        private void DeleteFile(string photo)
        {
            string path = Path.Combine(_environment.WebRootPath, "img");
            string filepath = Path.Combine(path, photo);
            System.IO.File.Delete(filepath);
        }

        private bool ProductExists(int id)
        {
            return _context.Product.Any(e => e.ProductID == id);
        }
    }
}
