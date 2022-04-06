using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using WebApp.Models;
using WebApp.ViewModels;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using WebApp.Data;

namespace WebApp.Controllers
{
    public class AccountController : Controller
    {
        private readonly ApplicationDbContext _db;

        public AccountController(ApplicationDbContext context)
        {
            _db = context;
        }
        
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email && u.Password == model.Password);
            if (user == null || user.Status != "Active User") return View(model);
            user.LastLoginDate = DateTime.Now;
            await _db.SaveChangesAsync();

            await Authenticate(model.Email);

            return RedirectToAction("Index", "Home");
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (model.Password != model.ConfirmPassword) return View(model);
            var user = _db.Users.FirstOrDefault(u => u.Email == model.Email);

            if (user != null) return View(model);
            var newUser = new User()
            {
                FirstName = model.FirstName,
                LastName = model.LastName,
                Email = model.Email,
                Password = model.Password,
                RegisterDate = DateTime.Now,
                LastLoginDate = DateTime.Now,
                Status = "Active User"
            };
            _db.Users.Add(newUser);
            await _db.SaveChangesAsync();

            await Authenticate(model.Email); 

            return RedirectToAction("Index", "Home");

        }

        private async Task Authenticate(string userName)
        {
           
            var claims = new List<Claim>
            {
                new Claim(ClaimsIdentity.DefaultNameClaimType, userName)
            };
           
            var id = new ClaimsIdentity(claims, "ApplicationCookie", ClaimsIdentity.DefaultNameClaimType, ClaimsIdentity.DefaultRoleClaimType);

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(id));
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Index", "Account");
        }
    }
}