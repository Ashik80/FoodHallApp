using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using ClientApp.Data;
using ClientApp.Models;
using ClientApp.Models.ViewModels;
using Microsoft.Extensions.FileProviders;
using System.IO;
using Microsoft.AspNetCore.Http;
using System.Text;

namespace ClientApp.Controllers
{
    public class MenusController : Controller
    {
        private readonly ClientAppContext _context;

        public MenusController(ClientAppContext context)
        {
            _context = context;
        }

        // GET: Menus
        public async Task<IActionResult> Index()
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Client", new { msg = "Login first!" });
            }
            ViewData["cust_id"] = (int)HttpContext.Session.GetInt32("userID");
            var model = new MenuViewModel
            {
                Products = _context.Product.ToList(),
                Categories = await _context.Category.ToListAsync(),
            };
            
            return View(model);
        }

        public async Task<IActionResult> Cartamar(MenuViewModel model)
        {
            model.Carts = await _context.Cart.Where(c => c.CustomerID == HttpContext.Session.GetInt32("userID")).Include(c => c.Product).ToListAsync();
            ViewData["totalprice"] = TotalPriceCart(model.Carts);
            return PartialView("Cartamar",model);
        }

        public int TotalCount(IEnumerable<Cart> carts)
        {
            carts = _context.Cart.Where(c => c.CustomerID == HttpContext.Session.GetInt32("userID"));
            return carts.Count();
        }

        private decimal TotalPriceCart(IEnumerable<Cart> carts)
        {
            return carts.Sum(c => c.Price);
        }

        [HttpPost]
        public async Task<IActionResult> Create(int? pid)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Client", new { msg = "Login first!" });
            }
            var product = await _context.Product.FindAsync(pid);
            var cartItem = await _context.Cart.Where(c => c.CustomerID == HttpContext.Session.GetInt32("userID")).FirstOrDefaultAsync(c => c.ProductID == product.ProductID);
            if (cartItem == null)
            {
                var cartitem = new Cart
                {
                    ProductID = product.ProductID,
                    CustomerID = (int)HttpContext.Session.GetInt32("userID"),
                    Price = product.Price,
                    Quantity = 1
                };
                _context.Cart.Add(cartitem);
                await _context.SaveChangesAsync();
            }
            else
            {
                cartItem.Quantity += 1;
                cartItem.Price = product.Price * cartItem.Quantity;
                _context.Cart.Update(cartItem);
                await _context.SaveChangesAsync();
            }

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> DecreaseQuantity(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Client", new { msg = "Login first!" });
            }
            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.Quantity -= 1;
            if (cart.Quantity > 0)
            {
                cart.Price = cart.Product.Price * cart.Quantity;
                _context.Cart.Update(cart);
            }
            if (cart.Quantity <= 0)
            {
                _context.Cart.Remove(cart);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> IncreaseQuantity(int id)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Client", new { msg = "Login first!" });
            }
            var cart = await _context.Cart
                .Include(c => c.Product)
                .FirstOrDefaultAsync(c => c.ID == id);
            if (cart == null)
            {
                return NotFound();
            }
            cart.Quantity += 1;
            if (cart.Quantity > 0)
            {
                cart.Price = cart.Product.Price * cart.Quantity;
                _context.Cart.Update(cart);
            }
            if (cart.Quantity <= 0)
            {
                _context.Cart.Remove(cart);
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> PlaceOrder(int? customerID)
        {
            if (HttpContext.Session.GetString("username") == null)
            {
                return RedirectToAction("Login", "Client", new { msg = "Login first!" });
            }
            if (customerID != null)
            {
                var cart = await _context.Cart.Where(c => c.CustomerID == customerID).ToListAsync();

                if (cart != null)
                {
                    var order = new Order
                    {
                        CustomerID = (int)customerID,
                        TotalPrice = TotalPriceCart(cart)
                    };
                    order.OrderedProducts = new List<OrderedProducts>();
                    _context.Order.Add(order);
                    foreach (var item in cart)
                    {
                        order.OrderedProducts.Add(new OrderedProducts
                        {
                            OrderID = order.OrderID,
                            ProductID = item.ProductID,
                            Quantity = item.Quantity
                        });
                    }
                }
                foreach (var item in cart)
                {
                    _context.Cart.Remove(item);
                }
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }

    }
}
