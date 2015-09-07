using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Task;

namespace FFLTask.UI.PC.Controllers
{
    public class PreferController : BaseController
    {
        private IProjectConfigService _projectConfigService;

        public PreferController(IProjectConfigService projectConfigService)
        {
            _projectConfigService = projectConfigService;
        }

        public ActionResult Status(int projectId)
        {
            IList<StatusModel> models = _projectConfigService.GetStatus(projectId);

            if (Request.Cookies[CookieKey.PreferStatus] != null)
            {
                string prefer_status = Server.UrlDecode(Request.Cookies[CookieKey.PreferStatus].Value);
                string[] statuss = prefer_status.Split(",".ToCharArray());
                for (int i = 0; i < statuss.Length - 1; i++)
                {
                    StatusModel model = models.Where(x => x.Stage == int.Parse(statuss[i])).SingleOrDefault();
                    if (model != null)
                    {
                        model.Checked = true;
                    }
                }
            }

            return View(models);
        }

        [HttpPost]
        public ActionResult Status(IList<StatusModel> model)
        {
            return View(model);
        }
    }
}
