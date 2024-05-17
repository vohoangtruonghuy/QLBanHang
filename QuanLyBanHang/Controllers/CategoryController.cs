using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class CategoryController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: Category
        public ActionResult CategoryPartial()
        {
            //var danhmuc = db.LoaiHangs.ToList();
            //return PartialView(danhmuc);

            var danhmuc = db.Categories.Where(n => n.Name != "Thùng rác").Take(7).ToList();
            return PartialView(danhmuc);
        }
    }
}