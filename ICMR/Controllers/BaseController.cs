﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace ICMR.Controllers
{
    public class BaseController : Controller
    {
        // GET: Base
        protected override void OnActionExecuting(ActionExecutingContext filterContext)
        {
            HttpCachePolicyBase cache = filterContext.HttpContext.Response.Cache;
            cache.SetExpires(DateTime.UtcNow.AddDays(-1));
            cache.SetValidUntilExpires(false);
            cache.SetRevalidation(HttpCacheRevalidation.AllCaches);
            cache.SetCacheability(HttpCacheability.NoCache);
            cache.SetNoStore();

            base.OnActionExecuting(filterContext);
        }

    }
}
