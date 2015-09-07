using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace FFLTask.UI.PC.Controllers
{
    public class HelpController : Controller
    {
        public ActionResult Account()
        {
            return View();
        }

        public ActionResult Project()
        {
            return View();
        }

        public ActionResult Team()
        {
            return View();
        }

        public ActionResult Task()
        {
            return View();
        }

        public ActionResult TaskChild(string child)
        {
            string viewName = string.Format("~/Views/Help/Task/{0}.cshtml", child);
            return View(viewName);
        }

        public ActionResult Welcome()
        {
            return View();
        }


    }
}
