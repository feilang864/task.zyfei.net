using System.Web.Mvc;

namespace FFLTask.UI.PC.Filter
{
    ///<summary>
    ///Use this filter for actions that need to persist state.
    ///</summary>
    public class SessionPerRequest : ActionFilterAttribute
    {
        public override void OnResultExecuted(ResultExecutedContext filterContext)
        {
            #if PROD
            FFLTask.SRV.ProdService.BaseService.EndSession();
            #endif

            base.OnResultExecuted(filterContext);
        }
    }
}