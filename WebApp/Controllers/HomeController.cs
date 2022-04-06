using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using WebApp.Data;
using WebApp.Models;

namespace WebApp.Controllers
{
    public class HomeController : Controller
    {

        private readonly ApplicationDbContext _db;

        public HomeController(ApplicationDbContext context)
        {
            _db = context;
        }

        [Authorize]
        public IActionResult Index()
        {

            if (_db.Users.Any(u => User.Identity != null && u.Email == User.Identity.Name))
            {

                if (_db.Users.First(u => User.Identity != null && u.Email == User.Identity.Name).Status != "Blocked User")
                    return View(_db.Users);
                else
                    return RedirectToAction("Login", "Account");

            }
            else
            {
                HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return RedirectToAction("Login", "Account");
            }

            
        }

        public IActionResult Delete(int[] Ids)
        {

            if (IsUserInvalid(User.Identity.Name))
                return RedirectToAction("Login", "Account");

            foreach (int id in Ids)
            {
                var objectToDelete = _db.Users.Find(id);
                _db.Users.Remove(objectToDelete);
            }

            _db.SaveChanges();

            return Redirect("~/");

        }

        public bool IsUserInvalid(string? Email)
        {

            User user = _db.Users.First(u => u.Email == Email);

            if (user.Status == "Active User")
                return false;
            else
                return true;

        }

        public IActionResult Block(int[] Ids)
        {

            if (IsUserInvalid(User.Identity.Name))
                return RedirectToAction("Login", "Account");

            foreach (int id in Ids)
            {
                var ObjectToDelete = _db.Users.Find(id);
                _db.Users.Find(id).Status = "Blocked User";
            }

            _db.SaveChanges();

            return Redirect("~/");

        }

        public IActionResult Unblock(int[] Ids)
        {

            if (IsUserInvalid(User.Identity?.Name))
                return RedirectToAction("Login", "Account");

            foreach (int id in Ids)
            {
                var objectToDelete = _db.Users.Find(id);
                _db.Users.Find(id).Status = "Active User";
            }

            _db.SaveChanges();

            return Redirect("~/");

        }

    }
}
