using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Task;

namespace FFLTask.SRV.UIDevService
{
    public class ProjectConfigService : IProjectConfigService
    {
        public IList<TaskDifficulty> GetDifficulties(int projectId)
        {
            IList<TaskDifficulty> difficulties = new List<TaskDifficulty>();
            FakeProjects Project = (FakeProjects)projectId;
            switch (Project)
            {
                case FakeProjects.UI:
                case FakeProjects.后台:
                    difficulties.Add(TaskDifficulty.Easiest);
                    difficulties.Add(TaskDifficulty.Easy);
                    difficulties.Add(TaskDifficulty.Common);
                    difficulties.Add(TaskDifficulty.Hard);
                    difficulties.Add(TaskDifficulty.Hardest);
                    break;
                case FakeProjects.美工:
                    difficulties.Add(TaskDifficulty.Easy);
                    difficulties.Add(TaskDifficulty.Common);
                    difficulties.Add(TaskDifficulty.Hard);
                    break;
                default:
                    break;

            }
            return difficulties;

        }

        public IList<TaskQuality> GetQualities(int projectId)
        {
            return new List<TaskQuality>
            {
                TaskQuality.Qualified,
                TaskQuality.Good,
                TaskQuality.Perfect
            };
        }

        public IList<StatusModel> GetStatus(int projectId)
        {
            return new List<StatusModel>
            {
                new StatusModel { Stage = (int)FakeStatus.发布, Name =FakeStatus.发布.ToString()},
                new StatusModel { Stage = (int)FakeStatus.分配, Name =FakeStatus.分配.ToString()},
                new StatusModel { Stage = (int)FakeStatus.承接, Name =FakeStatus.承接.ToString()},
                //new StatusModel { Stage = (int)FakeStatus.暂停, Name =FakeStatus.暂停.ToString()},
                new StatusModel { Stage = (int)FakeStatus.完成, Name =FakeStatus.完成.ToString()},
                //new StatusModel { Stage = (int)FakeStatus.取消, Name =FakeStatus.取消.ToString()},
                new StatusModel { Stage = (int)FakeStatus.验收通过, Name =FakeStatus.验收通过.ToString()},
                new StatusModel { Stage = (int)FakeStatus.验收失败, Name =FakeStatus.验收失败.ToString()},
            };
        }

        public IList<TaskPriority> GetPriorities(int projectId)
        {
            if (projectId == (int)FakeProjects.美工
                || projectId == (int)FakeProjects.UI)
            {
                return new List<TaskPriority> 
                {
                    TaskPriority.High, 
                    TaskPriority.Common, 
                    TaskPriority.Low 
                };
            }
            else if (projectId == (int)FakeProjects.后台)
            {
                return new List<TaskPriority> 
                {
                    TaskPriority.Highest,
                    TaskPriority.High,
                    TaskPriority.Common,
                    TaskPriority.Low,
                    TaskPriority.Lowest 
                };
            }
            else
            {
                return new List<TaskPriority> { };
            }
        }
    }
}
