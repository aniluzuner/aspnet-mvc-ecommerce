using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eticaret.DAL;
using eticaret.Models;
using System.Web.Security;

namespace eticaret.Controllers
{
    public class MainController : Controller
    {
        private Context db = new Context();


        public Product GetProductById(int id)
        {

            var product = db.Product. FirstOrDefault(p => p.Id == id);

            return product;
        }

        public List<Product> GetRandomProducts(List<Product> products, int count)
        {
            List<Product> randomProducts = new List<Product>();

            Random random = new Random();
            int totalProducts = products.Count;

            if (count >= totalProducts)
            {
                randomProducts = products;
            }
            else
            {
                while (randomProducts.Count < count)
                {
                    int randomIndex = random.Next(0, totalProducts);
                    Product randomProduct = products[randomIndex];

                    if (!randomProducts.Contains(randomProduct) /*&& randomProduct.isApproved == true*/)
                    {
                        randomProducts.Add(randomProduct);
                    }
                }
            }

            return randomProducts;
        }

        // GET: Main
        [Route("")]
        public ActionResult Index()
        {
            ViewBag.HazirSistemler = GetRandomProducts(db.Product.Where(p => p.CategoryID == 3).ToList(), 3);
            ViewBag.HaftaninUrunleri = GetRandomProducts(db.Product.ToList(), 4);
            ViewBag.SizinIcinUrunler = GetRandomProducts(db.Product.ToList(), 8);
            
            return View();
        }

       
        [HttpGet]
        [Route("giris")]
        public ActionResult Login(String ReturnUrl)
        {
          
            if (ReturnUrl == "/Admin")
            {
                return RedirectToAction("AdminLogin");
            }


            return View();
        }

        [HttpPost]
        public ActionResult CustomerLogin(FormCollection form)
        {

            if (!string.IsNullOrEmpty(form["Register"]))
            {
                Customer customer = new Customer();
                customer.Name = form["Name"];
                customer.Surname = form["Surname"];
                customer.Email = form["Email"];
                customer.Password = form["Password"];

                db.Customer.Add(customer);
                db.SaveChanges();
                ViewBag.id = customer.Id;

                return RedirectToAction("Index");
            }

            if (!string.IsNullOrEmpty(form["Login"]))
            {
                //var liste1 = db.Customer.ToList();
                var liste = db.Customer.ToList().Where(Customer => Customer.Email == form["Email"] && Customer.Password == form["Password"]);

                if (liste.Count() != 0)
                {
                    //int customerid = db.Customer.FirstOrDefault(c => c.Email == form["email"]).Id;
                    //string id = Convert.ToString(customerid);

                    FormsAuthentication.SetAuthCookie(Convert.ToString(liste.FirstOrDefault().Id), false);
                    return RedirectToAction("Index");

                }
            }


            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("saticigiris")]
        public ActionResult SellerLogin()
        {
            return View();
        }

        [HttpPost]
        [Route("saticigiris")]
        public ActionResult SellerLogin(FormCollection form)
        {


            if (!string.IsNullOrEmpty(form["Register"]))
            {

                Seller seller = new Seller();
                seller.Name = form["name"];
                seller.Email = form["email"];
                seller.Password = form["password"];

                db.Seller.Add(seller);
                db.SaveChanges();


                //ViewBag.id = customer.Id;

                return RedirectToAction("SelerLogin");
            }


            if (!string.IsNullOrEmpty(form["Login"]))
            {
                //var liste1 = db.Customer.ToList();
                var liste = db.Seller.ToList().Where(Seller => Seller.Email == form["Email"] && Seller.Password == form["Password"]);

                if (liste.Count() != 0)
                {
                    FormsAuthentication.SetAuthCookie(Convert.ToString(liste.FirstOrDefault().Id), false);
                    return RedirectToAction("AddProduct","Seller");

                }
            }


            return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
        }

        [Route("admingiris")]
        public ActionResult AdminLogin()
        {
            return View();
        }
        [HttpPost]
        public ActionResult AdminLoginControl(FormCollection form)
        {

            if (form["username"] =="admin" && form["password"]== "admin")
            {
                FormsAuthentication.SetAuthCookie("admin", false);
                return RedirectToAction("Index","Admin");
            }
            else
            {
                return View();
            }
        }

        [Authorize]
        [Route("cikis")]
        public  ActionResult Logout()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index");
        }

        [HttpGet]
        [Route("urun")]
        public ActionResult Product(int id)
        {
            var Product = GetProductById(Convert.ToInt32(id));

            ViewBag.SellerName = db.Seller.FirstOrDefault(s => s.Id == Product.SellerID).Name;

            return View(Product);
        }

        [HttpGet]
        [Route("urunler")]
        public ActionResult Products(int CategoryID)
        {
            ViewBag.CategoryID = CategoryID;
            ViewBag.CategoryName = db.Category.Where(s => s.Id == CategoryID).FirstOrDefault().Name;
            ViewBag.SubCategories = db.SubCategory.Where(c=> c.ParentCategoryID == CategoryID.ToString()).ToList();
            ViewBag.Brands = db.Brand.ToList();
            ViewBag.Sellers = db.Seller.ToList();

            return View();
        }

