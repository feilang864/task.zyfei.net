using System.Collections.Generic;
using System.Linq;
using Global.Core.ExtensionMethod;

namespace FFLTask.BLL.Entity
{
    public class User : BaseEntity
    {
        #region Mapped

        public virtual string Name { get; set; }
        public virtual string Password { get; set; }
        public virtual IList<Authorization> Authorizations { get; set; }
        public virtual string RealName { get; set; }
        public virtual string AuthenticationCode { get; set; }
        //public virtual SelfIntroduction SelfIntroduction { get; set; }
        public virtual IList<Message> MessagesToMe { get; set; }
        public virtual IList<Message> MessagesFromMe { get; set; }

        public virtual Profile Profile { get; set; }
        
        #endregion

        #region UnMapped

        /// <summary>
        /// the root projects of all the projects that the current user has joined
        /// </summary>
        public virtual IList<Project> RootProjects
        {
            get
            {
                var result = from auth in Authorizations
                             select auth.Project.Root;
                return result.Distinct().ToList();
            }
        }

        public virtual bool IsAdmin
        {
            get
            {
                return !Authorizations.IsNullOrEmpty() &&
                    Authorizations.Count(a => a.IsAdmin) > 0;
            }
        }

        #endregion

        public virtual void Create(Project project)
        {
            Authorization auth = new Authorization
            {
                User = this,
                Project = project,
                IsFounder = true,
                IsPublisher = true,
                IsOwner = true,
                IsAdmin = true
            };

            Add(project, auth);
        }

        public virtual void Join(Project project)
        {
            Authorization auth = new Authorization
            {
                User = this,
                Project = project
            };

            Add(project, auth);
        }

        protected virtual void Add(Project project, Authorization auth)
        {
            Authorizations = Authorizations ?? new List<Authorization>();
            Authorizations.Add(auth);

            project.Authorizations = project.Authorizations ?? new List<Authorization>();
            project.Authorizations.Add(auth);

            //TODO: not sure 
            if (project.Parent != null)
            {
                project.Parent.AddChild(project);
            }
        }
    }
}
