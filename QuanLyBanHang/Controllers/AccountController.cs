using QuanLyBanHang.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;

namespace QuanLyBanHang.Controllers
{
    public class AccountController : Controller
    {
        private QuanLyBanHangEntities db = new QuanLyBanHangEntities();

        // GET: Account/Register
        public ActionResult Register()
        {
            return View();
        }

        // POST: Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(User user)
        {
            try
            {
                Session["userReg"] = user;

                // If data is valid, proceed with registration
                if (ModelState.IsValid)
                {
                    var check = db.Users.FirstOrDefault(s => s.Email == user.Email);
                    if (check == null)
                    {
                        // Adding a new user
                        db.Users.Add(user);
                        db.SaveChanges();

                        ViewBag.RegOk = "Đăng kí thành công. Đăng nhập ngay";
                        ViewBag.isReg = true;
                        return View("Register");
                    }
                    else
                    {
                        ViewBag.RegOk = "Email đã tồn tại! Vui lòng chọn 1 email khác";
                        return View("Register");
                    }
                }
                else
                {
                    return View("Register");
                }
            }
            catch
            {
                return View();
            }
        }

        [AllowAnonymous]
        public ActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Login(FormCollection userlog)
        {
            string userMail = userlog["userMail"].ToString();
            string password = userlog["password"].ToString();
            var islogin = db.Users.SingleOrDefault(x => x.Email == userMail && x.Password == password);

            if (islogin != null)
            {
                Session["use"] = islogin;

                if (userMail == "votruonghuy2004@gmail.com")
                {
                    return RedirectToAction("Index", "Admin/Home");
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            else
            {
                ViewBag.Fail = "Tài khoản hoặc mật khẩu không chính xác.";
                return View("Login");
            }
        }

        public ActionResult LogOut()
        {
            Session["use"] = null;
            return RedirectToAction("Index", "Home");
        }

        public new ActionResult Profile(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            return View(user);
        }

        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            User user = db.Users.Find(id);
            if (user == null)
            {
                return HttpNotFound();
            }
            ViewBag.IDQuyen = new SelectList(db.Decentralizations, "PermissionId", "NamePermission", user.PermissionId);
            return View(user);
        }

        // POST: Account/Edit
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Username,Password,FullName,Address,Email,DayOfBirth,PermissionId")] User user)
        {
            if (ModelState.IsValid)
            {
                try
                {
                    db.Entry(user).State = EntityState.Modified;
                    db.SaveChanges();
                    return RedirectToAction("Profile", new { id = user.Id });
                }
                catch (DbEntityValidationException ex)
                {
                    foreach (var validationErrors in ex.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            var errorMessage = $"Property: {validationError.PropertyName}, Error: {validationError.ErrorMessage}";
                            // Log the error (consider using a logging framework)
                            // Example: logger.Error(errorMessage);
                        }
                    }
                }
            }
            ViewBag.Id = new SelectList(db.Decentralizations, "PermissionId", "NamePermission", user.PermissionId);
            return View(user);
        }

        public static byte[] encryptData(string data)
        {
            System.Security.Cryptography.MD5CryptoServiceProvider md5Hasher = new System.Security.Cryptography.MD5CryptoServiceProvider();
            byte[] hashedBytes;
            System.Text.UTF8Encoding encoder = new System.Text.UTF8Encoding();
            hashedBytes = md5Hasher.ComputeHash(encoder.GetBytes(data));
            return hashedBytes;
        }

        public static string md5(string data)
        {
            return BitConverter.ToString(encryptData(data)).Replace("-", "").ToLower();
        }
    }
}