        [Route("Sepet")]
        public ActionResult Cart()
        {
            return View();
        }

       
        [Route("Hakkimizda")]
        public ActionResult About()
        {
            return View();
        }

        public class OrderViewModel
        {
            public int OrderId { get; set; }
            public string ProductName { get; set; }
            public int ProductQuantity { get; set; }
            public int TotalPrice { get; set; }
            public string SellerName { get; set; }
            public string Status { get; set; }
            public DateTime Date { get; set; }

        }
        [Authorize]
        public ActionResult Orders()
        {
            var orders = db.Order.ToList();

          
            var orderViewModels = orders.Select(order => new OrderViewModel
            {
                OrderId = order.OrderId,
                ProductName = db.Product.FirstOrDefault(p => p.Id.ToString() == order.ProductID).Name,
                SellerName = db.Seller.FirstOrDefault(s => s.Id == order.SellerId)?.Name,
                ProductQuantity = order.piece,
                TotalPrice = db.Product.FirstOrDefault(p => p.Id.ToString() == order.ProductID).Price * order.piece,
                Status = order.status,  
                Date = order.orderTime
            }).ToList();

            return View(orderViewModels);

        }


        [Authorize]
        public ActionResult CreateOrder()
        {
            var cart = Request.Cookies["cart"];

            if (cart!=null)
            {
                var cartItems = cart.Value.Split(',');
                Random rand = new Random(); 
                int OrderID = rand.Next(10000, 99999);

                foreach (var item in cartItems)
                {
                    var itemParts = item.Split(':');
                    var ProductId = int.Parse(itemParts[0]);
                    var Quantity = int.Parse(itemParts[1]);

                    Order order = new Order();
                    order.OrderId = OrderID;
                    order.ProductID = Convert.ToString(ProductId);
                    order.piece = Quantity;
                    order.SellerId = db.Product.FirstOrDefault(p=> p.Id == ProductId).SellerID;
                    order.CustomerId = Convert.ToInt32(User.Identity.Name);
                    order.status = "Onay Bekliyor";
                    order.orderTime = DateTime.Now;

                    db.Order.Add(order);

                    db.Product.FirstOrDefault(p => p.Id == ProductId).Stock -= Quantity;
                    db.SaveChanges();

                }

            }
            Response.Cookies["cart"].Expires = DateTime.Now.AddDays(-1);
            return RedirectToAction("Orders");


        }


        [HttpGet]
        public JsonResult GetProductJson(int ProductID)
        {
            try
            {
                var product = db.Product.FirstOrDefault(p => p.Id == ProductID);


                return Json(product, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu: " + ex.Message });
            }
        }


        [HttpGet]
        public JsonResult FilterProducts(string SortBy, string CategoryID, string subCategoryID, string Brands, string MinPrice, string MaxPrice, string Rating, string Sellers)
        {
            try
            {
                var query = db.Product.AsQueryable().ToList();

                if (!string.IsNullOrEmpty(CategoryID))
                {
                    query = query.Where(p => Convert.ToInt32(CategoryID) == p.CategoryID).ToList();
                }
                if (!string.IsNullOrEmpty(subCategoryID))
                {
                    query = query.Where(p => Convert.ToInt32(subCategoryID) == p.SubCategoryID).ToList();
                }
                if (!string.IsNullOrEmpty(Brands))
                {
                    query = query.Where(p => Brands.Contains(Convert.ToString(p.BrandID))).ToList();
                }

                if (!string.IsNullOrEmpty(MinPrice))
                {
                    query = query.Where(p => Convert.ToInt32(MinPrice) <= p.Price).ToList();
                }

                if (!string.IsNullOrEmpty(MaxPrice))
                {
                    query = query.Where(p => Convert.ToInt32(MaxPrice) >= p.Price).ToList();
                }

                if (!string.IsNullOrEmpty(Rating))
                {
                    query = query.Where(p => Convert.ToInt32(Rating) <= p.Rating).ToList();
                }

                if (!string.IsNullOrEmpty(Sellers))
                {
                    query = query.Where(p => Sellers.Contains(Convert.ToString(p.SellerID))).ToList();
                }

                if (!string.IsNullOrEmpty(SortBy))
                {

                    switch (SortBy)
                    {
                        case "FiyatArtan":
                            query = query.OrderBy(p => p.Price).ToList();
                            break;
                        case "FiyatAzalan":
                            query = query.OrderByDescending(p => p.Price).ToList();
                            break;
                        case "PuanArtan":
                            query = query.OrderBy(p => p.Rating).ToList();
                            break;
                        case "PuanAzalan":
                            query = query.OrderByDescending(p => p.Rating).ToList();
                            break;

                        default:
                            query = query.OrderBy(p => p.Name).ToList();
                            break;

                    }


                }
                //query = query.Where(p=> p.isApproved ==true).ToList();
                query = query.ToList();

                return Json(query.ToList(), JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Hata oluştu " + ex.Message });
            }
        }


    }
}
