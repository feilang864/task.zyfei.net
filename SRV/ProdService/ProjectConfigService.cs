using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.SRV.ViewModelMap;

namespace FFLTask.SRV.ProdService
{
    public class ProjectConfigService : BaseService, IProjectConfigService
    {
        public IList<TaskDifficulty> GetDifficulties(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return project.Config.GetDifficulties();
        }

        public IList<TaskQuality> GetQualities(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return project.Config.GetQualities();
        }

        public IList<StatusModel> GetStatus(int projectId)
        {
            IList<StatusModel> models = new List<StatusModel>();

            var allStatus = Enum.GetValues(typeof(Status)).Cast<Status>();
            foreach (var status in allStatus)
            {
                StatusModel model = new StatusModel();
                model.FilledBy(status);
                models.Add(model);
            };
            return models;
        }

        public IList<TaskPriority> GetPriorities(int projectId)
        {
            Project project = session.Load<Project>(projectId);

            return project.Config.GetPrioritys();
        }
    }
}
