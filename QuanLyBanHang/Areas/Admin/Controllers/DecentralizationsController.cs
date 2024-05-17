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
    public class DecentralizationsController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Admin/Decentralizations
        public ActionResult Index()
        {
            return View(db.Decentralizations.ToList());
        }

        // GET: Admin/Decentralizations/Details/5
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decentralization decentralization = db.Decentralizations.Find(id);
            if (decentralization == null)
            {
                return HttpNotFound();
            }
            return View(decentralization);
        }

        // GET: Admin/Decentralizations/Create
        public ActionResult Create()
        {
            return View();
        }

        // POST: Admin/Decentralizations/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "PermissionId,NamePermission")] Decentralization decentralization)
        {
            if (ModelState.IsValid)
            {
                db.Decentralizations.Add(decentralization);
                db.SaveChanges();
                return RedirectToAction("Index");
            }

            return View(decentralization);
        }

        // GET: Admin/Decentralizations/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decentralization decentralization = db.Decentralizations.Find(id);
            if (decentralization == null)
            {
                return HttpNotFound();
            }
            return View(decentralization);
        }

        // POST: Admin/Decentralizations/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "PermissionId,NamePermission")] Decentralization decentralization)
        {
            if (ModelState.IsValid)
            {
                db.Entry(decentralization).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(decentralization);
        }

        // GET: Admin/Decentralizations/Delete/5
        public ActionResult Delete(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Decentralization decentralization = db.Decentralizations.Find(id);
            if (decentralization == null)
            {
                return HttpNotFound();
            }
            return View(decentralization);
        }

        // POST: Admin/Decentralizations/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            Decentralization decentralization = db.Decentralizations.Find(id);
            db.Decentralizations.Remove(decentralization);
            db.SaveChanges();
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
