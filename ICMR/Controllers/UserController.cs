using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using ICMR.Models;
using ICMR;
using System.Web.Security;

namespace ICMR.Controllers
{
    public class UserController : Controller
    {

        private ICMREntities2 db = new ICMREntities2();
        // GET: User
        public ActionResult Index()
        {
            return View();
        }
        [NoCache]
        public ActionResult UserHome()
        {
            if (Session["uid"] == null)
            {
                return RedirectToAction("UserLogin", "User");
            }

            return View();
        }


        [HttpGet]
        public ActionResult UserLogin()
        {
            return View();
        }

        [HttpGet]
        public ActionResult EditProfile()
        {
            string uid = Session["uid"]?.ToString();

            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("UserLogin", "User");

            var user = db.Master_icmr.FirstOrDefault(x => x.userid == uid);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

        public ActionResult CheckUserAndRedirect()
        {
            if (Session["uid"] != null)
            {
                // User is logged in
                return RedirectToAction("ConfirmPayment", "Home");
            }
            else
            {
                // User is not logged in
                return RedirectToAction("UserLogin", "User");
            }
        }



        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult EditProfile(Master_icmr model)
        {
            if (ModelState.IsValid)
            {
                var user = db.Master_icmr.FirstOrDefault(x => x.userid == model.userid);

                if (user != null)
                {
                    user.first_name = model.first_name;
                    user.last_name = model.last_name;
                    user.add1 = model.add1;
                    user.city = model.city;
                    user.state = model.state;
                    user.PIN = model.PIN;
                    user.country = model.country;
                    user.mobile_phone = model.mobile_phone;
                    user.email = model.email;
                    user.OrgGST = model.OrgGST;
                    user.Password = model.Password;

                    db.SaveChanges();

                    ViewBag.Message = "Profile updated successfully!";
                    return RedirectToAction("UserProfile");
                }

                ModelState.AddModelError("", "User not found.");
            }

            return View(model);
        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult UserLogin(Master_icmr model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var user = db.Master_icmr.FirstOrDefault(u => u.email == model.email && u.Password == model.Password);

            if (user != null)
            {
                Session["uid"] = user.userid;
                Session["userEmail"] = user.email;
                return RedirectToAction("UserHome", "User");
            }
            else
            {
                ViewBag.Message = "Invalid email or password.";
                return View(model);
            }
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();

            return RedirectToAction("UserLogin", "User");
        }

        public  ActionResult UserDashboard()
        {

            if (Session["uid"] != null)
            {
                string userId = (Session["uid"].ToString());

                List<proccas> userDetails = db.proccases.Where(u => u.userid == userId.ToString()).ToList(); // Remove .ToString() if userid is int
                
                return View(userDetails);
            }
            else
            {
                return RedirectToAction("UserLogin", "User");
            }
        }

        [HttpGet]
        public ActionResult ChangePassword()
        {

            string uid = Session["uid"]?.ToString();

            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("UserLogin", "User");

            return View();
        }

        [HttpPost]
        public ActionResult ChangePassword(string NewPassword, string ConfirmPassword)
        {
            if (NewPassword != ConfirmPassword)
            {
                ViewBag.Message = "Passwords do not match!";
                return View();
            }

            string userId = Session["uid"]?.ToString();

            if (!string.IsNullOrEmpty(userId))
            {
                var user = db.Master_icmr.FirstOrDefault(x => x.userid == userId);

                if (user != null)
                {
                    user.Password = NewPassword;
                    db.SaveChanges();
                    ViewBag.Message = "Password changed successfully!";
                }
                else
                {
                    ViewBag.Message = "User not found.";
                }
            }
            else
            {
                ViewBag.Message = "Session expired. Please login again.";
            }

            return View();
        }


        public ActionResult UserProfile()
        {
            string uid = Session["uid"]?.ToString();

            if (string.IsNullOrEmpty(uid))
                return RedirectToAction("UserLogin", "User");

            var user = db.Master_icmr.FirstOrDefault(x => x.userid == uid);

            if (user == null)
                return HttpNotFound();

            return View(user);
        }

    }
}