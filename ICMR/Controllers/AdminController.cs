using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using CaptchaMvc.HtmlHelpers;
using ICMR.Models;

namespace ICMR.Controllers
{
    public class AdminController : BaseController
    {

        private ICMREntities2 db = new ICMREntities2();

        public class NoCacheAttribute : ActionFilterAttribute
        {
            public override void OnResultExecuting(ResultExecutingContext filterContext)
            {
                filterContext.HttpContext.Response.Cache.SetExpires(DateTime.UtcNow.AddDays(-1));
                filterContext.HttpContext.Response.Cache.SetValidUntilExpires(false);
                filterContext.HttpContext.Response.Cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
                filterContext.HttpContext.Response.Cache.SetCacheability(HttpCacheability.NoCache);
                filterContext.HttpContext.Response.Cache.SetNoStore();

                base.OnResultExecuting(filterContext);
            }
        }

       
        [HttpGet]
        public ActionResult AdminLogin()
        {
            return View(new ICMRLogin()); 
        }

        [HttpPost]
        public ActionResult AdminLogin(ICMRLogin model) 
        {
            if (ModelState.IsValid)
            {
                var user = db.ICMRLogins.FirstOrDefault(u => u.EmailId == model.EmailId && u.Password == model.Password);

                if (user != null)
                {
                    if (user.Role == "Admin")
                    {

                        Session["UserEmail"] = user.EmailId;
                        Session["UserRole"] = user.Role;
                        Session["UserId"] = user.EmpId;
                        Session["Name"] = user.Name;

                        return RedirectToAction("AdminHome", "Admin");
                    }
                    else
                    {
                        ViewBag.Message = "You are not authorized to access the Admin panel.";
                        return View();
                    }
                }
                else
                {
                    ViewBag.Message = "Invalid Email ID or Password.";
                    return View();
                }
            }

            return View();
        }

        public ActionResult Logout()
        {
            Session.Clear();
            Session.Abandon();
            FormsAuthentication.SignOut();


            return RedirectToAction("AdminLogin"); 
        }

        [NoCache]
        public ActionResult AdminHome()
        {
            if (Session["UserRole"] == null)
                return RedirectToAction("AdminLogin", "Admin");

            return View();
        }



        [HttpGet]
        public ActionResult Create()
        {
            return View();
        }

       

