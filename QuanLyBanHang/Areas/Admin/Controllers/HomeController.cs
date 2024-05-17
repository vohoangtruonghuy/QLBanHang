using QuanLyBanHang.Models;
using System;
using System.Linq;
using System.Web.Mvc;

namespace QuanLyBanHang.Areas.Admin.Controllers
{
    public class HomeController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        public ActionResult Index()
        {
            var u = Session["use"] as User;
            if (u == null)
            {
                Console.WriteLine("Session user is null.");
                return RedirectPermanent("~/Home/Index");
            }

            Console.WriteLine("Session user: " + u.Username);
            if (u.Decentralization == null)
            {
                Console.WriteLine("User decentralization is null.");
            }
            else
            {
                Console.WriteLine("User permission: " + u.Decentralization.NamePermission);
            }

            if (u.Decentralization != null && u.Decentralization.NamePermission == "Admin")
            {
                ViewBag.TongDoanhThu = ThongKeDoanhThu();
                ViewBag.TongSoLuong = ThongKeSL();
                ViewBag.SoLuongDonHang = ThongKeDonHang();
                ViewBag.SoLuongKhachHang = ThongKeKhachHang();
                Console.WriteLine("Total revenue: " + ViewBag.TongDoanhThu);
                return View();
            }

            return RedirectPermanent("~/Home/Index");
        }

        public decimal ThongKeDoanhThu()
        {
            try
            {
                decimal TongDoanhThu = db.OrderDetails
                    .Where(m => m.PaymentMethods == 1)
                    .Sum(n => (decimal?)n.Quantity * n.Price) ?? 0;
                return TongDoanhThu;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating total revenue: " + ex.Message);
                return 0;
            }
        }

        public int ThongKeSL()
        {
            try
            {
                int TongSoLuong = db.OrderDetails
                    .Where(m => m.PaymentMethods == 1)
                    .Sum(n => (int?)n.Quantity) ?? 0;
                return TongSoLuong;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating total quantity: " + ex.Message);
                return 0;
            }
        }

        public int ThongKeDonHang()
        {
            try
            {
                int SoLuongDonHang = db.Orders.Count();
                return SoLuongDonHang;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating order count: " + ex.Message);
                return 0;
            }
        }

        public int ThongKeKhachHang()
        {
            try
            {
                int SoLuongKhachHang = db.Users.Count();
                return SoLuongKhachHang;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Error calculating customer count: " + ex.Message);
                return 0;
            }
        }
    }
}
