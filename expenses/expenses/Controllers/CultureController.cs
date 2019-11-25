using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace expenses.Controllers
{
    public class CultureController : Controller
    {
        // GET: /SetPreferredCulture/de-DE 
        [AllowAnonymous]
        public ActionResult SetPreferredCulture(string culture, string returnUrl)
        {
            Response.SetPreferredCulture(culture);

            if (string.IsNullOrEmpty(returnUrl))
                return RedirectToAction("Index", "Home");

            return Redirect(returnUrl);
        }

    }




}