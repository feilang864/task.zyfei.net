using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.ViewModelMapTest
{
    public class ProjectBaseMapTest
    {
       protected bool contains(IList<FullItemModel> model, Project project)
        {
            return model.ToList().Exists(m => (
                m.LiteItem.Id == project.Id &&
                m.LiteItem.Name == project.Name &&
                m.Description == project.Description));
        }

        protected bool contains(IList<LiteItemModel> model, Project project)
        {
            return model.ToList().Exists(m => (
                m.Id == project.Id &&
                m.Name == project.Name));
        }

        protected bool contains(IList<AuthorizationModel> model, Authorization authorization)
        {
            return model.ToList().Exists(m => (
                m.CanAdmin == authorization.IsAdmin &&
                m.CanOwn == authorization.IsOwner &&
                m.CanPublish == authorization.IsPublisher &&
                m.User.Name == authorization.User.Name));

        }
    }
}
