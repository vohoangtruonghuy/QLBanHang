﻿using PagedList;
using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class OrderController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Order
        public ActionResult Index(int? page)
        {
            int pageSize = 10; 
            int pageNumber = (page ?? 1);
            if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            User kh = (User)Session["use"];
            int maND = kh.Id;
            var donhangs = db.Orders.Where(d => d.UserID == maND).OrderByDescending(d => d.OrderDate);
            var pagedDonhangs = donhangs.ToPagedList(pageNumber, pageSize);

            return View(pagedDonhangs);
        }

        // GET: Donhangs/Details/5
        //Hiển thị chi tiết đơn hàng
        public ActionResult Details(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Order donhang = db.Orders.Find(id);
            var chitiet = db.OrderDetails.Where(d => d.OrderID == id).ToList();
            if (donhang == null)
            {
                return HttpNotFound();
            }
            return View(chitiet);
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