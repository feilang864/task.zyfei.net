using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.Tool.BuildDatabase
{
    class AuthorizationFactory
    {
        internal static void SetAuth(User user, Project project,
            bool isAdmin = true,
            bool isPublisher = true,
            bool isOwner = true)
        {
            if (user.Authorizations != null)
            {
                Authorization auth = user.Authorizations
                    .Where(a => a.Project == project).SingleOrDefault(); 
                if ( auth == null)
                {
                    addAuth(user, project, isAdmin, isPublisher, isOwner);    
                }
                else
                {
                    setAuth(auth, isAdmin, isPublisher, isOwner);
                }
            }
            else
            {
                createAuth(user,project,isAdmin,isPublisher,isOwner);
            }
        }

        internal static void createAuth(User user, Project project,
            bool isAdmin = true,
            bool isPublisher = true,
            bool isOwner = true)
        {
            Authorization auth = new Authorization { User = user, Project = project };
            user.Authorizations = new List<Authorization> { auth };
            setAuth(auth, isAdmin, isPublisher, isOwner);
        }

        internal static void addAuth(User user, Project project,
            bool isAdmin = true,
            bool isPublisher = true,
            bool isOwner = true)
        {
            Authorization auth = new Authorization { User = user, Project = project };
            setAuth(auth, isAdmin, isPublisher, isOwner);
            user.Authorizations.Add(auth);
        }

        private static void setAuth(Authorization auth,
            bool isAdmin = true,
            bool isPublisher = true,
            bool isOwner = true)
        {
            auth.IsAdmin = isAdmin;
            auth.IsPublisher = isPublisher;
            auth.IsOwner = isOwner;
        }
    }
}
