using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using POE_MVC.Models;
using POE_MVC.Helpers;

namespace POE_MVC.Controllers
{
    public class DB_UserController : Controller
    {
        private WeatherForecastAppEntities db = new WeatherForecastAppEntities();

        // GET: DB_User/Login
        public ActionResult Login()
        {
            if (Session["Username"] != null)
            {
                return RedirectToAction("Index", "DB_Favourite");
            }
            return View();
        }

        [HttpPost]
        public ActionResult Login(string username, string password)
        {
            string encryptedPassword = Helpers.Encryption.GetMD5Hash(password);
            if(db.DB_User.Any(u => u.Username.Equals(username) && u.Password.Equals(encryptedPassword)))
            {
                Session["Username"] = username;
                return RedirectToAction("Index", "DB_Favourite");
            }
            
            ViewBag.Error = "Incorrect username or password";
            return View();
        }

        // GET: DB_User/Logout
        public ActionResult Logout()
        {
            if (Session["Username"] == null) return RedirectToAction("Login", "DB_User");

            ViewBag.Username = Session["Username"];
            Session.Clear();
            return View();
        }

        // GET: DB_User/Create
        public ActionResult Register()
        {
            return View();
        }

        // POST: DB_User/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Register(string username, string password)
        {

            if(db.DB_User.Any(u => u.Username.Equals(username)))
            {
                return Json(new { error ="Username already taken!"});
            }

            DB_User user = new DB_User();
            user.Username = username;
            user.Password = Encryption.GetMD5Hash(password);
            user.UserType = 0;

            db.DB_User.Add(user);
            db.SaveChanges();

            Session["Username"] = username;
            return Json(new { url = "/DB_Favourite/Index"});
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
