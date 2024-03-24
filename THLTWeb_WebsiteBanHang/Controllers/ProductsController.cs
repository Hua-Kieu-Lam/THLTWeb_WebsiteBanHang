using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using THLTWeb_WebsiteBanHang.Data;
using THLTWeb_WebsiteBanHang.Models;
namespace THLTWeb_WebsiteBanHang.Controllers
{
    public class ProductsController : Controller
    {
        private readonly THLTWeb_WebsiteBanHangContext _context;

        public ProductsController(THLTWeb_WebsiteBanHangContext context)
        {
            _context = context;
        }

        // GET: Products
        public async Task<IActionResult> Index()
        {
            var tHLTWeb_WebsiteBanHangContext = _context.Product.Include(p => p.Category);
            return View(await tHLTWeb_WebsiteBanHangContext.ToListAsync());
        }

        // GET: Products/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Products/Create
        public IActionResult Create()
        {
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name");
            return View();
        }

        // POST: Products/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product, IFormFile imageUrl)
        {
            if (ModelState.IsValid)
            {

                //if (imageUrl != null && imageUrl.Length > 0)
                //{
                //    product.ImageUrl = await SaveImage(imageUrl);
                //}

                //_context.Add(product);
                //await _context.SaveChangesAsync();
                //return RedirectToAction(nameof(Index));
                if (imageUrl != null && imageUrl.Length > 0)
                {
                    try
                    {
                        product.ImageUrl = await SaveImage(imageUrl);
                    }
                    catch (Exception ex)
                    {
                        ModelState.AddModelError("ImageUrl", ex.Message); 
                    }
                }

                if (ModelState.IsValid) 
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
            }

            ViewBag.CategoryId = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }
        private async Task<string> SaveImage(IFormFile image)
        {
            var extension = Path.GetExtension(image.FileName).ToLower();
            if (extension != ".png" && extension != ".jpeg" && extension != ".jpg")
            {
                throw new Exception("Định dạng hình ảnh không hợp lệ. Chỉ chấp nhận .png, .jpeg hoặc .jpg.");
            }
            else
            {

            var savePath = Path.Combine("wwwroot/images", image.FileName);
            using (var fileStream = new FileStream(savePath, FileMode.Create))
            {
                await image.CopyToAsync(fileStream);
            }
            return "/images/" + image.FileName;
            }
            
        }

        // GET: Products/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // POST: Products/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,Price,Description,ImageUrl,CategoryId")] Product product)
        {
            if (id != product.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    //if (imageUrl != null && imageUrl.Length > 0)
                    //{
                    //    try
                    //    {
                    //        product.ImageUrl = await SaveImage(imageUrl);
                    //    }
                    //    catch (Exception ex)
                    //    {
                    //        ModelState.AddModelError("ImageUrl", ex.Message);
                    //    }
                    //}
                    _context.Update(product);
                    await _context.SaveChangesAsync();

                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["CategoryId"] = new SelectList(_context.Category, "Id", "Name", product.CategoryId);
            return View(product);
        }

        // GET: Products/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Product == null)
            {
                return NotFound();
            }

            var product = await _context.Product
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Product == null)
            {
                return Problem("Entity set 'THLTWeb_WebsiteBanHangContext.Product'  is null.");
            }
            var product = await _context.Product.FindAsync(id);
            if (product != null)
            {
                _context.Product.Remove(product);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
          return (_context.Product?.Any(e => e.Id == id)).GetValueOrDefault();
        }
        //public async Task<int> CountProductsByCategory(int categoryId)
        //{
        //    int count = await _context.Product.CountAsync(p => p.CategoryId == categoryId);
        //    return count;
        //}
    }
}
