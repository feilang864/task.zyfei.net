using System;
using System.Web;
using Autofac;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.UI.PC.CustomException;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;

namespace FFLTask.UI.PC.WebHelper
{
    public class UserHelper
    {
        public UserHelper()
        {

        }

        public UserHelper(HttpContextBase context)
        {
            Context = context;
        }

        public HttpContextBase Context { get; set; }

        /// <summary>
        /// will never be null in such process
        /// </summary>
        public int? CurrentUserId
        {
            get
            {
                return CurrentUser.Id;
            }
        }

        public UserModel CurrentUser
        {
            get
            {
                UserModel model = new UserModel();
                string key = "usermodelid";

                //use http context directly
                if (Context.Items[key] != null)
                {
                    return (UserModel)Context.Items[key];
                }

                var userIdentity = Context.Request.Cookies[CookieKey.UserId];
                if (userIdentity != null)
                {
                    try
                    {
                        //try to get from cookie
                        int userId = Convert.ToInt32(userIdentity.Values[0]);
                        string encryptInfo = userIdentity.Values[1];

                        if (string.IsNullOrEmpty(encryptInfo))
                        {
                            string message = string.Format("cookie for user id(id={0}) has wrong encryption info.", userId);
                            throw new CookieParseException(message);
                        }

                        using (var scope = MvcApplication.container.BeginLifetimeScope())
                        {
                            var accountService = scope.Resolve<IUserService>();
                            model = accountService.GetUser(userId);

                            if (model.AuthCode.Md5Encypt() != encryptInfo)
                            {
                                string message = string.Format("cookie for user id(id={0}) has wrong encryption info.", userId);
                                throw new CookieParseException(message);
                            }

                            Context.Items[key] = model;
                        }
                    }
                    catch (System.Exception e)
                    {
                        LogOff();

                        var log = log4net.LogManager.GetLogger("KnownError");
                        //TODO: probably need more information later.
                        log.Error(e.Message);
                    }
                }
                return model;
            }
        }

        /// <summary>
        /// key: id; value: encypted id
        /// </summary>
        /// <param name="user"></param>
        /// <param name="remember"></param>
        public void SetUserId(string userId, int? days)
        {
            HttpCookie cookie = new HttpCookie(CookieKey.UserId, userId.ToString());

            using (var scope = MvcApplication.container.BeginLifetimeScope())
            {
                var accountService = scope.Resolve<IUserService>();
                UserModel user = accountService.GetUser(Convert.ToInt32(userId));
                cookie.Values.Add(CookieKey.AuthCode, user.AuthCode.Md5Encypt());
            }

            if (days.HasValue)
            {
                cookie.Expires = DateTime.Now.AddDays(days.Value);
            }
            Context.Response.Cookies.Add(cookie);
        }

        public void SetUserId(string userId)
        {
            SetUserId(userId, null);
        }

        public void LogOff()
        {
            if (Context.Request.Cookies[CookieKey.UserId] != null)
            {
                HttpCookie myCookie = new HttpCookie(CookieKey.UserId);
                myCookie.Expires = SystemTime.Now().AddDays(-1d);
                Context.Response.Cookies.Add(myCookie);
            }
        }

        //TODO: Should move into CookieHelper
        public void RemoveCookie(string name)
        {
            if (Context.Request.Cookies[name] != null)
            {
                HttpCookie myCookie = new HttpCookie(name);
                myCookie.Expires = SystemTime.Now().AddDays(-1d);
                Context.Response.Cookies.Add(myCookie);
            }
        }
    }
}