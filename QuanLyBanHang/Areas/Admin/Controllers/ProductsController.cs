using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using System.Web.UI;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class ProductsController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Admin/Products
        public ActionResult Index(int? page)
        {
            int pageSize = 10; // Or any desired page size
            int pageNumber = (page ?? 1); // Default to page 1 if no page number is specified
            var products = db.Products.OrderBy(p => p.Id).ToPagedList(pageNumber, pageSize);
            return View(products);
        }

        // GET: Admin/Products/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // GET: Admin/Products/Create
        public ActionResult Create()
        {
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Name");
            ViewBag.SupID = new SelectList(db.Suppliers, "Id", "SupplierName");
            return View();
        }

        // POST: Admin/Products/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,Name,TinyDes,FullDes,Price,CatID,SupID,Quantity")] Product product, HttpPostedFileBase imageFile)
        {
            if (ModelState.IsValid)
            {
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    using (BinaryReader reader = new BinaryReader(imageFile.InputStream))
                    {
                        product.Image = reader.ReadBytes(imageFile.ContentLength);
                    }
                }

                db.Products.Add(product);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.CatID = new SelectList(db.Categories, "Id", "Name", product.CatID);
            ViewBag.SupID = new SelectList(db.Suppliers, "Id", "SupplierName", product.SupID);
            return View(product);
        }

        // GET: Admin/Products/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Name", product.CatID);
            ViewBag.SupID = new SelectList(db.Suppliers, "Id", "SupplierName", product.SupID);
            return View(product);
        }

        // POST: Admin/Products/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit(int id, HttpPostedFileBase imageFile)
        {
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }

            if (ModelState.IsValid)
            {
                // Kiểm tra xem có file ảnh được tải lên không
                if (imageFile != null && imageFile.ContentLength > 0)
                {
                    using (BinaryReader reader = new BinaryReader(imageFile.InputStream))
                    {
                        product.Image = reader.ReadBytes(imageFile.ContentLength);
                    }
                }

                db.Entry(product).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.CatID = new SelectList(db.Categories, "Id", "Name", product.CatID);
            ViewBag.SupID = new SelectList(db.Suppliers, "Id", "SupplierName", product.SupID);
            return View(product);
        }


        // GET: Admin/Products/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Product product = db.Products.Find(id);
            if (product == null)
            {
                return HttpNotFound();
            }
            return View(product);
        }

        // POST: Admin/Products/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Product product = db.Products.Find(id);
            db.Products.Remove(product);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult GetImage(int id)
        {
            var product = db.Products.Find(id);
            if (product != null && product.Image != null)
            {
                return File(product.Image, "image/jpeg"); // Thay "image/jpeg" bằng định dạng hình ảnh thích hợp
            }
            else
            {
                return null; // Hoặc trả về một hình ảnh mặc định
            }
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
