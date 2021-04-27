using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia_ApplicationV1.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialMedia_ApplicationV1.Controllers
{
    public class UserController : Controller
    {
        private readonly DataBaseContext _context;
        private readonly IHostingEnvironment _environment;

        public UserController(DataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;

        }
    // GET: /<controller>/
    public IActionResult Index()
        {
            return View();
        }
        public IActionResult Create()
        {
            return View();
        }
        
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(User user)
        {
            string fileName = string.Empty;
            string path = string.Empty;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(_environment.WebRootPath, "UserImages") + "/" + fileName;
                using (FileStream fs = System.IO.File.Create(path))
                {
                    files[0].CopyTo(fs);
                    fs.Flush();
                }
            }
            user.Image = fileName;
            if (ModelState.IsValid)
            {
                _context.Add(user);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(user);
        }
        public ActionResult Login()
        {
            return View();
        }

        [AllowAnonymous]
        [HttpPost]
        public ActionResult Login(User modal)
        {
            var user = _context.Users.Where(u => u.Email == modal.Email && u.Password == modal.Password).FirstOrDefault();

            if (user != null)
            {
                var userClaims = new List<Claim>()
                {
                    new Claim("Id", user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.Name),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim("Image", user.Image),

                };

                var userIdentity = new ClaimsIdentity(userClaims, "User Identity");

                var userPrincipal = new ClaimsPrincipal(new[] { userIdentity });
                HttpContext.SignInAsync(userPrincipal);

                return RedirectToAction("Index", "Home");
            }
            else
            {
                return View();

            }
        }


    }
}
