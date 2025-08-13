using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using System.Web.Mvc;
using ICMR.Models;
using Newtonsoft.Json;
using PagedList;
using PagedList.Mvc;

namespace ICMR.Controllers
{
    public class HomeController : Controller
    {
        private ICMREntities2 db = new ICMREntities2();

        //public ActionResult Index()
        //{
        //    return View();
        //}

        public ActionResult CaseStudiesForPagination(string searchString, int? page)
        {
            int pageSize = 25;
            int pageNumber = (page ?? 1);

            var caseStudies = db.AvaCases.AsQueryable();

            if (!String.IsNullOrEmpty(searchString))
            {
                caseStudies = caseStudies.Where(c => c.casetitle.Contains(searchString));
            }

            caseStudies = caseStudies.OrderBy(c => c.Id);

            ViewBag.CurrentFilter = searchString;

            IPagedList<AvaCas> pagedList = caseStudies.ToPagedList(pageNumber, pageSize);

            return View(pagedList);
        }
        public ActionResult Search()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SearchCase(string searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm))
            {
                return Json(new { success = false, message = "Empty search term" });
            }

            // Search AvaCases for "full" case type
            var fullCases = db.AvaCases
                .Where(c => c.casetitle.Contains(searchTerm) ||
                            c.subject.Contains(searchTerm) ||
                            c.casecode.Contains(searchTerm) ||
                            c.keywords.Contains(searchTerm))
                .Select(c => new
                {
                    c.Id,
                    c.casetitle,
                    c.subject,
                    c.keywords,
                    c.casecode,
                    c.pricers,
                    Type = "full"
                })
                .ToList();

            // Search avashortcases for "short" case type
            var shortCases = db.avashortcases
                .Where(c => c.casetitle.Contains(searchTerm) ||
                            c.subject.Contains(searchTerm) ||
                            c.casecode.Contains(searchTerm) ||
                            c.keywords.Contains(searchTerm))
                .Select(c => new
                {
                    c.Id,
                    c.casetitle,
                    c.subject,
                    c.keywords,
                    c.casecode,
                    c.pricers,
                    Type = "short"
                })
                .ToList();

            // Combine both results
            var matchedCases = fullCases.Concat(shortCases).ToList();

