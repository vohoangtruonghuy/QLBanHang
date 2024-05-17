using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index()
        {
            var products = db.Products.ToList();
            return View(products);
        }

        public ActionResult Jewelry()
        {
            var ip = db.Products.Where(n => n.CatID == 1).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult Computer()
        {
            var ip = db.Products.Where(n => n.CatID == 2).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult ClothesAndShoes()
        {
            var ip = db.Products.Where(n => n.CatID == 3).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult Camera()
        {
            var ip = db.Products.Where(n => n.CatID == 4).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult Phone()
        {
            var ip = db.Products.Where(n => n.CatID == 5).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult Book()
        {
            var ip = db.Products.Where(n => n.CatID == 7).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult Other()
        {
            var ip = db.Products.Where(n => n.CatID == 8).Take(4).ToList();
            return PartialView(ip);
        }

        public ActionResult SeeDetails(int Masp = 0)
        {
            var chitiet = db.Products.SingleOrDefault(n => n.Id == Masp);
            if (chitiet == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            return View(chitiet);
        }


        public ActionResult SeeCatalogDatails(int? CGId)
        {
            if (CGId == 0)
            {
                // Log the error for debugging
                System.Diagnostics.Debug.WriteLine("CGId is missing or zero");
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Category ID is missing");
            }

            var products = db.Products.Where(n => n.CatID == CGId).ToList();
            return View(products);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}