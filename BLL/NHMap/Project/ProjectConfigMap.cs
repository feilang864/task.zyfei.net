using FFLTask.BLL.Entity;
using FluentNHibernate.Mapping;

namespace FFLTask.BLL.NHMap
{
    class ProjectConfigMap : ComponentMap<ProjectConfig>
    {
        public ProjectConfigMap()
        {
            Map(m => m.StrDifficulties);
            Map(m => m.StrPrioritys);
            Map(m => m.StrQualities);
        }
    }
}
