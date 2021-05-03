using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using SocialMedia_ApplicationV1.Models;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace SocialMedia_ApplicationV1.Controllers
{
    public class ProductController : Controller
    {
        private readonly DataBaseContext _context;
        [Obsolete]
        private readonly IHostingEnvironment _environment;
        [Obsolete]
        public ProductController(DataBaseContext context, IHostingEnvironment hostingEnvironment)
        {
            _context = context;
            _environment = hostingEnvironment;

        }

        // GET: /<controller>/
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products.ToListAsync());

        }
       

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Product product)
        {
            string fileName = string.Empty;
            string path = string.Empty;
            var files = HttpContext.Request.Form.Files;
            if (files.Count > 0)
            {
                var extension = Path.GetExtension(files[0].FileName);
                fileName = Guid.NewGuid().ToString() + extension;
                path = Path.Combine(_environment.WebRootPath, "ProductImages") + "/" + fileName;
                using (FileStream fs = System.IO.File.Create(path))
                {
                    files[0].CopyTo(fs);
                    fs.Flush();
                }
            }
            product.Image = fileName;
            product.UserId = Convert.ToInt32(User.FindFirst("Id").Value);
            if (ModelState.IsValid)
            {
                _context.Add(product);
                await _context.SaveChangesAsync();
                return RedirectToAction("Index");
            }
            //ViewData["RecipeId"] = new SelectList(_context.Recipes, "Id", "Id", step.RecipeId);
            //ViewData["UserId"] = new SelectList(_context.Users, "Id", "Id", step.UserId);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var prod = await _context.Products
                .FirstOrDefaultAsync(m => m.Id == id);
            if (prod == null)
            {
                return NotFound();
            }

            return View(prod);
        }

        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var pro = await _context.Products
                .Include(r => r.User)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (pro == null)
            {
                return NotFound();
            }

            return View(pro);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var prod = await _context.Products.FindAsync(id);
            _context.Products.Remove(prod);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Product");
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
