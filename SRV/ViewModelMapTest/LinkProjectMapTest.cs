using System.Collections.Generic;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModelMap;
using NUnit.Framework;

namespace FFLTask.SRV.ViewModelMapTest
{
    [TestFixture]
    public class LinkProjectMapTest : ProjectBaseMapTest
    {
        Project root;
        Project project_1, project_2;
        Project project_2_1, project_2_2, project_2_3;
        LinkedList<_DropdownlistLinkedNodeModel> models;

        [SetUp]
        public void SetUp()
        {
            root = new Project();
            models = new LinkedList<_DropdownlistLinkedNodeModel>();
        }

        private void fill_1_level()
        {
            project_1 = new Project { Parent = root };
            project_1.MockId(1);

            project_2 = new Project { Parent = root };
            project_2.MockId(2);

            root.Children = new List<Project> { project_1, project_2 };
        }

        private void fill_2_level()
        {
            project_2_1 = new Project { Parent = project_2 };
            project_2_1.MockId(21);

            project_2_2 = new Project { Parent = project_2 };
            project_2_2.MockId(22);

            project_2_3 = new Project { Parent = project_2 };
            project_2_3.MockId(23);

            project_2.Children = new List<Project> { project_2_1, project_2_2, project_2_3 };
        }

        [Test]
        public void LinkedProjects_FilledByHead_From_Root()
        {
            fill_1_level();
            fill_2_level();

            models.FilledByHead(root);
            assert_by_head_fill_root(models.First, project_1.Id);
        }

        [Test]
        public void LinkedProjects_FilledByHead_From_1Level()
        {
            fill_1_level();
            fill_2_level();

            models.FilledByHead(project_2);
            assert_by_head_fill_1_level(models.First, project_2_1.Id);
        }

        private void assert_by_head_fill_root(LinkedListNode<_DropdownlistLinkedNodeModel> first, int projectId)
        {
            Assert.That(first.Value.CurrentProject.Id, Is.EqualTo(projectId));
            Assert.That(first.Value.Projects.Count, Is.EqualTo(2));
            Assert.That(contains(first.Value.Projects, project_1));
            Assert.That(contains(first.Value.Projects, project_2));
        }

        private void assert_by_head_fill_1_level(LinkedListNode<_DropdownlistLinkedNodeModel> second, int projectId)
        {
            Assert.That(second.Value.CurrentProject.Id, Is.EqualTo(projectId));
            Assert.That(second.Value.Projects.Count, Is.EqualTo(3));
            Assert.That(contains(second.Value.Projects, project_2_1));
            Assert.That(contains(second.Value.Projects, project_2_2));
            Assert.That(contains(second.Value.Projects, project_2_3));
        }

        [Test]
        public void LinkedProjects_FilledByTail_Project_1Level()
        {
            fill_1_level();

            models.FilledByTail(project_2);

            Assert.That(models.Count, Is.EqualTo(2));
            assert_by_tail_fill_root();
            assert_by_tail_fill_1_level();
        }

        [Test]
        public void LinkedProjects_FilledByTail_Project_2Level()
        {
            fill_1_level();
            fill_2_level();

            models.FilledByTail(project_2_2);

            Assert.That(models.Count, Is.EqualTo(3));
            assert_by_tail_fill_root();
            assert_by_tail_fill_1_level();
            assert_by_tail_fill_2_level();
        }

        private void assert_by_tail_fill_root()
        {
            LinkedListNode<_DropdownlistLinkedNodeModel> first = models.First;
            Assert.That(first.Value.CurrentProject.Id, Is.EqualTo(root.Id));
        }

        private void assert_by_tail_fill_1_level()
        {
            LinkedListNode<_DropdownlistLinkedNodeModel> second = models.First.Next;
            Assert.That(second.Value.CurrentProject.Id, Is.EqualTo(project_2.Id));
            Assert.That(second.Value.Projects.Count, Is.EqualTo(2));
            Assert.That(contains(second.Value.Projects, project_1));
            Assert.That(contains(second.Value.Projects, project_2));
        }

        private void assert_by_tail_fill_2_level()
        {
            LinkedListNode<_DropdownlistLinkedNodeModel> third = models.Last;
            Assert.That(third.Value.CurrentProject.Id, Is.EqualTo(project_2_2.Id));
            Assert.That(third.Value.Projects.Count, Is.EqualTo(3));
            Assert.That(contains(third.Value.Projects, project_2_1));
            Assert.That(contains(third.Value.Projects, project_2_2));
            Assert.That(contains(third.Value.Projects, project_2_3));
        }
    }
}
