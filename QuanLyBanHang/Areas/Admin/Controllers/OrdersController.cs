using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using QuanLyBanHang.Models;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class OrdersController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Admin/Orders
        public ActionResult Index()
        {
            var orders = db.Orders.Include(o => o.User);
            return View(orders.ToList());
        }

        // GET: Admin/Orders/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        public ActionResult CTDetails(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order donhang = db.Orders.Find(id);
            var chitiet = db.OrderDetails.Include(d => d.Product).Where(d => d.ID == id).ToList();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(chitiet);
        }

        // GET: Admin/Orders/Create
        public ActionResult Create()
        {
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username");
            return View();
        }

        // POST: Admin/Orders/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Id,OrderDate,Status,Pay,Address,UserID,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Orders.Add(order);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", order.UserID);
            return View(order);
        }

        // GET: Admin/Orders/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", order.UserID);
            return View(order);
        }

        // POST: Admin/Orders/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,OrderDate,Status,Pay,Address,UserID,Total")] Order order)
        {
            if (ModelState.IsValid)
            {
                db.Entry(order).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            ViewBag.UserID = new SelectList(db.Users, "Id", "Username", order.UserID);
            return View(order);
        }

        // GET: Admin/Orders/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order order = db.Orders.Find(id);
            if (order == null)
            {
                return HttpNotFound();
            }
            return View(order);
        }

        // POST: Admin/Orders/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Order order = db.Orders.Find(id);
            db.Orders.Remove(order);
            db.SaveChanges();
            return RedirectToAction("Index");
        }

        public ActionResult Xacnhan(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order donhang = db.Orders.Find(id);
            donhang.Status = 1;
            db.SaveChanges();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return RedirectToAction("Index");
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
