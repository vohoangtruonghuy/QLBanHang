using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Cryptography;
using System.Web;
using System.Xml.Linq;

namespace QuanLyBanHang.Models
{
    public class Cart
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();
        public int iId { get; set; }
        public string sName { get; set; }
        public string sImage { get; set; }
        public double dPrice { get; set; }
        public int iQuantity { get; set; }
        public double IntoMoney
        {
            get { return iQuantity * dPrice; }
        }
        //Hàm tạo cho giỏ hàng
        public Cart(int PDId)
        {
            iId = PDId;
            Product sp = db.Products.Single(n => n.Id == iId);
            sName = sp.Name;
            sImage = Convert.ToBase64String(sp.Image);
            dPrice = double.Parse(sp.Price.ToString());
            iQuantity = 1;
        }
    }
}