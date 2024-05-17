using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class CartController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        // GET: GioHang
        //Lấy giỏ hàng 
        public List<Cart> GetCart()
        {
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang == null)
            {
                //Nếu giỏ hàng chưa tồn tại thì mình tiến hành khởi tao list giỏ hàng (sessionGioHang)
                lstGioHang = new List<Cart>();
                Session["GioHang"] = lstGioHang;
            }
            return lstGioHang;
        }
        //Thêm giỏ hàng
        public ActionResult ThemGioHang(int iMasp, string strURL)
        {
            Product sp = db.Products.SingleOrDefault(n => n.Id == iMasp);
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy ra session giỏ hàng
            List<Cart> lstGioHang = GetCart();
            //Kiểm tra sp này đã tồn tại trong session[giohang] chưa
            Cart sanpham = lstGioHang.Find(n => n.iId == iMasp);
            if (sanpham == null)
            {
                sanpham = new Cart(iMasp);
                //Add sản phẩm mới thêm vào list
                lstGioHang.Add(sanpham);
                return Redirect(strURL);
            }
            else
            {
                sanpham.iQuantity++;
                return Redirect(strURL);
            }
        }
        //Cập nhật giỏ hàng 
        public ActionResult CapNhatGioHang(int iMaSP, FormCollection f)
        {
            //Kiểm tra masp
            Product sp = db.Products.SingleOrDefault(n => n.Id == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Cart> lstGioHang = GetCart();
            //Kiểm tra sp có tồn tại trong session["GioHang"]
            Cart sanpham = lstGioHang.SingleOrDefault(n => n.iId == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                sanpham.iQuantity = int.Parse(f["txtSoLuong"].ToString());

            }
            return RedirectToAction("GioHang");
        }
        //Xóa giỏ hàng
        public ActionResult XoaGioHang(int iMaSP)
        {
            //Kiểm tra masp
            Product sp = db.Products.SingleOrDefault(n => n.Id == iMaSP);
            //Nếu get sai masp thì sẽ trả về trang lỗi 404
            if (sp == null)
            {
                Response.StatusCode = 404;
                return null;
            }
            //Lấy giỏ hàng ra từ session
            List<Cart> lstGioHang = GetCart();
            Cart sanpham = lstGioHang.SingleOrDefault(n => n.iId == iMaSP);
            //Nếu mà tồn tại thì chúng ta cho sửa số lượng
            if (sanpham != null)
            {
                lstGioHang.RemoveAll(n => n.iId == iMaSP);

            }
            if (lstGioHang.Count == 0)
            {
                return RedirectToAction("Index", "Home");
            }
            return RedirectToAction("GioHang");
        }
        //Xây dựng trang giỏ hàng
        public ActionResult GioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> lstGioHang = GetCart();
            return View(lstGioHang);
        }
        //Tính tổng số lượng và tổng tiền
        //Tính tổng số lượng
        private int TongSoLuong()
        {
            int iTongSoLuong = 0;
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang != null)
            {
                iTongSoLuong = lstGioHang.Sum(n => n.iQuantity);
            }
            return iTongSoLuong;
        }
        //Tính tổng thành tiền
        private double TongTien()
        {
            double dTongTien = 0;
            List<Cart> lstGioHang = Session["GioHang"] as List<Cart>;
            if (lstGioHang != null)
            {
                dTongTien = lstGioHang.Sum(n => n.IntoMoney);
            }
            return dTongTien;
        }
        //tạo partial giỏ hàng
        public ActionResult GioHangPartial()
        {
            if (TongSoLuong() == 0)
            {
                return PartialView();
            }
            ViewBag.TongSoLuong = TongSoLuong();
            ViewBag.TongTien = TongTien();
            return PartialView();
        }
        //Xây dựng 1 view cho người dùng chỉnh sửa giỏ hàng
        public ActionResult SuaGioHang()
        {
            if (Session["GioHang"] == null)
            {
                return RedirectToAction("Index", "Home");
            }
            List<Cart> lstGioHang = GetCart();
            return View(lstGioHang);

        }

        #region 
        //Xây dựng chức năng đặt hàng
        [HttpPost]
        public ActionResult DatHang(FormCollection donhangForm)
        {
            //Kiểm tra đăng đăng nhập
            if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            Console.WriteLine(donhangForm);
            string diachinhanhang = donhangForm["Address"].ToString();
            string thanhtoan = donhangForm["MaTT"].ToString();
            int ptthanhtoan = Int32.Parse(thanhtoan);

            //Thêm đơn hàng
            Order ddh = new Order();
            User kh = (User)Session["use"];
            List<Cart> gh = GetCart();
            ddh.UserID = kh.Id;
            ddh.OrderDate = DateTime.Now;
            ddh.Total = ptthanhtoan;
            ddh.Address = diachinhanhang;
            decimal tongtien = 0;
            foreach (var item in gh)
            {
                decimal thanhtien = item.iQuantity * (decimal)item.dPrice;
                tongtien += thanhtien;
            }
            ddh.Total = tongtien;
            db.Orders.Add(ddh);
            db.SaveChanges();
            //Thêm chi tiết đơn hàng
            foreach (var item in gh)
            {
                OrderDetail ctDH = new OrderDetail();
                decimal thanhtien = item.iQuantity * (decimal)item.dPrice;
                ctDH.OrderID = ddh.Id;
                ctDH.ProID = item.iId;
                ctDH.Quantity = item.iQuantity;
                ctDH.Price = (decimal)item.dPrice;
                ctDH.Amount = (decimal)thanhtien;
                ctDH.PaymentMethods = 1;
                db.OrderDetails.Add(ctDH);
            }
            db.SaveChanges();
            return RedirectToAction("Index", "Order");
        }
        #endregion

        public ActionResult ThanhToanDonHang()
        {

            ViewBag.MaTT = new SelectList(new[]
                {
                    new { MaTT = 1, TenPT="Thanh toán tiền mặt" },
                    new { MaTT = 2, TenPT="Thanh toán chuyển khoản" },
                }, "MaTT", "TenPT", 1);
            ViewBag.MaNguoiDung = new SelectList(db.Users, "Id", "FullName");

            //Kiểm tra đăng đăng nhập
            if (Session["use"] == null || Session["use"].ToString() == "")
            {
                return RedirectToAction("Login", "Account");
            }
            //Kiểm tra giỏ hàng
            if (Session["GioHang"] == null)
            {
                RedirectToAction("Index", "Home");
            }
            //Thêm đơn hàng
            Order ddh = new Order();
            User kh = (User)Session["use"];
            List<Cart> gh = GetCart();
            decimal tongtien = 0;
            foreach (var item in gh)
            {
                decimal thanhtien = item.iQuantity * (decimal)item.dPrice;
                tongtien += thanhtien;
            }

            ddh.UserID = kh.Id;
            ddh.OrderDate = DateTime.Now;
            OrderDetail ctDH = new OrderDetail();
            ViewBag.tongtien = tongtien;
            return View(ddh);

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
    }
}