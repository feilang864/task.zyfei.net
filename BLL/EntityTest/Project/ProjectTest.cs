using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using Global.Core.ExtensionMethod;
using NUnit.Framework;

namespace FFLTask.BLL.EntityTest
{
    [TestFixture]
    public class ProjectTest
    {
        [Test]
        public void Top_Group()
        {
            Project root = new Project();
            Project project_1 = new Project
            {
                Parent = root
            };
            Assert.That(project_1.Root, Is.EqualTo(root));

            Project project_2 = new Project
            {
                Parent = project_1
            };
            Assert.That(project_2.Root, Is.EqualTo(root));

            Project project_3_1 = new Project
            {
                Parent = project_2
            };
            Project project_3_2 = new Project
            {
                Parent = project_2
            };
            Assert.That(project_3_1.Root, Is.EqualTo(root));
            Assert.That(project_3_2.Root, Is.EqualTo(root));
        }

        [Test]
        public void Get_Descendant_Ids()
        {
            Project project_1 = new Project();
            project_1.MockId(1);

            Project project_1_1 = new Project();
            project_1_1.MockId(11);

            Project project_1_1_1 = new Project();
            project_1_1_1.MockId(111);

            Project project_1_1_2 = new Project();
            project_1_1_2.MockId(112);

            Project project_1_1_3 = new Project();
            project_1_1_3.MockId(113);

            Project project_1_1_3_1 = new Project();
            project_1_1_3_1.MockId(1131);

            Project project_1_2 = new Project();
            project_1_2.MockId(12);

            Project project_1_3 = new Project();
            project_1_3.MockId(13);

            Project project_1_3_1 = new Project();
            project_1_3_1.MockId(131);

            project_1.Children = new List<Project> { project_1_1, project_1_2, project_1_3 };
            project_1_1.Children = new List<Project> { project_1_1_1, project_1_1_2, project_1_1_3 };
            project_1_1_3.Children = new List<Project> { project_1_1_3_1 };
            project_1_3.Children = new List<Project> { project_1_3_1 };

            IList<int> ids = project_1.GetDescendantIds();
            Assert.That(ids.Count, Is.EqualTo(9));
            Assert.That(ids.Contains(1));
            Assert.That(ids.Contains(11));
            Assert.That(ids.Contains(111));
            Assert.That(ids.Contains(112));
            Assert.That(ids.Contains(113));
            Assert.That(ids.Contains(1131));
            Assert.That(ids.Contains(12));
            Assert.That(ids.Contains(13));
            Assert.That(ids.Contains(131));
        }

        [Test]
        public void AllUsers()
        {
            Project project = new Project();
            Project other_project = new Project();

            User user = new User();
            user.Join(project);
            user.Join(other_project);

            User user_admin = new User();
            user_admin.Join(project);
            user_admin.Authorizations.FirstOrDefault().IsAdmin = true;

            User user_founder = new User();
            user_founder.Join(project);
            user_founder.Authorizations.FirstOrDefault().IsFounder = true;

            User user_publisher = new User();
            user_publisher.Join(project);
            user_publisher.Authorizations.FirstOrDefault().IsPublisher = true;

            User user_owner = new User();
            user_owner.Join(project);
            user_owner.Authorizations.FirstOrDefault().IsOwner = true;

            var allUsers = project.AllUsers;

            Assert.That(allUsers.Contains(user));
            Assert.That(allUsers.Contains(user_admin));
            Assert.That(allUsers.Contains(user_founder));
            Assert.That(allUsers.Contains(user_publisher));
            Assert.That(allUsers.Contains(user_owner));
        }

