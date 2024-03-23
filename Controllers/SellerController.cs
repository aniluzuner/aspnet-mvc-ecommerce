using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using eticaret.DAL;
using eticaret.Models;

namespace eticaret.Controllers
{
    [Authorize]
    public class SellerController : Controller
    {
        private Context db = new Context();

        // GET: Seller
        public ActionResult Index()
        {
            return RedirectToAction("AddProducts");
        }

        public ActionResult Products()
        {
            var SellerId = Convert.ToInt32(User.Identity.Name);
            ViewBag.Urunler = db.Product.Where(c => c.SellerID == SellerId).ToList();
            return View();
        }

        public ActionResult AddProduct()
        {
            ViewBag.Categories = db.Category.ToList();
            ViewBag.SubCategories = db.SubCategory.ToList();
            ViewBag.Brands = db.Brand.ToList();

            return View();
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult AddProduct(FormCollection form)
        {
            Product product = new Product();
            product.BrandID = Convert.ToInt32(form["brand"]);
            product.Name = form["name"];
            product.Price = Convert.ToInt32(form["price"]);
            product.Rating = 0;
            product.Detail = form["description"];
            product.isApproved = false;
            product.Stock = Convert.ToInt32(form["stock"]);
            product.CategoryID = Convert.ToInt32(form["categoryId"]);
            product.SubCategoryID = Convert.ToInt32(form["subcategoryId"]);
            product.SellerID = Convert.ToInt32(form["SellerId"]);

            db.Product.Add(product);
            db.SaveChanges();

            string imagesPath = Server.MapPath("~/Content/Images/ProductImages/") + product.Id; 
            Directory.CreateDirectory(imagesPath);

            for (int i = 1; i <= 3; i++)
            {
                HttpPostedFileBase image = Request.Files["image"+i];
                image.SaveAs(imagesPath + "/" +i+ ".jpg");
            }

            return RedirectToAction("AddProduct");
        }

        public ActionResult DeleteProduct(int? id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();

            Directory.Delete(Server.MapPath("~/Content/Images/ProductImages/") + id);
            return RedirectToAction("Products");

        }
    }
}
