using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.UI.PC.WebHelper;
using FFLTask.UI.PC.Filter;

namespace FFLTask.UI.PC.Controllers
{
    [SessionPerRequest]
    public class BaseController : Controller
    {
        public UserHelper userHelper
        {
            get
            {
                return new UserHelper(ControllerContext.HttpContext);
            }
        }

        public ImageCodeHelper imageCodeHelper
        {
            get
            {
                return new ImageCodeHelper(ControllerContext.HttpContext);
            }
        }

        public PrepageUrlHelper prepageUrlHelper
        {
            get
            {
                return new PrepageUrlHelper(ControllerContext.HttpContext);
            }

        }
    }
}