        [Test]
        public void IsOffspring()
        {
            Project child_1 = new Project();
            Project child_2 = new Project();
            Project parent_1 = new Project();
            Project parent_2 = new Project();
            Project grand_parent = new Project();

            Project other = new Project();

            parent_1.AddChild(child_1);
            parent_1.AddChild(child_2);
            grand_parent.AddChild(parent_1);
            grand_parent.AddChild(parent_2);

            Assert.That(child_1.IsOffspring(parent_1), Is.True);
            Assert.That(child_2.IsOffspring(parent_1), Is.True);
            Assert.That(parent_1.IsOffspring(parent_1), Is.False);
            Assert.That(parent_2.IsOffspring(parent_1), Is.False);
            Assert.That(grand_parent.IsOffspring(parent_1), Is.False);
            Assert.That(other.IsOffspring(parent_1), Is.False);

            Assert.That(child_1.IsOffspring(parent_2), Is.False);
            Assert.That(child_2.IsOffspring(parent_2), Is.False);
            Assert.That(parent_1.IsOffspring(parent_2), Is.False);
            Assert.That(parent_2.IsOffspring(parent_2), Is.False);
            Assert.That(grand_parent.IsOffspring(parent_1), Is.False);
            Assert.That(other.IsOffspring(parent_1), Is.False);

            Assert.That(child_1.IsOffspring(grand_parent), Is.True);
            Assert.That(child_2.IsOffspring(grand_parent), Is.True);
            Assert.That(parent_1.IsOffspring(grand_parent), Is.True);
            Assert.That(parent_2.IsOffspring(grand_parent), Is.True);
            Assert.That(grand_parent.IsOffspring(grand_parent), Is.False);
            Assert.That(other.IsOffspring(grand_parent), Is.False);
        }

        [Test]
        public void RemoveChild()
        {
            Project parent = new Project();
            Project child_1 = new Project();
            Project child_2 = new Project();

            parent.AddChild(child_1);
            parent.AddChild(child_2);

            parent.RemoveChild(child_1);

            Assert.That(parent.Children.Count, Is.EqualTo(1));
            Assert.That(parent.Children.Contains(child_2));
            Assert.That(child_1.Parent, Is.Null);
        }

        [Test]
        public void ChangeParent()
        {
            Project parent_1 = new Project();
            Project parent_2 = new Project();
            Project child = new Project();

            parent_1.AddChild(child);
            Assert.That(child.Parent, Is.EqualTo(parent_1));
            Assert.That(parent_1.Children.Count, Is.EqualTo(1));
            Assert.That(parent_1.Children.Contains(child));

            child.ChangeParent(parent_2);
            Assert.That(child.Parent, Is.EqualTo(parent_2));
            Assert.That(parent_1.Children.IsNullOrEmpty(), Is.True);
            Assert.That(parent_2.Children.Count, Is.EqualTo(1));
            Assert.That(parent_2.Children.Contains(child));
        }

        [Test]
        public void ChangeParent_When_New_Parent_Is_Old_Parent()
        {
            Project parent = new Project();
            Project child_1 = new Project();
            Project child_2 = new Project();
            parent.AddChild(child_1);
            parent.AddChild(child_2);

            child_1.ChangeParent(parent);
            Assert.That(child_1.Parent, Is.EqualTo(parent));
            Assert.That(child_2.Parent, Is.EqualTo(parent));
            Assert.That(parent.Children.Count, Is.EqualTo(2));
            Assert.That(parent.Children.Contains(child_1));
            Assert.That(parent.Children.Contains(child_2));
        }

        [Test]
        public void ChangeParent_When_New_Parent_Is_Null()
        {
            Project parent = new Project();
            Project child_1 = new Project();
            parent.AddChild(child_1);

            child_1.ChangeParent(null);
            Assert.That(child_1.Parent, Is.EqualTo(null));
            Assert.That(parent.Children.Count, Is.EqualTo(0));
        }

        [Test]
        public void ChangeParent_When_Parent_And_New_Parent_Is_Null()
        {
            Project project = new Project();

            project.ChangeParent(null);
            Assert.That(project.Parent, Is.EqualTo(null));
        }

        [Test]
        public void ChangeParent_When_Child_Has_No_Parent()
        {
            Project new_parent = new Project();
            Project child = new Project();

            child.ChangeParent(new_parent);
            Assert.That(child.Parent, Is.EqualTo(new_parent));
            Assert.That(new_parent.Children.Count, Is.EqualTo(1));
            Assert.That(new_parent.Children.Contains(child));
        }
    }
}