            return Json(new { success = true, data = matchedCases });
        }

        public ActionResult NewCases()
        {
            var latestCases = db.AvaCases
                       .OrderByDescending(c => c.sdate)
                       .Take(5)
                       .ToList();
            return View(latestCases);
        }

        public ActionResult InterviewDetail()
        {
            return View();
        }
        public ActionResult Interview()
        {
            var facultyList = db.ICMRFaculties
                   .OrderByDescending(f => f.CreatedDate)
                   .ToList();


            return View(facultyList);
        }
        public ActionResult JustInCase()
        {
            return View();
        }
        public ActionResult ICMRServices()
        {
            return View();
        }
        public ActionResult EventDetailPage()
        {
            return View();
        }
        public ActionResult EventPage()
        {
            return View();
        }
        public ActionResult MediaDetailPage()
        {
            return View();
        }
        public ActionResult Media()
        {
            return View();
        }
        public ActionResult AlumniSpeak()
        {
            return View();
        }
        public ActionResult Subscribe()
        {
            return View();
        }
        public ActionResult Index1()
        {
            return View();
        }
        public ActionResult MdpFdp()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult CaseMappingToCurriculum()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult CaseSubscription()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Telecom()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Retailing()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult MediaEntertainment()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ElectricalsElectronics()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult BankingFinancialServices()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Aviation()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult IcmrCaseStudies()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult AwardsRecognitions()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Publications()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ProfileSanjibDutta()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult AboutIbsCaseResearchCenter()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }




        public ActionResult CaseStudyDetails(string casecode)
        {
            // Step 1: Find the case study by casecode
            var caseStudy = db.AvaCases.FirstOrDefault(c => c.casecode == casecode);
            if (caseStudy == null)
            {
                return HttpNotFound();
            }

            // Step 2: Get details from tblCaseDetails using Id from AvaCases
            var contents = db.tblCaseDetails
                             .Where(c => c.CaseId == caseStudy.Id) // match by Id
                             .ToList();

            ViewBag.Contents = contents;

            return View(caseStudy);
        }


        //public ActionResult CaseStudyDetails(int? id)
        //{

        //    //int decodedId = int.Parse(Encoding.UTF8.GetString(Convert.FromBase64String(id)));
        //    var caseStudy = db.AvaCases.FirstOrDefault(c => c.Id == id);

        //    if (caseStudy == null)
        //    {
        //        return HttpNotFound();
        //    }
        //    var contents = db.tblCaseDetails
        //             .Where(c => c.CaseId == id)
        //             .ToList();

        //    ViewBag.Contents = contents;
        //    return View(caseStudy);
        //}


        [ValidateInput(false)]
        public ActionResult CaseStudies(string category = "Business Environment")
        {
            ViewBag.CartCount = db.AvaCases.Count(c => (bool)!c.AddToCart);


            var cases = db.AvaCases
          .Where(c => c.subject == category &&
                     (c.CaseType == "FullCases" || c.CaseType == "ShortCases"))
          .OrderByDescending(c => c.sdate ?? DateTime.MinValue)
          .ToList();


            //var cases = db.AvaCases
            //      .Where(c => c.subject == category)
            //      .OrderByDescending(c => c.casecode) // Sort by casecode descending
            //      .ToList();
            //var cases = db.AvaCases.Where(c => c.subject == category).ToList();

            if (Request.IsAjaxRequest())
            {
                return View("Cart", cases); // Return only the updated section
            }

            ViewBag.SelectedCategory = category;
            return View(cases);
        }

        public ActionResult ShortCases(string category)
        {
            ViewBag.CartCount = db.avashortcases.Count(c => (bool)!c.AddToCart);

            var cases = string.IsNullOrEmpty(category)
                ? db.avashortcases.ToList()
                : db.avashortcases.Where(c => c.subject == category).GroupBy(c => c.casecode)
                .Select(g => g.OrderByDescending(x => x.Id).FirstOrDefault())
                .OrderByDescending(c => c.casecode)
                .ToList();

            if (Request.IsAjaxRequest())
            {
                return View("Cart", cases);
            }

            ViewBag.SelectedCategory = category ?? "All Categories";
            return View(cases);
        }

        public ActionResult Index()
        {
            var caseStudies = db.AvaCases.ToList();
            return View(caseStudies);
        }


        [HttpPost]
        public ActionResult AddToCart(int articleId, string type)
        {
            string sessionKey = "CartItems";

            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartViewModel>()
                : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            object articleToAdd = null;

            if (type == "full")
            {
                articleToAdd = db.AvaCases.FirstOrDefault(a => a.Id == articleId);
            }
            else if (type == "short")
            {
                articleToAdd = db.avashortcases.FirstOrDefault(a => a.Id == articleId);
            }

            if (articleToAdd == null)
            {
                return Json(new { success = false, message = "Article not found" });
            }


            if (cartItems.Any(c => c.Id == articleId && c.Type == type))
            {
                return Json(new { success = false, message = "Item already in cart", cartCount = cartItems.Count });
            }


            cartItems.Add(new CartViewModel
            {
                Id = articleId,
                Type = type

            });


            Session[sessionKey] = JsonConvert.SerializeObject(cartItems);
            Session["CartItemCount"] = cartItems.Count;

            return Json(new
            {
                success = true,
                message = "Item added to cart successfully",
                cartCount = cartItems.Count
            });
        }


        //public ActionResult AddToCart(int id)
        //{
        //    var caseStudy = db.AvaCases.FirstOrDefault(c => c.Id == id);
        //    if (caseStudy == null)
        //    {
        //        return HttpNotFound();
        //    }

        //    caseStudy.AddToCart = true;
        //    db.SaveChanges();

        //    Session["CartCount"] = db.AvaCases.Count(c => c.AddToCart == true);

        //    return RedirectToAction("CaseStudies");
        //}

        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            Session["CartCount"] = db.AvaCases.Count(c => c.AddToCart == true);

            ViewBag.CartCount = Session["CartCount"] ?? 0;

            base.OnActionExecuting(filterContext);
        }

        public ActionResult Cart()
        {
            string sessionKey = "CartItems";


            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartViewModel>()
                : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            // Get the full objects from the database based on cart items
            var model = new CartViewModel
            {
                AvaCases = cartItems.Where(c => c.Type == "full")
                                    .Select(c => db.AvaCases.FirstOrDefault(a => a.Id == c.Id))
                                    .Where(a => a != null)
                                    .ToList(),

                AvaShortCases = cartItems.Where(c => c.Type == "short")
                                         .Select(c => db.avashortcases.FirstOrDefault(a => a.Id == c.Id))
                                         .Where(a => a != null)
                                         .ToList()
            };

            return View(model);
        }


        public ActionResult RemoveFromCart(int id, string type)
        {
            // Get the cart items from session
            string sessionKey = "CartItems";
            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                            ? new List<CartViewModel>()
                            : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            // Find and remove the item from the session list
            var itemToRemove = cartItems.FirstOrDefault(c => c.Id == id && c.Type == type);
            if (itemToRemove != null)
            {
                cartItems.Remove(itemToRemove);
            }

            // Update the session with the modified list
            Session[sessionKey] = JsonConvert.SerializeObject(cartItems);
            Session["CartItemCount"] = cartItems.Count;

            // If item type is "ava", remove from database
            if (type == "ava")
            {
                var item = db.AvaCases.FirstOrDefault(x => x.Id == id);
                if (item != null) item.AddToCart = false;
            }
            // If item type is "short", remove from database
            else if (type == "short")
            {
                var item = db.avashortcases.FirstOrDefault(x => x.Id == id);
                if (item != null) item.AddToCart = false;
            }

            // Save the changes to the database
            db.SaveChanges();

            // Redirect back to the Cart view
            return RedirectToAction("Cart");
        }


        //public ActionResult RemoveFromCart(int id)
        //{
        //    var caseStudy = db.AvaCases.FirstOrDefault(c => c.Id == id);
        //    if (caseStudy != null)
        //    {
        //        caseStudy.AddToCart = false;
        //        db.SaveChanges();
        //    }

        //    return RedirectToAction("Cart");
        //}

        public ActionResult ClearCart()
        {
            Session["CartItems"] = null;
            Session["CartItemCount"] = 0;
            return RedirectToAction("Cart");
        }

        public ActionResult ProceedToPayAll()
        {


            // Get the cart items from the session (this assumes your session stores cart items as a serialized string)
            string sessionKey = "CartItems";
            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                            ? new List<CartViewModel>()
                            : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            // If no items were added to the cart, show a message and redirect to the Cart page
            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty!";
                return RedirectToAction("Cart");
            }

            // Get the full objects for the cart items from the database based on the session data
            var fullItems = cartItems.Where(c => c.Type == "full")
                                     .Select(c => db.AvaCases.FirstOrDefault(a => a.Id == c.Id))
                                     .Where(a => a != null)
                                     .ToList();

            var shortItems = cartItems.Where(c => c.Type == "short")
                                      .Select(c => db.avashortcases.FirstOrDefault(a => a.Id == c.Id))
                                      .Where(a => a != null)
                                      .ToList();

            // Calculate the total price
            decimal totalPrice = (decimal)(fullItems.Sum(item => item.pricers ?? 0)
                                           + shortItems.Sum(item => item.pricers ?? 0));

            // Create the payment model
            var paymentModel = new IcmrModel
            {
                TotalAmount = totalPrice,
                //Items = fullItems,
                //ShortItems = shortItems
            };

            // Set session values for payment processing
            string category = "ICMR-2025";
            string prg = "ICMR 2025";

            Session["prg"] = prg;
            Session["amt"] = paymentModel.TotalAmount.ToString();
            Session["category"] = category;


            // Redirect to the PaymentPage
            return RedirectToAction("PaymentPage");
        }

        //public ActionResult ProceedToPay(int id)
        //{
        //    var item = db.AvaCases.FirstOrDefault(c => c.Id == id && c.AddToCart == true);
        //    if (item == null)
        //    {
        //        TempData["Message"] = "Item not found!";
        //        return RedirectToAction("Cart");
        //    }

        //    var paymentModel = new AvaCas
        //    {
        //        TotalAmount = (decimal)(item.pricers ?? 0),
        //        Items = new List<AvaCas> { item }
        //    };

        //    string category = "ICMR-2025";
        //    string prg = "ICMR 2025";

        //    Session["prg"] = prg;
        //    Session["amt"] = paymentModel.TotalAmount.ToString();
        //    Session["category"] = category;
        //    Session["uid"] = id.ToString();


        //    return View("PaymentPage", paymentModel);
        //}


        public JsonResult GetCartCount()
        {
            string sessionKey = "CartItems";

            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartViewModel>()
                : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            return Json(cartItems.Count, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ConfirmPayment()
        {
            string prg = checkstring(Session["prg"]?.ToString() ?? string.Empty);
            string amtStr = checkstring(Session["amt"]?.ToString() ?? string.Empty);
            string category = checkstring(Session["category"]?.ToString() ?? string.Empty);
            string userId = checkstring(Session["uid"]?.ToString() ?? string.Empty);

            if (string.IsNullOrEmpty(userId))
            {
                TempData["Message"] = "Invalid transaction!";
                return RedirectToAction("Cart");
            }

            if (!decimal.TryParse(amtStr, out decimal totalAmount))
            {
                TempData["Message"] = "Invalid amount!";
                return RedirectToAction("Cart");
            }

            // Get items from session
            var cartItemsJson = Session["CartItems"] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartViewModel>()
                : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            var fullItems = cartItems.Where(c => c.Type == "full")
                                     .Select(c => db.AvaCases.FirstOrDefault(a => a.Id == c.Id))
                                     .Where(a => a != null)
                                     .ToList();

            var shortItems = cartItems.Where(c => c.Type == "short")
                                      .Select(c => db.avashortcases.FirstOrDefault(a => a.Id == c.Id))
                                      .Where(a => a != null)
                                      .ToList();

            if (!fullItems.Any() && !shortItems.Any())
            {
                TempData["Message"] = "No items found for payment!";
                return RedirectToAction("Cart");
            }

            // Set view data for the confirmation page
            ViewBag.prg = prg;
            ViewBag.amt = totalAmount;
            ViewBag.cat = category;
            ViewBag.uid = userId;
            ViewBag.FullItems = fullItems;
            ViewBag.ShortItems = shortItems;

            Session["CartItems"] = null;


            return View();
        }


        public string checkstring(string str)
        {
            if (string.IsNullOrEmpty(str))
            {
                return string.Empty; // Return an empty string if input is null or empty
            }

            // Remove potentially harmful strings
            string[] strkeywords = new string[]
            {
        "'", "--", "*/", "/", " * ", "xp_", "; --", "; ", "/*", "*/", "@@",
        "char(", "nchar(", "varchar(", "nvarchar(", "alter", "begin", "cast",
        "create", "cursor", "declare", "delete", "drop", "end", "exec",
        "execute", "fetch", "insert", "kill", "select", "sys", "sysobjects",
        "syscolumns", "table", "sleep", "windows", "script", "delay", " or ",
        "()", " + ", " < ", " > ", "PG_SLEEP", "wait", "concat", "socket",
        "echo", "gethostbyname", " || ", "()", "nslookup", "chr(", "hex(",
        "response.Write", "=", "waitfor", "update", "truncate", "union", "unionall"
            };

            foreach (string spcl in strkeywords)
            {
                str = str.Replace(spcl, "");
            }

            return str;
        }

        //public ActionResult CheckUserAndRedirect()
        //{
        //    if (Session["uid"] != null)
        //    {
        //        // User is logged in
        //        return RedirectToAction("ConfirmPayment", "Home");
        //    }
        //    else
        //    {
        //        // User is not logged in
        //        return RedirectToAction("UserLogin", "Home");
        //    }
        //}


        public ActionResult PaymentPage()
        {

            string userId = checkstring(Session["uid"]?.ToString() ?? string.Empty);


            string sessionKey = "CartItems";
            var cartItemsJson = Session[sessionKey] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                            ? new List<CartViewModel>()
                            : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);


            if (!cartItems.Any())
            {
                TempData["Message"] = "Your cart is empty!";
                return RedirectToAction("Cart");
            }


            var fullItems = cartItems.Where(c => c.Type == "full")
                                     .Select(c => db.AvaCases.FirstOrDefault(a => a.Id == c.Id))
                                     .Where(a => a != null)
                                     .ToList();

            var shortItems = cartItems.Where(c => c.Type == "short")
                                      .Select(c => db.avashortcases.FirstOrDefault(a => a.Id == c.Id))
                                      .Where(a => a != null)
                                      .ToList();


            decimal totalPrice = (decimal)(fullItems.Sum(item => item.pricers ?? 0)
                                           + shortItems.Sum(item => item.pricers ?? 0));


            var paymentModel = new IcmrModel
            {
                TotalAmount = totalPrice,
                //Items = fullItems,
                //ShortItems = shortItems
            };


            return View(paymentModel);
        }


        //public ActionResult PaymentPage()
        //{
        //    var cartItems = db.AvaCases
        //                      .Where(c => c.AddToCart == true)
        //                      .ToList();
        //    decimal totalAmount = cartItems.Sum(item => (decimal)(item.pricers ?? 0));
        //    var model = new IcmrModel
        //    {
        //        Items = cartItems,
        //        TotalAmount = totalAmount
        //    };
        //    return View(model);
        //}


        [HttpPost]
        public ActionResult SubmitBillingDetails(Master_icmr model)
        {
            string timestamp = DateTime.Now.ToString("yyyyMMddHHmmss");
            string randomUserId = model.first_name.ToLower() + "_" + timestamp;

            string generatedPassword = Guid.NewGuid().ToString("N").Substring(0, 8);

            Master_icmr personalInfo = new Master_icmr
            {
                userid = randomUserId,
                prefix = model.prefix,
                first_name = model.first_name,
                last_name = model.last_name,
                add1 = model.add1,
                state = model.state,
                country = model.country,
                city = model.city,
                PIN = model.PIN,
                mobile_phone = model.mobile_phone,
                fax = model.fax,
                email = model.email,
                sdate = model.sdate ?? DateTime.Now,
                dcname = model.dcname,
                dophone = model.dophone,
                EmailAlert = model.EmailAlert,
                Exdate = model.Exdate,
                OrgGST = model.OrgGST,
                Password = generatedPassword
            };

            db.Master_icmr.Add(personalInfo);

            // Get cart items from session
            var cartItemsJson = Session["CartItems"] as string;
            var cartItems = string.IsNullOrEmpty(cartItemsJson)
                ? new List<CartViewModel>()
                : JsonConvert.DeserializeObject<List<CartViewModel>>(cartItemsJson);

            // Separate full and short case studies
            var fullCases = cartItems.Where(c => c.Type == "full")
                                     .Select(c => db.AvaCases.FirstOrDefault(a => a.Id == c.Id))
                                     .Where(a => a != null)
                                     .ToList();

            var shortCases = cartItems.Where(c => c.Type == "short")
                                      .Select(c => db.avashortcases.FirstOrDefault(a => a.Id == c.Id))
                                      .Where(a => a != null)
                                      .ToList();

            decimal totalAmount = fullCases.Sum(i => (decimal)(i.pricers ?? 0.0)) +
                                  shortCases.Sum(i => (decimal)(i.pricers ?? 0.0));

            // Insert each selected case
            foreach (var item in fullCases)
            {
                decimal itemAmount = (decimal)(item.pricers ?? 0.0);
                db.proccases.Add(new proccas
                {
                    userid = randomUserId,
                    casetitle = item.casetitle,
                    amount = itemAmount.ToString("0.00"),
                    TotalAmount = totalAmount.ToString("0.00")
                });
            }

            // Insert short cases into proccases
            foreach (var item in shortCases)
            {
                decimal itemAmount = (decimal)(item.pricers ?? 0.0);
                db.proccases.Add(new proccas
                {
                    userid = randomUserId,
                    casetitle = item.casetitle + " (Short Case)",
                    amount = itemAmount.ToString("0.00"),
                    TotalAmount = totalAmount.ToString("0.00")
                });
            }

            db.SaveChanges();


            // Set session
            Session["uid"] = randomUserId;

            // Send confirmation email
            string body = $"<p>Dear {model.first_name} {model.last_name},</p>";
            body += "<p>Thank you for your interest. Here are the case studies you have selected:</p>";

            if (fullCases.Any() || shortCases.Any())
            {
                body += "<table border='1' cellpadding='5' cellspacing='0' style='border-collapse:collapse;'>";
                body += "<tr><th>Case Title</th><th>Price</th></tr>";

                foreach (var item in fullCases)
                {
                    decimal price = (decimal)(item.pricers ?? 0);
                    body += $"<tr><td>{item.casetitle}</td><td>{price.ToString("0.00")}</td></tr>";
                }

                foreach (var item in shortCases)
                {
                    decimal price = (decimal)(item.pricers ?? 0);
                    body += $"<tr><td>{item.casetitle} (Short Case)</td><td>{price.ToString("0.00")}</td></tr>";
                }

                body += $"<tr><td colspan='2' style='text-align:right;'><strong>Total:</strong>  {totalAmount.ToString("0.00")}</strong></td></tr>";
                body += "</table>";
            }
            else
            {
                body += "<p>No items found in your cart.</p>";
            }

            body += $"<p><strong>Your login password:</strong> {generatedPassword}</p>";
            body += "<p>We will process your request shortly. For any queries, feel free to contact us.</p>";
            body += "<p><strong>Regards,<br/>ICFAI University</strong></p>";

            string subject = "Your Selected Case Studies and Login Details";

            SendEmail(model.email, body, subject, "", "");

            return RedirectToAction("ConfirmPayment", "Home");
        }


        public void SendEmail(string EmailID, string Body, string sub, string Carbon, string BlindCarbon)
        {
            System.Net.ServicePointManager.SecurityProtocol = System.Net.SecurityProtocolType.Tls12;
            var baseAddress = "https://api.zeptomail.in/v1.1/email";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";
            http.PreAuthenticate = true;
            http.Headers.Add("Authorization", "Zoho-enczapikey PHtE6r1fSurtgmB79hJW4/G7RZHxNNwurO9hJQdE5dkTXPZWGE1Vq9kpkGDkoxt8UvRAFP6fzIpp4rqfseiGIWzsZmdLCWqyqK3sx/VYSPOZsbq6x00asFwfdUfdVofse99v1SDSudfYNA==");


            var emailContent = new
            {
                from = new { address = "noreply@icfaiuniversity.in" },
                to = new[]
                {
            new { email_address = new { address = EmailID, name = "" } }
                },
                cc = !string.IsNullOrEmpty(Carbon) ? new[] { new { email_address = new { address = Carbon } } } : null,
                bcc = !string.IsNullOrEmpty(BlindCarbon) ? new[] { new { email_address = new { address = BlindCarbon } } } : null,
                subject = sub,
                htmlbody = Body
            };


            string jsonContent = Newtonsoft.Json.JsonConvert.SerializeObject(emailContent);

            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(jsonContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
            Console.WriteLine(content);
        }


        public ActionResult PaymentMethods()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Careers()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Faqs()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult TermsOfUse()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Disclosure()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult PrivacyPolicy()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult Sitemap()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult CustomizedCaseStudies()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult CaseWritingWorkshops()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult ReprintPermission()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }
        public ActionResult IcmrCasePricing()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult ContactUs()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Achievements()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Registration()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
        public ActionResult Login()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult CheckOut()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }

        public ActionResult ByCategory(string category)
        {
            var ebooks = db.tblEbooks
                           .Where(e => e.category == category)
                           .ToList();

            ViewBag.Category = category;
            return View(ebooks);
        }

        public ActionResult ByCategoryDetails(int id)
        {
            var ebook = db.tblEbooks.FirstOrDefault(e => e.ebookId == id);
            if (ebook == null)
            {
                return HttpNotFound();
            }

            // Join ebook with related cases
            var relatedCases = db.tblEbookCases
                .Where(c => c.ebookId == id)
                .Select(c => new EbookCaseViewModel
                {
                    CaseId = c.caseId,
                    CaseTitle = c.caseTitle,
                    SubjectCategory = c.subjectCategory,
                    casecode = c.casecode,
                    Title = ebook.title,
                    Volume = ebook.volume,
                    Category = ebook.category,
                    Pages = (int)ebook.pages,
                    Prise = (decimal)ebook.prise,
                    Year = (int)ebook.year,
                    Month = (int)ebook.month,
                    CoverImg = ebook.coverImg,
                    EbookPdf = ebook.ebookPdf
                })
                .ToList();


            return View(relatedCases); // Pass list to the view
        }

        public ActionResult ByTextCategory()
        {
            var books = db.tblTextBooks
                .ToList() // Load from database first
                .Select(tb => new TextBookCaseViewModel
                {
                    TextBookId = tb.TextBookId,
                    Title = tb.Title,
                    TextBookPdf = tb.TextBookPdf,
                    WorkBookPdf = tb.WorkBookPdf,
                    TextBookPages = tb.TextBookPages,
                    WorkBookPages = tb.WorkBookPages,
                    TextBookprise = tb.TextBookprise,
                    WorkBookprise = tb.WorkBookprise,
                    tblTextBookImgs = db.tblTextBookImgs.Where(img => img.TextBookId == tb.TextBookId).ToList(),
                    tblChapters = db.tblChapters.Where(ch => ch.TextBookId == tb.TextBookId).ToList()
                })
                .ToList();

            return View(books);
        }


        public ActionResult ByTextCategoryDetails(int id)
        {
            // First fetch the entity directly
            var ebook = db.tblTextBooks.FirstOrDefault(e => e.TextBookId == id);
            if (ebook == null)
            {
                return HttpNotFound();
            }

            // Then map manually (in memory)
            var viewModel = new TextBookCaseViewModel
            {
                Title = ebook.Title,
                Volume = ebook.Volume,
                Category = ebook.Category,
                Pages = ebook.TextBookPages ?? 0,
                WorkBookPages = ebook.WorkBookPages ?? 0,
                Overview = ebook.Overview,
                Year = ebook.Year ?? 0,
                Month = ebook.Month ?? 0,
                TextBookprise = ebook.TextBookprise ?? 0,
                WorkBookprise = ebook.WorkBookprise ?? 0,
                TextBookPdf = ebook.TextBookPdf,
                //WorkBookPdf = ebook.WorkBookPdf, // Don't cast — keep as string or nullable string
                CoverImages = db.tblTextBookImgs.Where(i => i.TextBookId == id).ToList(),
                Chapters = db.tblChapters.Where(c => c.TextBookId == id).ToList()
            };

            return View(viewModel);
        }

        public ActionResult DisplayTextBooks()
        {
            var details = db.tblTextBooks.OrderByDescending(e => e.CreatedOn).ToList();
            return View(details);
        }
    }
}