        public ActionResult Events()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var events = db.ICMREvents.ToList();
            return View(events);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }
        public ActionResult AddEvents()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddEvents(ICMREvent model, HttpPostedFileBase ImageFile, HttpPostedFileBase PdfFile)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddEvents");
            }
            if (ModelState.IsValid)
            {
                string createdBy = Session["UserRole"] != null ? Session["UserRole"].ToString() : "Unknown";

                ICMREvent personalInfo = new ICMREvent
                {
                    Title = model.Title,
                    Type = model.Type,
                    EventDescription = model.EventDescription,
                    EventDate = model.EventDate,
                    StartDate = model.StartDate,
                    EndDate = model.EndDate,
                    EventUrl = model.EventUrl,
                    AltTagDescription = model.AltTagDescription,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                };

                db.ICMREvents.Add(personalInfo);
                db.SaveChanges();

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string imageExt = Path.GetExtension(ImageFile.FileName);
                    string imageName = $"{personalInfo.EventId}_{Path.GetFileName(ImageFile.FileName)}";
                    string imagePath = Path.Combine(Server.MapPath("~/Uploads/Images/"), imageName);

                    ImageFile.SaveAs(imagePath);
                    personalInfo.Image = imageName;
                }

                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{personalInfo.EventId}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    PdfFile.SaveAs(pdfPath);
                    personalInfo.Pdf = pdfName;
                }

                db.Entry(personalInfo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Event added successfully!";
                return RedirectToAction("AddEvents");
            }

            TempData["ErrorMessage"] = "Failed to add event. Please check your input.";
            return View(model);
        }

        public ActionResult EditEvent(int id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var eventItem = db.ICMREvents.Find(id);
            if (eventItem == null)
            {
                return HttpNotFound();
            }

            return View(eventItem);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditEvent(ICMREvent model, HttpPostedFileBase ImageFile, HttpPostedFileBase PdfFile)
        {
            if (ModelState.IsValid)
            {
                var eventItem = db.ICMREvents.Find(model.EventId);
                if (eventItem == null)
                {
                    return HttpNotFound();
                }

                eventItem.Title = model.Title;
                eventItem.Type = model.Type;
                eventItem.EventDescription = model.EventDescription;
                eventItem.EventDate = model.EventDate;
                eventItem.StartDate = model.StartDate;
                eventItem.EndDate = model.EndDate;
                eventItem.EventUrl = model.EventUrl;
                eventItem.AltTagDescription = model.AltTagDescription;

                // Update Image
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string imageName = $"{eventItem.EventId}_{Path.GetFileName(ImageFile.FileName)}";
                    string imagePath = Path.Combine(Server.MapPath("~/Uploads/Images/"), imageName);
                    ImageFile.SaveAs(imagePath);
                    eventItem.Image = imageName;
                }

                // Update PDF
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfName = $"{eventItem.EventId}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);
                    PdfFile.SaveAs(pdfPath);
                    eventItem.Pdf = pdfName;
                }

                db.Entry(eventItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction("Events");
            }

            return View(model);
        }

        public ActionResult DeleteEvent(int id)
        {
            var eventItem = db.ICMREvents.Find(id);
            if (eventItem == null)
            {
                return HttpNotFound();
            }

            // Delete image file
            if (!string.IsNullOrEmpty(eventItem.Image))
            {
                string imagePath = Server.MapPath("~/Uploads/Images/") + eventItem.Image;
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Delete PDF file
            if (!string.IsNullOrEmpty(eventItem.Pdf))
            {
                string pdfPath = Server.MapPath("~/Uploads/PDFs/") + eventItem.Pdf;
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            db.ICMREvents.Remove(eventItem);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction("Events");
        }


        //Profiles


        public ActionResult Profiles()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var events = db.ICMRFaculties.ToList();
            return View(events);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }
        public ActionResult AddProfiles()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddProfiles(ICMRFaculty model, HttpPostedFileBase ImageFile, HttpPostedFileBase PdfFile)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddProfiles");
            }
            if (ModelState.IsValid)
            {
                string createdBy = Session["UserRole"] != null ? Session["UserRole"].ToString() : "Unknown";

                ICMRFaculty personalInfo = new ICMRFaculty
                {
                    FacultyName = model.FacultyName,
                    FacultyDesignation = model.FacultyDesignation,
                    FacultyExperience = model.FacultyExperience,
                    FacultyQualification = model.FacultyQualification,
                    UpdatedBy = model.UpdatedBy,
                    UpdatedDate = model.UpdatedDate,
                    CreatedBy = createdBy,
                    CreatedDate = DateTime.Now
                };

                db.ICMRFaculties.Add(personalInfo);
                db.SaveChanges();

                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string imageExt = Path.GetExtension(ImageFile.FileName);
                    string imageName = $"{personalInfo.FacultyId}_{Path.GetFileName(ImageFile.FileName)}";
                    string imagePath = Path.Combine(Server.MapPath("~/Uploads/Images/"), imageName);

                    ImageFile.SaveAs(imagePath);
                    personalInfo.FacultyImage = imageName;
                }

                if (PdfFile != null && PdfFile.ContentLength > 0)
                { 
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{personalInfo.FacultyId}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    PdfFile.SaveAs(pdfPath);
                    personalInfo.FacultyResume = pdfName;
                }

                db.Entry(personalInfo).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Event added successfully!";
                return RedirectToAction("Profiles");
            }

            TempData["ErrorMessage"] = "Failed to add event. Please check your input.";
            return View(model);
        }

        public ActionResult EditPrifile(int id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var eventItem = db.ICMRFaculties.Find(id);
            if (eventItem == null)
            {
                return HttpNotFound();
            }
            return View(eventItem);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditPrifile(ICMRFaculty model, HttpPostedFileBase ImageFile, HttpPostedFileBase PdfFile)
        {
            if (ModelState.IsValid)
            {
                var eventItem = db.ICMRFaculties.Find(model.FacultyId);
                if (eventItem == null)
                {
                    return HttpNotFound();
                }

                eventItem.FacultyName = model.FacultyName;
                eventItem.FacultyDesignation = model.FacultyDesignation;
                eventItem.FacultyExperience = model.FacultyExperience;


                // Update Image
                if (ImageFile != null && ImageFile.ContentLength > 0)
                {
                    string imageName = $"{eventItem.FacultyId}_{Path.GetFileName(ImageFile.FileName)}";
                    string imagePath = Path.Combine(Server.MapPath("~/Uploads/Images/"), imageName);
                    ImageFile.SaveAs(imagePath);
                    eventItem.FacultyImage = imageName;
                }

                // Update PDF
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfName = $"{eventItem.FacultyId}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);
                    PdfFile.SaveAs(pdfPath);
                    eventItem.FacultyResume = pdfName;
                }

                db.Entry(eventItem).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Event updated successfully!";
                return RedirectToAction("Profiles");
            }

            return View(model);
        }

        public ActionResult DeletePrifile(int id)
        {
            var eventItem = db.ICMRFaculties.Find(id);
            if (eventItem == null)
            {
                return HttpNotFound();
            }

            // Delete image file
            if (!string.IsNullOrEmpty(eventItem.FacultyImage))
            {
                string imagePath = Server.MapPath("~/Uploads/Images/") + eventItem.FacultyImage;
                if (System.IO.File.Exists(imagePath))
                {
                    System.IO.File.Delete(imagePath);
                }
            }

            // Delete PDF file
            if (!string.IsNullOrEmpty(eventItem.FacultyResume))
            {
                string pdfPath = Server.MapPath("~/Uploads/PDFs/") + eventItem.FacultyResume;
                if (System.IO.File.Exists(pdfPath))
                {
                    System.IO.File.Delete(pdfPath);
                }
            }

            db.ICMRFaculties.Remove(eventItem);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Event deleted successfully!";
            return RedirectToAction("Events");
        }


        // Full Case Studies
        public ActionResult AddFullCaseStudies()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {

                var subjects = db.AvaCases
                         .Where(c => c.subject != null && c.subject != "")
                         .Select(c => c.subject)
                         .Distinct()
                         .OrderBy(s => s)
                         .ToList();

                ViewBag.Subjects = new SelectList(subjects);
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }


        private string GetLocalIPAddress()
        {
            var host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            return "Local IP Not Found";
        }

        private string GetPublicIPAddress()
        {
            try
            {
                using (WebClient client = new WebClient())
                {
                    return client.DownloadString("https://api.ipify.org");
                }
            }
            catch
            {
                return "Public IP Not Found";
            }
        }


        [HttpPost]
        public ActionResult AddFullCaseStudies(AvaCas Fullcases, HttpPostedFileBase PdfFile, string[] Titles, string[] Texts)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddFullCaseStudies");
            }

            if (ModelState.IsValid)
            {
                var userIdValue = Session["UserId"];
                var Name = Session["Name"];
                var Role = Session["UserRole"];
                string localIp = GetLocalIPAddress();
                string publicIp = GetPublicIPAddress();
                AvaCas full = new AvaCas
                {
                    UploadedBy = Name.ToString(),
                    UploadedRole = Role.ToString(),
                    UploadedId = userIdValue.ToString(),
                    LocalIpAddress = localIp,
                    PublicIpAddress = publicIp,

                    casetitle = Fullcases.casetitle,
                    area = Fullcases.area,
                    company = Fullcases.company,
                    industry = Fullcases.industry,
                    casecode = Fullcases.casecode,
                    caseteach = Fullcases.caseteach,
                    pricers = Fullcases.pricers,
                    pricedollar = Fullcases.pricedollar,
                    subject = Fullcases.subject,
                    pindustry = Fullcases.pindustry,
                    secindustry = Fullcases.secindustry,
                    subindustry = Fullcases.subindustry,
                    region = Fullcases.region,
                    author = Fullcases.author,
                    centername = Fullcases.centername,
                    SecondaryArea1 = Fullcases.SecondaryArea1,
                    SecondaryArea2 = Fullcases.SecondaryArea2,
                    SecondaryArea3 = Fullcases.SecondaryArea3,
                    SecondaryArea4 = Fullcases.SecondaryArea4,
                    SecondaryArea5 = Fullcases.SecondaryArea5,
                    SecondaryArea6 = Fullcases.SecondaryArea6,
                    Market = Fullcases.Market,
                    p_area = Fullcases.p_area,
                    Discipline = Fullcases.Discipline,
                    keywords = Fullcases.keywords,
                    country = Fullcases.country,
                    CaseLength = Fullcases.CaseLength,
                    Period = Fullcases.Period,
                    PubDate = Fullcases.PubDate,
                    Organization = Fullcases.Organization,
                    Description = Fullcases.Description,
                    Themes = Fullcases.Themes,
                    CaseType = "FullCases",
                    @new = "Y",
                    sdate = DateTime.Now
                };

                db.AvaCases.Add(full);
                db.SaveChanges();

                // Handle PDF file
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{full.Id}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    Directory.CreateDirectory(Server.MapPath("~/Uploads/PDFs/"));
                    PdfFile.SaveAs(pdfPath);

                    full.PDF = pdfName;
                    db.Entry(full).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["PDFMessage"] = "PDF uploaded successfully!";
                }

                // Handle Titles[] and Texts[]
                if (Titles != null && Texts != null)
                {
                    if (Titles.Length != Texts.Length)
                    {
                        TempData["ErrorMessage"] = "Mismatch in number of Titles and Texts.";
                        return View(Fullcases);
                    }

                    for (int i = 0; i < Titles.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(Titles[i]) && !string.IsNullOrWhiteSpace(Texts[i]))
                        {
                            db.tblCaseDetails.Add(new tblCaseDetail
                            {
                                CaseId = full.Id,
                                Introduction = Titles[i].Trim(),
                                Content = Texts[i].Trim()
                            });
                        }
                    }

                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Case study added successfully!";
                return RedirectToAction("AddFullCaseStudies");
            }

            TempData["ErrorMessage"] = "There was an error adding the case study. Please check your inputs.";
            return View(Fullcases);


        }


        //[HttpPost]
        //public ActionResult AddFullCaseStudies(AvaCas Fullcases, HttpPostedFileBase PdfFile, string Description)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        //string createdBy = Session["UserRole"] != null ? Session["UserRole"].ToString() : "Unknown";

        //        AvaCas full = new AvaCas
        //        {
        //            casetitle = Fullcases.casetitle,
        //            area = Fullcases.area,
        //            company = Fullcases.company,
        //            industry = Fullcases.industry,
        //            casecode = Fullcases.casecode,
        //            caseteach = Fullcases.caseteach,
        //            pricers = Fullcases.pricers,
        //            pricedollar = Fullcases.pricedollar,
        //            subject = Fullcases.subject,
        //            ordno = Fullcases.ordno,
        //            @new = Fullcases.@new, // `new` is a reserved keyword; consider renaming it in your model.
        //            sdate = Fullcases.sdate,
        //            pindustry = Fullcases.pindustry,
        //            secindustry = Fullcases.secindustry,
        //            subindustry = Fullcases.subindustry,
        //            AVacases_keywords = Fullcases.AVacases_keywords,
        //            region = Fullcases.region,
        //            author = Fullcases.author,
        //            centername = Fullcases.centername,
        //            SecondaryArea1 = Fullcases.SecondaryArea1,
        //            SecondaryArea2 = Fullcases.SecondaryArea2,
        //            SecondaryArea3 = Fullcases.SecondaryArea3,
        //            SecondaryArea4 = Fullcases.SecondaryArea4,
        //            SecondaryArea5 = Fullcases.SecondaryArea5,
        //            SecondaryArea6 = Fullcases.SecondaryArea6,
        //            Market = Fullcases.Market,
        //            Discipline = Fullcases.Discipline,
        //            keywords = Fullcases.keywords,
        //            country = Fullcases.country,
        //            p_area = Fullcases.p_area,
        //            AddToCart = Fullcases.AddToCart,
        //            Description = Fullcases.Description,
        //        };

        //        db.AvaCases.Add(full);
        //        db.SaveChanges();

        //        if (PdfFile != null && PdfFile.ContentLength > 0)
        //        {
        //            string pdfExt = Path.GetExtension(PdfFile.FileName);
        //            string pdfName = $"{full.Id}_{Path.GetFileName(PdfFile.FileName)}";
        //            string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

        //            Directory.CreateDirectory(Server.MapPath("~/Uploads/PDFs/")); 
        //            PdfFile.SaveAs(pdfPath);

        //            full.PDF = pdfName;
        //            db.Entry(full).State = EntityState.Modified;
        //            db.SaveChanges();
        //        }

        //        TempData["SuccessMessage"] = "Case study added successfully!";
        //        return RedirectToAction("AddFullCaseStudies");
        //    }

        //    TempData["ErrorMessage"] = "There was an error adding the case study. Please check your inputs.";
        //    return View(Fullcases); 
        //}


        public ActionResult DisplayFullCaseStudies()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var cases = db.AvaCases.ToList();
            return View(cases);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public ActionResult EditCaseStudy(int id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var fullCase = db.AvaCases.Find(id);
                if (fullCase == null)
                {
                    return HttpNotFound();
                }
                return View(fullCase);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditCaseStudy(AvaCas model, HttpPostedFileBase PdfFile)
        {
            if (ModelState.IsValid)
            {
                var existing = db.AvaCases.Find(model.Id);
                if (existing == null)
                {
                    return HttpNotFound();
                }

                // ✅ Update only selected fields
                existing.casetitle = model.casetitle;
                existing.area = model.area;
                existing.company = model.company;
                existing.industry = model.industry;
                existing.casecode = model.casecode;
                existing.caseteach = model.caseteach;
                existing.pricers = model.pricers;
                existing.country = model.country;
                existing.CaseLength = model.CaseLength;
                existing.Period = model.Period;
                existing.PubDate = model.PubDate;
                existing.Organization = model.Organization;
                existing.Description = model.Description;
                existing.Themes = model.Themes;
                existing.pindustry = model.pindustry;
                existing.secindustry = model.secindustry;
                existing.subindustry = model.subindustry;
                existing.region = model.region;
                existing.author = model.author;
                existing.centername = model.centername;
                existing.SecondaryArea1 = model.SecondaryArea1;
                existing.SecondaryArea2 = model.SecondaryArea2;
                existing.SecondaryArea3 = model.SecondaryArea3;
                existing.SecondaryArea4 = model.SecondaryArea4;
                existing.SecondaryArea5 = model.SecondaryArea5;
                existing.SecondaryArea6 = model.SecondaryArea6;
                existing.Market = model.Market;
                existing.Discipline = model.Discipline;
                existing.p_area = model.p_area;
                existing.keywords = model.keywords;


                // Handle new PDF upload (replace existing if needed)
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{existing.Id}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    Directory.CreateDirectory(Server.MapPath("~/Uploads/PDFs/"));
                    PdfFile.SaveAs(pdfPath);
                    existing.PDF = pdfName;
                }

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Case study updated successfully!";
                return RedirectToAction("DisplayFullCaseStudies"); // or wherever you list the cases
            }

            TempData["ErrorMessage"] = "There was an error updating the case study.";
            return View(model);
        }


        public ActionResult DeleteCaseStudy(int id)
        {
            var caseToDelete = db.AvaCases.Find(id);
            if (caseToDelete == null)
            {
                return HttpNotFound();
            }

            // Optionally delete PDF file from the server
            if (!string.IsNullOrEmpty(caseToDelete.PDF))
            {
                string fullPdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), caseToDelete.PDF);
                if (System.IO.File.Exists(fullPdfPath))
                {
                    System.IO.File.Delete(fullPdfPath);
                }
            }

            db.AvaCases.Remove(caseToDelete);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Case study deleted successfully!";
            return RedirectToAction("DisplayFullCaseStudies");
        }


        //Short Case Studies
        public ActionResult AddShortCaseStudies()
        {


            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var subjects = db.AvaCases
                         .Where(c => c.subject != null && c.subject != "")
                         .Select(c => c.subject)
                         .Distinct()
                         .OrderBy(s => s)
                         .ToList();

                ViewBag.Subjects = new SelectList(subjects);
                return View();
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult AddShortCaseStudies(avashortcas Fullcases, HttpPostedFileBase PdfFile, string[] Titles, string[] Texts)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddShortCaseStudies");
            }

            if (ModelState.IsValid)
            {
                var userIdValue = Session["UserId"];
                var Name = Session["Name"];
                var Role = Session["UserRole"];
                string localIp = GetLocalIPAddress();
                string publicIp = GetPublicIPAddress();

                AvaCas full = new AvaCas
                {
                    UploadedBy = Name.ToString(),
                    UploadedRole = Role.ToString(),
                    UploadedId = userIdValue.ToString(),
                    LocalIpAddress = localIp,
                    PublicIpAddress = publicIp,

                    casetitle = Fullcases.casetitle,
                    area = Fullcases.area,
                    company = Fullcases.company,
                    industry = Fullcases.industry,
                    casecode = Fullcases.casecode,
                    caseteach = Fullcases.caseteach,
                    pricers = Fullcases.pricers,
                    pricedollar = Fullcases.pricedollar,
                    subject = Fullcases.subject,
                    pindustry = Fullcases.pindustry,
                    secindustry = Fullcases.secindustry,
                    subindustry = Fullcases.subindustry,
                    region = Fullcases.region,
                    author = Fullcases.author,
                    centername = Fullcases.centername,
                    SecondaryArea1 = Fullcases.SecondaryArea1,
                    SecondaryArea2 = Fullcases.SecondaryArea2,
                    SecondaryArea3 = Fullcases.SecondaryArea3,
                    SecondaryArea4 = Fullcases.SecondaryArea4,
                    SecondaryArea5 = Fullcases.SecondaryArea5,
                    SecondaryArea6 = Fullcases.SecondaryArea6,
                    Market = Fullcases.Market,
                    p_area = Fullcases.area,
                    Discipline = Fullcases.Discipline,
                    keywords = Fullcases.keywords,
                    country = Fullcases.country,
                    CaseLength = Fullcases.CaseLength,
                    Period = Fullcases.Period,
                    PubDate = Fullcases.PubDate,
                    Organization = Fullcases.Organization,
                    Description = Fullcases.Description,
                    Themes = Fullcases.Themes,
                    CaseType = "ShortCases",
                    @new = "Y",
                    sdate = DateTime.Now
                };

                db.AvaCases.Add(full);
                db.SaveChanges();

                // Handle PDF file
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{full.Id}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    Directory.CreateDirectory(Server.MapPath("~/Uploads/PDFs/"));
                    PdfFile.SaveAs(pdfPath);

                    full.PDF = pdfName;
                    db.Entry(full).State = EntityState.Modified;
                    db.SaveChanges();

                    TempData["PDFMessage"] = "PDF uploaded successfully!";
                }

                // Handle Titles[] and Texts[]
                if (Titles != null && Texts != null)
                {
                    if (Titles.Length != Texts.Length)
                    {
                        TempData["ErrorMessage"] = "Mismatch in number of Titles and Texts.";
                        return View(Fullcases);
                    }

                    for (int i = 0; i < Titles.Length; i++)
                    {
                        if (!string.IsNullOrWhiteSpace(Titles[i]) && !string.IsNullOrWhiteSpace(Texts[i]))
                        {
                            db.tblCaseDetails.Add(new tblCaseDetail
                            {
                                CaseId = full.Id,
                                Introduction = Titles[i].Trim(),
                                Content = Texts[i].Trim()
                            });
                        }
                    }

                    db.SaveChanges();
                }

                TempData["SuccessMessage"] = "Case study added successfully!";
                return RedirectToAction("AddShortCaseStudies");
            }

            TempData["ErrorMessage"] = "There was an error adding the short case study. Please check your inputs.";
            return View(Fullcases);
        }


        public ActionResult DisplayShortCases()
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var cases = db.avashortcases.ToList();
            return View(cases);
            }
            else
            {
                // Not authorized, redirect to AdminLogin
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        public ActionResult EditShortCase(int id)
        {
            if (Session["UserRole"] != null && Session["UserRole"].ToString() == "Admin")
            {
                var fullCase = db.AvaCases.Find(id);
                if (fullCase == null)
                {
                    return HttpNotFound();
                }
                return View(fullCase);
            }
            else
            {
                return RedirectToAction("AdminLogin", "Admin");
            }
        }

        [HttpPost]
        public ActionResult EditShortCase(AvaCas model, HttpPostedFileBase PdfFile)
        {
            if (ModelState.IsValid)
            {
                var existing = db.AvaCases.Find(model.Id);
                if (existing == null)
                {
                    return HttpNotFound();
                }

                // ✅ Update only selected fields
                existing.casetitle = model.casetitle;
                existing.area = model.area;
                existing.company = model.company;
                existing.industry = model.industry;
                existing.casecode = model.casecode;
                existing.caseteach = model.caseteach;
                existing.pricers = model.pricers;
                existing.country = model.country;
                existing.CaseLength = model.CaseLength;
                existing.Period = model.Period;
                existing.PubDate = model.PubDate;
                existing.Organization = model.Organization;
                existing.Description = model.Description;
                existing.Themes = model.Themes;
                existing.pindustry = model.pindustry;
                existing.secindustry = model.secindustry;
                existing.subindustry = model.subindustry;
                existing.region = model.region;
                existing.author = model.author;
                existing.centername = model.centername;
                existing.SecondaryArea1 = model.SecondaryArea1;
                existing.SecondaryArea2 = model.SecondaryArea2;
                existing.SecondaryArea3 = model.SecondaryArea3;
                existing.SecondaryArea4 = model.SecondaryArea4;
                existing.SecondaryArea5 = model.SecondaryArea5;
                existing.SecondaryArea6 = model.SecondaryArea6;
                existing.Market = model.Market;
                existing.Discipline = model.Discipline;
                existing.keywords = model.keywords;

                // Handle new PDF upload (replace existing if needed)
                if (PdfFile != null && PdfFile.ContentLength > 0)
                {
                    string pdfExt = Path.GetExtension(PdfFile.FileName);
                    string pdfName = $"{existing.Id}_{Path.GetFileName(PdfFile.FileName)}";
                    string pdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), pdfName);

                    Directory.CreateDirectory(Server.MapPath("~/Uploads/PDFs/"));
                    PdfFile.SaveAs(pdfPath);
                    existing.PDF = pdfName;
                }

                db.Entry(existing).State = EntityState.Modified;
                db.SaveChanges();

                TempData["SuccessMessage"] = "Case study updated successfully!";
                return RedirectToAction("DisplayShortCases"); // or wherever you list the cases
            }

            TempData["ErrorMessage"] = "There was an error updating the case study.";
            return View(model);
        }


        public ActionResult DeleteShortCase(int id)
        {
            var caseToDelete = db.avashortcases.Find(id);
            if (caseToDelete == null)
            {
                return HttpNotFound();
            }

            // Optionally delete PDF file from the server
            if (!string.IsNullOrEmpty(caseToDelete.PDF))
            {
                string fullPdfPath = Path.Combine(Server.MapPath("~/Uploads/PDFs/"), caseToDelete.PDF);
                if (System.IO.File.Exists(fullPdfPath))
                {
                    System.IO.File.Delete(fullPdfPath);
                }
            }

            db.avashortcases.Remove(caseToDelete);
            db.SaveChanges();

            TempData["SuccessMessage"] = "Case study deleted successfully!";
            return RedirectToAction("DisplayShortCases");
        }


        [HttpGet]
        public ActionResult DisplayItemDetails()
        {
            var data = (from m in db.Master_icmr
                        join p in db.proccases on m.userid equals p.userid
                        select new CartItem
                        {
                            userid = m.userid,
                            first_name = m.first_name,
                            email = m.email,
                            mobile_phone=m.mobile_phone,
                            casetitle = p.casetitle,
                            sdate = m.sdate,
                            TransactionDate = p.TransactionDate,
                            amount = p.amount
                        }).ToList();

            return View(data);
        }

        [HttpGet]
        public ActionResult DisplayItems(int? month)
        {
            var data = (from m in db.Master_icmr
                        join p in db.proccases on m.userid equals p.userid
                        select new CartItem
                        {
                            userid = m.userid,
                            first_name = m.first_name,
                            email = m.email,
                            casetitle = p.casetitle,
                            sdate = m.sdate,
                            TransactionDate = p.TransactionDate,
                            amount = p.amount
                        }).ToList();

            if (month.HasValue)
            {
                data = data
                    .Where(x => x.sdate.HasValue && x.sdate.Value.Month == month.Value)
                    .ToList();
            }

            return View(data); // View expects List<CartItem>
        }
        public ActionResult AddTextBooks()
        {
            return View();
        }
        [HttpPost]
        public ActionResult SubmitTextBooks(HttpPostedFileBase TextBookPdf, HttpPostedFileBase[] CoverImages)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddTextBooks");
            }
            // Create new textbook record
            var textbook = new tblTextBook
            {
                Title = Request["Title"],
                Volume = Request["Volume"],
                Category = Request["Category"],
                LocalIpAddress = SecureHelp.GetLocalIPAddress(),
                IpAddress = SecureHelp.GetIPAddress(),
                TextBookPages = Convert.ToInt32(Request["TextBookPages"]),
                WorkBookPages = Convert.ToInt32(Request["WorkBookPages"]),
                TextBookprise = Convert.ToDecimal(Request["TextBookprise"]),
                WorkBookprise = Convert.ToDecimal(Request["WorkBookprise"]),
                Year = Convert.ToInt32(Request["Year"]),
                Month = Convert.ToInt32(Request["Month"]),
                Overview = Request["Overview"],
                CreatedOn = DateTime.Now,
                UserId = Session["UserId"]?.ToString()
            };

            // Add chapters
            var chapterNos = Request.Form.GetValues("ChapterNo");
            var chapterTitles = Request.Form.GetValues("ChapterTitle");
            var chapterPages = Request.Form.GetValues("Pages");
            var chapterKeywords = Request.Form.GetValues("Keywords");

            if (chapterNos != null)
            {
                for (int i = 0; i < chapterNos.Length; i++)
                {
                    textbook.tblChapters.Add(new tblChapter
                    {
                        ChapterNo = Convert.ToInt32(chapterNos[i]),
                        ChapterTitle = chapterTitles?[i],
                        Pages = Convert.ToInt32(chapterPages[i]),
                        Keywords = chapterKeywords?[i]
                    });
                }
            }

            // Save textbook to get TextBookId
            db.tblTextBooks.Add(textbook);
            db.SaveChanges(); // TextBookId is now available

            // Save PDF (if uploaded)
            if (TextBookPdf != null && TextBookPdf.ContentLength > 0)
            {
                string originalPdfName = Path.GetFileName(TextBookPdf.FileName);
                string renamedPdfName = $"{textbook.TextBookId}_{originalPdfName}";
                string pdfFolderPath = Server.MapPath("~/Uploads/PDFs/");
                if (!Directory.Exists(pdfFolderPath)) Directory.CreateDirectory(pdfFolderPath);
                string pdfPath = Path.Combine(pdfFolderPath, renamedPdfName);
                TextBookPdf.SaveAs(pdfPath);

                textbook.TextBookPdf = renamedPdfName;
                db.Entry(textbook).State = System.Data.Entity.EntityState.Modified;
                db.SaveChanges();
            }

            // Save Cover Images
            if (CoverImages != null && CoverImages.Any())
            {
                foreach (var img in CoverImages)
                {
                    if (img != null && img.ContentLength > 0)
                    {
                        string originalImgName = Path.GetFileName(img.FileName);
                        string renamedImgName = $"{textbook.TextBookId}_{originalImgName}";
                        string imgFolderPath = Server.MapPath("~/Uploads/Images/");
                        if (!Directory.Exists(imgFolderPath)) Directory.CreateDirectory(imgFolderPath);
                        string imgPath = Path.Combine(imgFolderPath, renamedImgName);
                        img.SaveAs(imgPath);

                        db.tblTextBookImgs.Add(new tblTextBookImg
                        {
                            TextBookId = textbook.TextBookId,
                            CoverImage = renamedImgName
                        });
                    }
                }

                db.SaveChanges();
            }

            TempData["Success"] = "TextBook, chapters, PDF, and cover images submitted successfully!";
            return RedirectToAction("AddTextBooks");
        }


        public ActionResult AddEbookCases()
        {
            var subjects = db.AvaCases
               .Select(a => a.subject)
               .Distinct()
               .ToList();

            ViewBag.Subjects = new SelectList(subjects);
            return View();
        }
        public JsonResult GetCaseTitlesBySubject(string subject, int year, int month)
        {
            var titles = db.AvaCases
                .Where(c => c.subject == subject
                            && c.sdate.HasValue
                            && c.sdate.Value.Year == year
                            && c.sdate.Value.Month == month)
                .Select(c => c.casetitle)
                .Distinct()
                .ToList();

            return Json(titles, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetYearsBySubject(string subject)
        {
            var years = db.AvaCases
                .Where(c => c.sdate.HasValue && c.subject == subject)
                .Select(c => c.sdate.Value.Year)
                .Distinct()
                .OrderByDescending(y => y)
                .ToList();

            return Json(years, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetMonthsByYearAndSubject(int year, string subject)
        {
            var months = db.AvaCases
                           .Where(c => c.sdate.HasValue && c.sdate.Value.Year == year && c.subject == subject)
                           .Select(c => c.sdate.Value.Month)
                           .Distinct()
                           .OrderBy(m => m)
                           .ToList();

            var monthNames = months.Select(m => new {
                Value = m,
                Text = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(m)
            });

            return Json(monthNames, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult AddEbookCases(FormCollection form, HttpPostedFileBase coverImage)
        {

            if (!this.IsCaptchaValid("Captcha is not valid"))
            {
                TempData["ErrorMsg"] = "Captcha validation failed. Please try again.";
                return RedirectToAction("AddEbookCases");
            }

            string email = Session["UserEmail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "User session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            var user = db.ICMRLogins.FirstOrDefault(u => u.EmailId == email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            string userEmpId = user.EmpId;

            // Handle cover image
            string fileName = "";
            if (coverImage != null && coverImage.ContentLength > 0)
            {
                fileName = Path.GetFileName(coverImage.FileName);
                string path = Path.Combine(Server.MapPath("~/UploadedImages"), fileName);
                coverImage.SaveAs(path);
            }

            // Create ebook
            var ebook = new tblEbook
            {
                title = form["title"],
                volume = form["volume"],
                category = form["category"],
                userid = userEmpId,
                createdOn = DateTime.Now,
                ipadress = SecureHelp.GetIPAddress(),
                LocalIpAddress = SecureHelp.GetLocalIPAddress(),
                pages = Convert.ToInt32(form["pages"]),
                prise = Convert.ToDecimal(form["ebookPrice"]),
                year = Convert.ToInt32(form["year"]),
                month = Convert.ToInt32(form["month"]),
                coverImg = fileName,
                ebookPdf = form["formate"]
            };

            db.tblEbooks.Add(ebook);
            db.SaveChanges(); // ✅ save now so ebookId is generated

            var categories = form.GetValues("subjectCategory");
            var titles = form.GetValues("casetitle");
            var months = form.GetValues("caseMonth");
            var years = form.GetValues("caseYear");

            List<string> duplicateTitles = new List<string>();

            if (categories != null && titles != null)
            {
                for (int i = 0; i < categories.Length; i++)
                {
                    string currentTitle = titles[i].Trim();
                    string subjectCategory = categories[i];
                    int currentYear = Convert.ToInt32(years[i]);
                    int currentMonthNumber = Convert.ToInt32(months[i]);
                    string currentMonthName = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(currentMonthNumber);

                    // ✅ Check if same Title + Subject + Year + Month exists
                    bool caseExists = db.tblEbookCases.Any(c =>
                        c.caseTitle.ToLower() == currentTitle.ToLower() &&
                        c.subjectCategory == subjectCategory &&
                        c.Year == currentYear &&
                        c.Month == currentMonthName
                    );

                    if (!caseExists)
                    {
                        // ✅ Get casecode from AvaCases
                        string casecode = db.AvaCases
                            .Where(a => a.casetitle.ToLower() == currentTitle.ToLower()
                                     && a.subject == subjectCategory
                                     && a.sdate.HasValue
                                     && a.sdate.Value.Year == currentYear
                                     && a.sdate.Value.Month == currentMonthNumber)
                            .Select(a => a.casecode)
                            .FirstOrDefault();

                        tblEbookCas c = new tblEbookCas
                        {
                            ebookId = ebook.ebookId,
                            caseTitle = currentTitle,
                            subjectCategory = subjectCategory,
                            category = ebook.category,
                            userid = userEmpId,
                            Month = currentMonthName,
                            Year = currentYear,
                            createdOn = DateTime.Now,
                            casecode = casecode
                        };

                        db.tblEbookCases.Add(c);
                    }
                    else
                    {
                        duplicateTitles.Add($"{currentTitle} ({subjectCategory} - {currentMonthName} {currentYear})");
                    }
                }
            }

            db.SaveChanges();

            if (duplicateTitles.Any())
            {
                TempData["DuplicateCaseTitles"] = "Already exists: " + string.Join(", ", duplicateTitles.Distinct());
            }
            else
            {
                TempData["AddEbookCasesMessage"] = "Inserted successfully!";
            }
            return RedirectToAction("AddEbookCases");
        }

        public ActionResult DisplayEBookDetails()
        {
            var details = db.tblEbooks.OrderByDescending(e => e.createdOn).ToList();
            return View(details);
        }

      


        public ActionResult AddCaseVolumes()
        {
            var subjects = db.AvaCases
               .Select(a => a.subject)
               .Distinct()
               .ToList();

            ViewBag.Subjects = new SelectList(subjects);
            return View();
        }


        [HttpPost]
        public ActionResult AddCaseVolumes(FormCollection form, HttpPostedFileBase coverImage)
        {
            string email = Session["UserEmail"] as string;

            if (string.IsNullOrEmpty(email))
            {
                TempData["Error"] = "User session expired. Please login again.";
                return RedirectToAction("Login", "Account");
            }

            var user = db.ICMRLogins.FirstOrDefault(u => u.EmailId == email);
            if (user == null)
            {
                TempData["Error"] = "User not found.";
                return RedirectToAction("Login", "Account");
            }

            string userEmpId = user.EmpId;

            string fileName = "";
            if (coverImage != null && coverImage.ContentLength > 0)
            {
                fileName = Path.GetFileName(coverImage.FileName);
                string path = Path.Combine(Server.MapPath("~/UploadedImages"), fileName);
                coverImage.SaveAs(path);
            }
            var ebook = new tblCaseStudyVolume
            {
                Title = form["Title"],
                Volume = form["Volume"],
                Category = form["Category"],
                UserId = userEmpId,
                CreatedOn = DateTime.Now,
                IpAdress = SecureHelp.GetIPAddress(),
                LocalIpAddress = SecureHelp.GetLocalIPAddress(),
                Pages = Convert.ToInt32(form["pages"]),
                Price = Convert.ToDecimal(form["ebookPrice"]),
                Year = Convert.ToInt32(form["year"]),
                Month = Convert.ToInt32(form["month"]),
                CoverImg = fileName,
                CasePdf = form["formate"]
            };

            db.tblCaseStudyVolumes.Add(ebook);

            int ebookId = ebook.CaseTudyVolumeId;

            var categories = form.GetValues("subjectCategory");
            var titles = form.GetValues("casetitle");
            var months = form.GetValues("caseMonth");
            var years = form.GetValues("caseYear");

            if (categories != null && titles != null)
            {
                for (int i = 0; i < categories.Length; i++)
                {
                    tblCaseStudy c = new tblCaseStudy
                    {
                        CaseTudyVolumeId = ebookId,
                        CaseTitle = titles[i],
                        SubjectCategory = categories[i],
                        Category = ebook.Category,
                        UserId = userEmpId,
                        Month = System.Globalization.CultureInfo.CurrentCulture.DateTimeFormat.GetMonthName(Convert.ToInt32(months[i])),
                        Year = Convert.ToInt32(years[i]),
                        CreatedOn = DateTime.Now,
                    };
                    db.tblCaseStudies.Add(c);
                }
            }

            db.SaveChanges();
            TempData["AddEbookCasesMessage"] = "Inserted successfully!";
            return RedirectToAction("AddEbookCases");
        }


    }
}