using PagedList;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace QuanLyBanHang.Controllers
{
    public class ProductController : Controller
    {
        // GET: Product
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public ActionResult Index(int? page)
        {
            int pageSize = 8;
            int pageNumber = (page ?? 1);

            var products = db.Products.OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        public ActionResult Jewelry(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 1).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult Computer(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 2).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult ClothesAndShoes(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 3).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult Camera(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 4).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult Phone(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 5).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult Book(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 7).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return PartialView(ip);
        }

        public ActionResult Other(int? page)
        {
            int pageSize = 4;
            int pageNumber = (page ?? 1);

            var ip = db.Products.Where(n => n.CatID == 8).OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
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

        public ActionResult Search(string keyword)
        {
            var products = db.Products.Where(p => p.Name.Contains(keyword)).ToList();
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