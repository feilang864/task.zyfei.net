using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.UI.PC.Controllers;

namespace FFLTask.UI.PC.Filter
{
    public class CheckCookieEnabled : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            SetCheckEnabledCookie(filterContext.RequestContext.HttpContext);
            base.OnResultExecuted(filterContext);
        }

        private void SetCheckEnabledCookie(HttpContextBase context)
        {
            if (context.Request.Cookies[CookieKey.CheckCookieEnable] == null)
            {
                //need NOT set the value
                context.Response.Cookies.Add(new HttpCookie(CookieKey.CheckCookieEnable, "enabled"));
            }
        }
    }
}