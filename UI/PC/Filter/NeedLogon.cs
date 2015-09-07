using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.UI.PC.WebHelper;
using FFLTask.UI.PC.Controllers;

namespace FFLTask.UI.PC.Filter
{
    public class NeedAuthorized : AuthorizeAttribute
    {
        private Privilege _token;
        public NeedAuthorized()
        {
            _token = Privilege.Logon;
        }

        public NeedAuthorized(Privilege token)
        {
            _token = token;
        }

        public override void OnAuthorization(AuthorizationContext filterContext)
        {
            UserHelper userHelper = (filterContext.Controller as BaseController).userHelper;

            if (!allowAccess(userHelper))
            {
                HttpRequest Request = HttpContext.Current.Request;
                HttpServerUtility Server = HttpContext.Current.Server;

                string wantAccess = Server.UrlEncode(Request.Url.PathAndQuery);
                string url = @"/Log/On?refuse=1&prepage=" + wantAccess;

                //TODO: don't know why this code is necessary, 
                //else cause 401 error when deploying on IIS7.5
                HttpContext.Current.Response.StatusCode = 302;
                filterContext.Result = new RedirectResult(url);
            }

            //base.OnAuthorization(filterContext);
        }

        private bool allowAccess(UserHelper userHelper)
        {
            if (userHelper.CurrentUserId != null)
            {
                if (_token == Privilege.Logon)
                {
                    return true;
                }
                else if (_token == Privilege.Admin)
                {
                    return userHelper.CurrentUser.IsAdmin;
                }
            }
            return false;
        }
    }

    public enum Privilege
    {
        Logon,
        Admin
    }
}