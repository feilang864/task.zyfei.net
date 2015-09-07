using System.Collections.Generic;
using System.Linq;
using Global.Core.ExtensionMethod;

namespace FFLTask.BLL.Entity
{
    public class Project : BaseEntity
    {
        #region Mapped

        public virtual string Name { get; set; }
        public virtual string Description { get; set; }
        public virtual IList<Authorization> Authorizations { get; set; }
        public virtual Project Parent { get; set; }
        public virtual IList<Project> Children { get; set; }

        private ProjectConfig _config;
        public virtual ProjectConfig Config
        {
            get { return _config ?? new ProjectConfig(); }
            set { _config = value; }
        }

        #endregion

        #region UnMapped

        public virtual IList<User> Publisher
        {
            get
            {
                var assigner = from a in Authorizations
                               where a.IsPublisher
                               select a.User;
                return assigner.ToList();
            }
        }

        public virtual IList<User> Owners
        {
            get
            {
                var owner = from a in Authorizations
                            where a.IsOwner
                            select a.User;
                return owner.ToList();
            }
        }

        public virtual IList<User> Admins
        {
            get
            {
                var owner = from a in Authorizations
                            where a.IsAdmin
                            select a.User;
                return owner.ToList();
            }
        }

        public virtual IList<User> AllUsers
        {
            get
            {
                return Authorizations
                    .Select(a => a.User).ToList();
            }
        }

        public virtual User Founder
        {
            get
            {
                var founder = from a in Authorizations
                              where a.IsFounder
                              select a.User;
                return founder.Single();
            }
        }

        public virtual Project Root
        {
            get
            {
                if (Parent == null)
                {
                    return this;
                }
                else
                {
                    return Parent.Root;
                }
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// one project is Not its own Offspring
        /// </summary>
        public virtual bool IsOffspring(Project project)
        {
            //one project can not be his own ancestor 
            if (project == this)
            {
                return false;
            }
            return project.IsAncestor(this);
        }

        /// <summary>
        /// one project is Not its own Ancestor
        /// </summary>
        public virtual bool IsAncestor(Project project)
        {
            //one project can not be his own ancestor 
            if (project == this)
            {
                return false;
            }
            while (project != null)
            {
                if (project == this)
                {
                    return true;
                }
                project = project.Parent;
            }
            return false;
        }

        public virtual void AddChild(Project project)
        {
            project.Parent = this;
            Children = Children ?? new List<Project>();
            Children.Add(project);
        }

        public virtual void RemoveChild(Project project)
        {
            Children.Remove(project);
            project.Parent = null;
        }

        public virtual void ChangeParent(Project newParent)
        {
            if (this.Parent != newParent)
            {
                if (this.Parent == null)
                {
                    newParent.AddChild(this);
                }
                else
                {
                    this.Parent.RemoveChild(this);
                    if (newParent != null)
                    {
                        newParent.AddChild(this);
                    }
                }
            }
        }

        public virtual IList<int> GetDescendantIds()
        {
            IList<int> ids = new List<int>();
            getChildren(this, ids);
            return ids;
        }

        private void getChildren(Project project, IList<int> ids)
        {
            ids.Add(project.Id);
            if (!project.Children.IsNullOrEmpty())
            {
                foreach (var child in project.Children)
                {
                    getChildren(child, ids);
                }
            }
        }

        #endregion
    }
}
