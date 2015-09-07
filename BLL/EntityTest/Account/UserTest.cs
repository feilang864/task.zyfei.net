using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class UserTest
    {
        [Test]
        public void Create_RootProject()
        {
            Project root = new Project();
            User executor = new User();

            executor.Create(root);

            Assert.That(executor.RootProjects.Count, Is.EqualTo(1));
            Assert.That(executor.RootProjects.Contains(root));

            project_founder_previlege(executor, root);
        }

        [Test]
        public void Create_Project_In_Parent_Project()
        {
            Project parent_Project = new Project();
            Project new_created_project = new Project
            {
                Parent = parent_Project,
                Name = "",
                Description = ""
            };
            User executor = new User();

            executor.Create(new_created_project);

            Assert.That(executor.RootProjects.Count, Is.EqualTo(1));
            Assert.That(executor.RootProjects.Contains(parent_Project));

            Assert.That(parent_Project.Children.Count == 1);
            Assert.That(parent_Project.Children.Contains(new_created_project));

            project_founder_previlege(executor, new_created_project);
        }

        private void project_founder_previlege(User user, Project project)
        {
            Assert.That(project.Founder == user);
            Assert.That(project.Owners.Contains(user));
            Assert.That(project.Publisher.Contains(user));
            Assert.That(project.Admins.Contains(user));
        }

        [Test]
        public void Join_Project_In_Parent_Project()
        {
            User user = new User();
            Project project = new Project
            {
                Parent = new Project()
            };

            user.Join(project);

            //Assert.That(user.Projects.Count, Is.EqualTo(1));
            //Assert.That(user.Projects.Contains(project));

            Assert.That(project.Authorizations.Count, Is.EqualTo(1));
            project_newbie_previlege(project.Authorizations[0]);
        }

        private void project_newbie_previlege(Authorization auth)
        {
            Assert.That(auth.IsFounder, Is.False);
            Assert.That(auth.IsPublisher, Is.False);
            Assert.That(auth.IsOwner, Is.False);
        }

        [Test]
        public void RootProjects()
        {
            User user_1 = new User();
            User user_2 = new User();
            Project project_1 = new Project();
            Project project_1_1 = new Project { Parent = project_1 };
            Project project_1_2 = new Project { Parent = project_1 };
            Project project_2 = new Project();
            Project project_2_1 = new Project { Parent = project_2 };
            Project project_3 = new Project();

            Authorization auth_1 = new Authorization
            {
                User = user_1,
                Project = project_1,
                IsFounder = true
            };
            Authorization auth_1_1 = new Authorization
            {
                User = user_1,
                Project = project_1_1,
                IsFounder = false
            };
            Authorization auth_1_2 = new Authorization
            {
                User = user_1,
                Project = project_1_2,
                IsFounder = true
            };
            user_1.Authorizations = new List<Authorization>();
            user_1.Authorizations.Add(auth_1);
            user_1.Authorizations.Add(auth_1_1);
            user_1.Authorizations.Add(auth_1_2);

            Authorization auth_2_1 = new Authorization
            {
                User = user_2,
                Project = project_2_1
            };
            Authorization auth_3 = new Authorization
            {
                User = user_2,
                Project = project_3
            };
            user_2.Authorizations = new List<Authorization>();
            user_2.Authorizations.Add(auth_2_1);
            user_2.Authorizations.Add(auth_3);

            //only the root projects are included
            Assert.That(user_1.RootProjects.Count, Is.EqualTo(1));
            Assert.That(user_1.RootProjects.Contains(project_1));

            Assert.That(user_2.RootProjects.Count, Is.EqualTo(2));
            Assert.That(user_2.RootProjects.Contains(project_2));
            Assert.That(user_2.RootProjects.Contains(project_3));
        }

        [Test]
        public void IsAdmin()
        {
            User user = new User();
            user.Authorizations = new List<Authorization>();
            Project project_1 = new Project();
            Project project_1_1 = new Project();
            Project project_2 = new Project();

            Authorization auth_1 = new Authorization { User = user, Project = project_1 };
            user.Authorizations.Add(auth_1);

            Assert.That(user.IsAdmin, Is.False);

            Authorization auth_2 = new Authorization { User = user, Project = project_2, IsAdmin = true };
            user.Authorizations.Add(auth_2);

            Assert.That(user.IsAdmin, Is.True);

            Authorization auth_1_1 = new Authorization { User = user, Project = project_1_1, IsAdmin = true };
            user.Authorizations.Add(auth_1_1);

            Assert.That(user.IsAdmin, Is.True);

            user.Authorizations.Remove(auth_2);
            Assert.That(user.IsAdmin, Is.True);

            user.Authorizations.Remove(auth_1_1);
            Assert.That(user.IsAdmin, Is.False);
        }
    }
}
