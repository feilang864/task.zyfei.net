using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.ServiceInterface
{
    public interface IProjectConfigService
    {
        IList<TaskDifficulty> GetDifficulties(int projectId);
        IList<TaskQuality> GetQualities(int projectId);
        IList<FFLTask.SRV.ViewModel.Task.StatusModel> GetStatus(int projectId);
        IList<TaskPriority> GetPriorities(int projectId);
    }
}
