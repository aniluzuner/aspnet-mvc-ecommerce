using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.Routing;
using eticaret.DAL;
using eticaret.Models;

namespace eticaret.Controllers
{
    public class AdminAuthorizeAttribute : ActionFilterAttribute
    {
        public override void OnActionExecuting(ActionExecutingContext filterContext)
        {
           if (filterContext.HttpContext.User.Identity.Name != "admin")
           {
                // Kullanıcı admin değil, girişine izin verme
                //filterContext.Result = new HttpStatusCodeResult(HttpStatusCode.Forbidden);
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Main", action = "AdminLogin" }));
           }
           else
           {
                base.OnActionExecuting(filterContext);
                //filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Admin", action = "Index" }));

           }
        }
    }

    [AdminAuthorize]
    public class AdminController : Controller
    {
        private Context db = new Context();

        // GET: Admin

        public ActionResult Index()
        {
            DateTime simdi = DateTime.Now;
            DateTime bugun = simdi.AddDays(-1);
            DateTime hafta = simdi.AddDays(-7);
            DateTime ay = simdi.AddDays(-30);
            DateTime yil = simdi.AddDays(-365);


            ViewBag.MagazaIstekleri = db.Seller.Where(s => s.isApproved == false).ToList().Count();
            ViewBag.UrunIstekleri = db.Product.Where(p => p.isApproved == false).ToList().Count();
            ViewBag.MusteriSayisi = db.Customer.ToList().Count();
            ViewBag.gunluk = 0;

            foreach (var Order in db.Order.Where(o => o.orderTime >= bugun).ToList())
            {
                ViewBag.gunluk += db.Product.FirstOrDefault(p =>  p.Id.ToString() == Order.ProductID).Price * Order.piece;
            }
            ViewBag.gunluk /= 20; 

            ViewBag.haftalik = 0;

            foreach (var Order in db.Order.Where(o => o.orderTime >= hafta).ToList())
            {
                ViewBag.haftalik += db.Product.FirstOrDefault(p =>  p.Id.ToString() == Order.ProductID).Price * Order.piece;
            }
            ViewBag.haftalik /= 20;

            ViewBag.aylik = 0;

            foreach (var Order in db.Order.Where(o => o.orderTime >= ay).ToList())
            {
                ViewBag.aylik += db.Product.FirstOrDefault(p =>  p.Id.ToString() == Order.ProductID).Price * Order.piece;
            }
            ViewBag.aylik /= 20;


            ViewBag.yillik = 0;

            foreach (var Order in db.Order.Where(o => o.orderTime >= yil).ToList())
            {
                ViewBag.yillik += db.Product.FirstOrDefault(p =>  p.Id.ToString() == Order.ProductID).Price * Order.piece;
            }
            ViewBag.yillik /= 20;


            ViewBag.toplam = 0;

            foreach (var Order in db.Order.ToList())
            {
                ViewBag.toplam += db.Product.FirstOrDefault(p =>  p.Id.ToString() == Order.ProductID).Price * Order.piece;
            }
            ViewBag.toplam /= 20;

            return View();
        }

        public ActionResult Products()
        {
            ViewBag.OnayliUrunler = db.Product.Where(p => p.isApproved == true).ToList();
            ViewBag.OnaysizUrunler = db.Product.Where(p => p.isApproved == false).ToList();

            return View();
        }

        public ActionResult DeleteProduct(int? id)
        {
            Product product = db.Product.Find(id);
            db.Product.Remove(product);
            db.SaveChanges();

            Directory.Delete(Server.MapPath("~/Content/Images/ProductImages/") + id);
            return RedirectToAction("Products");
        }
        
        public ActionResult ProductApprove(int? id)
        {
            Product product = db.Product.Find(id);
            product.isApproved = true;
            db.SaveChanges();

            return RedirectToAction("Products");
        }

        public ActionResult Sellers()
        {
            ViewBag.OnayliMagazalar = db.Seller.Where(p => p.isApproved == true).ToList();
            ViewBag.OnaysizMagazalar = db.Seller.Where(p => p.isApproved == false).ToList();

            return View();
        }

        public ActionResult DeleteSeller(int? id)
        {
            Seller seller = db.Seller.Find(id);
            db.Seller.Remove(seller);
            db.SaveChanges();

            return RedirectToAction("Sellers");
        }

        public ActionResult SellerApprove(int? id)
        {
            Seller seller = db.Seller.Find(id);
            seller.isApproved = true;
            db.SaveChanges();

            return RedirectToAction("Sellers");
        }
    }
}
