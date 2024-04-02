using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Globalization;
using THLTWeb_WebsiteBanHang.Data;
using THLTWeb_WebsiteBanHang.Models;

namespace THLTWeb_WebsiteBanHang.Controllers
{
    public class ShoppingCartController : Controller
    {
        private readonly THLTWeb_WebsiteBanHangContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public ShoppingCartController(THLTWeb_WebsiteBanHangContext context, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _context = context;
            _userManager = userManager;
            _signInManager = signInManager;
        }
        List<CartItem>? GetCartItems()
        {
            string jsoncart = HttpContext.Session.GetString("cart");
            if (jsoncart != null)
            {
                return JsonConvert.DeserializeObject<List<CartItem>>(jsoncart);
            }
            return new List<CartItem>();
        }
        void SaveCartSession(List<CartItem> ls)
        {
            string jsoncart = JsonConvert.SerializeObject(ls);
            HttpContext.Session.SetString("cart"

            , jsoncart);

        }

        public async Task<IActionResult> AddToCart(int id)
        {
            if (!User.Identity.IsAuthenticated)
            {
                return Redirect("/Identity/Account/Login");
            }
            Product? itemProduct = _context.Product.FirstOrDefault(p => p.Id == id);
            if (itemProduct == null)
                return BadRequest("Sản phẩm không tồn tại");
            var user=  _userManager.GetUserAsync(User);
            var carts = GetCartItems();
            var findCartItem = carts.FirstOrDefault(p => p.Id.Equals(id));
            if (findCartItem == null)
            {
                findCartItem = new CartItem()
                {
                    Id = itemProduct.Id,
                    Name = itemProduct.Name,
                    Price = itemProduct.Price,
                    Quantity = 1,
                    UserId = user.Id,
                };
                carts.Add(findCartItem);
            }
            else
                findCartItem.Quantity++;
            SaveCartSession(carts);
            return RedirectToAction("Index");
        }

        public ActionResult UpdateCart(int id, int quantity)
        {
            var carts = GetCartItems();
            var findCartItem = carts.FirstOrDefault(p => p.Id == id);
            if (findCartItem != null)
            {
                findCartItem.Quantity = quantity;
                SaveCartSession(carts);
            }
            return RedirectToAction("Index");
        }
        public ActionResult DeleteCart(int id)
        {
            var carts = GetCartItems();
            var findCartItem = carts.FirstOrDefault(p => p.Id == id);
            if (findCartItem != null)
            {
                carts.Remove(findCartItem);
                SaveCartSession(carts);
            }
            return RedirectToAction("Index");
        }

        public async Task<IActionResult> Order()
        {
            try
            {
                var carts = GetCartItems();
                var user = await _userManager.GetUserAsync(User);
                var order = new Order
                {
                    UserId = user != null ? user.Id : null,
                    OrderDate = DateTime.UtcNow,
                    TotalPrice = carts.Sum(i => i.Price * i.Quantity),
                    OrderDetails = carts.Select(item => new OrderDetail
                    {
                        ProductId = item.Id,
                        Quantity = item.Quantity,
                        Price = item.Price
                    }).ToList()
                };
                _context.Order.Add(order);
                await _context.SaveChangesAsync();

                HttpContext.Session.Remove("cart");
                return View("OrderCompleted", order);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public async Task<IActionResult> Logout()
        {
            HttpContext.Session.Clear();

            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Products");
        }
        // GET: ShoppingCartController
        public ActionResult Index()
            {
                var carts = GetCartItems();
                ViewBag.TongTien = carts.Sum(p => p.Price * p.Quantity);
                ViewBag.TongSoLuong = carts.Sum(p => p.Quantity);
                return View(carts);
            } 

        // GET: ShoppingCartController/Details/5
        public ActionResult Details(int id)
        {
            return View();
        }

        // GET: ShoppingCartController/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: ShoppingCartController/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingCartController/Edit/5
        public ActionResult Edit(int id)
        {
            return View();
        }

        // POST: ShoppingCartController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        // GET: ShoppingCartController/Delete/5
        public ActionResult Delete(int id)
        {
            return View();
        }

        // POST: ShoppingCartController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
