using FFLTask.BLL.Entity;
using NHibernate;

namespace FFLTask.Tool.BuildDatabase
{
    class TaskFactory
    {
        private static readonly ISession session = NHProvider.session;

        internal static Task CreateTask(Project project,
            User publisher, User accepter, string title)
        {
            return CreateTask(project, publisher, accepter, null, title);
        }

        internal static Task CreateTask(Project project, 
            User publisher = null,
            User accepter = null, 
            User owner = null, 
            string title = "")
        {
            Task task = new Task
            {
                Title = title,
                Project = project,
                Publisher = publisher,
                Accepter = accepter,
                Owner = owner
            };
            task.Publish();

            session.Save(task);

            return task;
        }
    }
}
