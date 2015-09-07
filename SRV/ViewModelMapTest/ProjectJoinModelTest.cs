using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModelMap;

namespace FFLTask.SRV.ViewModelMapTest
{
    [TestFixture]
    class ProjectJoinModelTest
    {
        [Test]
        public void JoinModel_FilledBy_Project_And_User()
        {
            #region Set Projects Tree

            Project root = new Project();
            root.MockId(0);
            Project child_project_1 = new Project();
            child_project_1.MockId(1);
            Project child_project_2 = new Project();
            child_project_2.MockId(2);
            Project child_project_1_1 = new Project();
            child_project_1_1.MockId(11);
            Project child_project_1_2 = new Project();
            child_project_1_2.MockId(12);
            Project child_project_1_3 = new Project();
            child_project_1_3.MockId(13);
            Project child_project_2_1 = new Project();
            child_project_2_1.MockId(21);
            Project child_project_1_2_1 = new Project();
            child_project_1_2_1.MockId(121);
            Project child_project_1_2_2 = new Project();
            child_project_1_2_2.MockId(122);

            child_project_1_2_1.Parent = child_project_1_2;
            child_project_1_2_2.Parent = child_project_1_2;
            child_project_1_2.Children = new List<Project> { child_project_1_2_1, child_project_1_2_2 };

            child_project_1_1.Parent = child_project_1;
            child_project_1_2.Parent = child_project_1;
            child_project_1_3.Parent = child_project_1;
            child_project_1.Children = new List<Project> { child_project_1_1, child_project_1_2, child_project_1_3 };

            child_project_2_1.Parent = child_project_2;
            child_project_2.Children = new List<Project> { child_project_2_1 };

            child_project_1.Parent = root;
            child_project_2.Parent = root;
            root.Children = new List<Project> { child_project_1, child_project_2 };

            #endregion

            #region user_1_2_1

            User user_1_2_1 = new User();
            user_1_2_1.MockId(12100);
            user_1_2_1.Join(child_project_1_2_1);

            JoinModel model_1_2_1 = new JoinModel();
            model_1_2_1.FilledBy(root, user_1_2_1.Id);

            Assert.That(model_1_2_1.Children, Is.Null);
            Assert.That(model_1_2_1.HasJoined, Is.True);
            Assert.That(model_1_2_1.Item.LiteItem.Id, Is.EqualTo(0));
            Assert.That(model_1_2_1.Selected, Is.True);

            #endregion

            #region user_1_2

            #region Joined Only Single child_project_1_2

            User user_1_2 = new User();
            user_1_2.MockId(1200);
            user_1_2.Join(child_project_1_2);

            JoinModel model_1_2_single = new JoinModel();
            model_1_2_single.FilledBy(child_project_1_2, user_1_2.Id);

            //the user doesn't join child_project_1_1_x, so its Children still null
            Assert.That(model_1_2_single.Children, Is.Null);
            Assert.That(model_1_2_single.HasJoined, Is.True);
            Assert.That(model_1_2_single.Item.LiteItem.Id, Is.EqualTo(0));
            Assert.That(model_1_2_single.Selected, Is.True);

            #endregion

            #region Joined both child_project_1_2 and child_project_1_2_2

            user_1_2.Join(child_project_1_2_2);

            JoinModel model_1_2_multiple = new JoinModel();
            model_1_2_multiple.FilledBy(child_project_1_2, user_1_2.Id);

            Assert.That(model_1_2_multiple.HasJoined, Is.True);
            Assert.That(model_1_2_multiple.Item.LiteItem.Id, Is.EqualTo(0));
            Assert.That(model_1_2_multiple.Selected, Is.True);

            Assert.That(model_1_2_multiple.Children.Count, Is.EqualTo(2));

            JoinModel model_1_2_child_1 = model_1_2_multiple.Children.Where(m => m.Item.LiteItem.Id == child_project_1_2_1.Id).SingleOrDefault();
            Assert.That(model_1_2_child_1.HasJoined, Is.False);
            Assert.That(model_1_2_child_1.Selected, Is.False);

            JoinModel model_1_2_child_2 = model_1_2_multiple.Children.Where(m => m.Item.LiteItem.Id == child_project_1_2_2.Id).SingleOrDefault();
            Assert.That(model_1_2_child_2.HasJoined, Is.True);
            Assert.That(model_1_2_child_2.Selected, Is.True);

            #endregion

            #endregion

            #region user_1

            User user_1 = new User();
            user_1.MockId(1);

            user_1.Join(child_project_1);
            user_1.Join(child_project_1_1);
            user_1.Join(child_project_1_2_1);
            user_1.Join(child_project_1_2_2);

            JoinModel model_1 = new JoinModel();
            model_1.FilledBy(child_project_1, user_1.Id);

            //make sure child_project_1_3 is in the tree
            Assert.That(model_1.Children.Count, Is.EqualTo(3));

            //make sure child_project_1_2_1, child_project_1_2_2 is in the tree
            JoinModel model_1_child_2 = model_1.Children.Where(m => m.Item.LiteItem.Id == child_project_1_2.Id).SingleOrDefault();
            Assert.That(model_1_child_2.Children.Count, Is.EqualTo(2));
            //and child_project_1_2 is not joined
            Assert.That(model_1_child_2.HasJoined, Is.False);
            Assert.That(model_1_child_2.Selected, Is.False);

            #endregion
        }
    }
}
