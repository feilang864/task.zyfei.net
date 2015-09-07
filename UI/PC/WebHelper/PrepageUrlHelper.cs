using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using FFLTask.UI.PC.WebHelper;

using FFLTask.SRV.ServiceInterface;

namespace FFLTask.UI.PC.WebHelper
{
    public class PrepageUrlHelper : Controller
    {
        public HttpContextBase Context { get; set; }
        public PrepageUrlHelper(HttpContextBase context)
        {
            Context = context;
        }

        /// <summary>
        /// get prepage from url parameter
        /// </summary>
        /// <returns>can be string.empty if no such url parameter</returns>
        public string GetPrepageFromUrl()
        {
            //why this is still necessary but not get it from Action's parameter
            //for it's still used by ChildAction, etc
            return Context.Request.QueryString["prepage"];
        }

        /// <summary>
        /// set the prepage on url
        /// </summary>
        /// <returns>if there has been one prepage parameter in the url, use the url from url parameter
        /// else use the current url</returns>
        public string SetPrepageForUrl()
        {
            //check if there has been prepage first
            //only when there is no prepage, then use current url as prepage
            if (string.IsNullOrEmpty(GetPrepageFromUrl()))
            {
                return Context.Server.UrlEncode(Context.Request.Url.PathAndQuery);
            }
            else
            {
                return Context.Server.UrlEncode(GetPrepageFromUrl());
            }
        }

        /// <summary>
        /// redirect the prepage:
        /// if there is no prepage in the url, to default url;
        /// else to the prepage from url
        /// </summary>
        public RedirectResult ReturnPrePage(string defaultUrl)
        {
            if (!string.IsNullOrEmpty(GetPrepageFromUrl()))
            {
                return new RedirectResult(GetPrepageFromUrl());
            }
            else
            {
                return new RedirectResult(defaultUrl);
            }
        }

        /// <summary>
        /// redirect to the home page when there is no prepage in the url
        /// </summary>
        public RedirectResult ReturnPrePage()
        {
            return ReturnPrePage(@"/");
        }
    }
}