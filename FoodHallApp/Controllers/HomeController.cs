using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using FoodHallApp.Models;
using FoodHallApp.Data;
using FoodHallApp.Models.ViewModels;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Http;

namespace FoodHallApp.Controllers
{
    public class HomeController : Controller
    {
        private readonly FoodHallAppContext _context;

        public HomeController(FoodHallAppContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Signup(string msg)
        {
            ViewData["message"] = msg;
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Signup(UserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == model.UserName);
                if (user != null)
                {
                    return RedirectToAction("Signup", new { msg = "Username exists" });
                }
                else
                {
                    byte[] hash, salt;
                    PasswordHashing(model.Password, out hash, out salt);
                    var newUser = new User
                    {
                        UserName = model.UserName,
                        PasswordHash = hash,
                        PasswordSalt = salt
                    };
                    _context.User.Add(newUser);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }
            return View(model);
        }

        public IActionResult Login(string msg)
        {
            ViewData["msg"] = msg;
            return View();
        }

        public async Task<IActionResult> LoginAct(UserViewModel model)
        {
            var user = await _context.User.FirstOrDefaultAsync(u => u.UserName == model.UserName);
            if (user == null)
            {
                return RedirectToAction("Login", new { msg = "User does not exist" });
            }
            else
            {
                if (CheckPassword(model.Password, user.PasswordHash, user.PasswordSalt))
                {
                    HttpContext.Session.SetString("adminname", user.UserName);
                    return RedirectToAction(nameof(Index),"Categories");
                }
                else
                {
                    return RedirectToAction("Login", new { msg = "Password wrong" });
                }
            }
        }

        private bool CheckPassword(string password, byte[] passwordHash, byte[] passwordSalt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passhash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                for(int i=0; i<passhash.Length; i++)
                {
                    if (passhash[i] != passwordHash[i]) return false;
                }
            }
            return true;
        }

        private void PasswordHashing(string password, out byte[] hash, out byte[] salt)
        {
            using(var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                salt = hmac.Key;
                hash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction(nameof(Login));
        }
    }
}
