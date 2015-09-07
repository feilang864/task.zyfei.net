using System.Collections.Generic;
using FFLTask.BLL.Entity;
using NUnit.Framework;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModelMap;

namespace FFLTask.SRV.ViewModelMapTest
{
    [TestFixture]
    public class ProjectMapTest : ProjectBaseMapTest
    {
        Authorization authorization_1, authorization_2, authorization_3, authorization_4;
        IList<Authorization> authorizations;
        Project project, project_1, project_2, project_3, project_4, project_5, project_6, project_7;

        [SetUp]
        public void Setup()
        {
            User zyfei = new User { Name = "自由飞" };
            authorization_1 = new Authorization
            {
                User = zyfei,
                IsAdmin = true,
                IsFounder = true,
                IsPublisher = false,
                IsOwner = false
            };
            zyfei.Authorizations = new List<Authorization> { authorization_1 };

            User yongbaosijie = new User { Name = "拥抱世界" };
            authorization_2 = new Authorization
            {
                User = yongbaosijie,
                IsAdmin = false,
                IsFounder = false,
                IsPublisher = true,
                IsOwner = false
                //TODO: not define the Project, OK?
            };
            yongbaosijie.Authorizations = new List<Authorization> { authorization_2 };

            User jishuzai = new User { Name = "技术宅" };
            authorization_3 = new Authorization
            {
                User = jishuzai,
                IsAdmin = false,
                IsFounder = false,
                IsPublisher = true,
                IsOwner = false
            };
            jishuzai.Authorizations = new List<Authorization> { authorization_3 };

            User kejigaibianshenghou = new User { Name = "科技改变生活" };
            authorization_4 = new Authorization
            {
                User = kejigaibianshenghou,
                IsAdmin = false,
                IsFounder = false,
                IsPublisher = false,
                IsOwner = false
            };
            kejigaibianshenghou.Authorizations = new List<Authorization> { authorization_4 };

            authorizations = new List<Authorization>
            {
                authorization_1,authorization_2,authorization_3,authorization_4
            };

            project_1 = new Project();
            project_1.MockId(1);
            project_1.Name = "创业家园";
            project_1.Description = "创业者的心灵家园";

            project_2 = new Project();
            project_2.MockId(2);
            project_2.Name = "前台";
            project_2.Description = "负责页面";

            project_3 = new Project();
            project_3.MockId(3);
            project_3.Name = "后台";
            project_3.Description = "负责业务流程";

            project_4 = new Project();
            project_4.MockId(4);
            project_4.Name = "文档";
            project_4.Description = "负责文档的编写";

            project_5 = new Project();
            project_5.MockId(5);
            project_5.Name = "任务管理";
            project_5.Description = "任务的分配承接的系统";

            project_6 = new Project();
            project_6.MockId(6);
            project_6.Name = "前台";
            project_6.Description = "负责页面";

            project_7 = new Project();
            project_7.MockId(7);
            project_7.Name = "后台";
            project_7.Description = "负责业务流程";

            project_1.AddChild(project_2);
            project_1.AddChild(project_3);
            project_1.AddChild(project_4);

            project_5.AddChild(project_6);
            project_5.AddChild(project_7);

            project = new Project
            {
                Children = new List<Project> { project_1, project_5 },
                Name = "自由飞任务管理系统",
                Description = null,
                Authorizations = authorizations
            };
            project_1.Authorizations = authorizations;
            project_2.Authorizations = authorizations;
            project_5.Authorizations = authorizations;
        }

        [Test]
        public void Summary_FilledBy_Project()
        {
            #region project

            SummaryModel model = new SummaryModel();
            model.FilledBy(project);

            summary_fill_by(model.Abstract, project);

            Assert.That(model.Projects, Is.Not.Null);
            Assert.That(model.Projects.Count, Is.EqualTo(2));
            Assert.That(model.Projects[0].Children.Count, Is.EqualTo(3));
            Assert.That(model.Projects[1].Children.Count, Is.EqualTo(2));

            Assert.That(contains(model.Projects, project_1));
            Assert.That(contains(model.Projects, project_5));

            #endregion

            #region project_1

            SummaryModel model_1 = new SummaryModel();
            model_1.FilledBy(project_1);

            summary_fill_by(model_1.Abstract, project_1);

            Assert.That(model_1.Projects, Is.Not.Null);
            Assert.That(model_1.Projects.Count, Is.EqualTo(3));
            Assert.That(contains(model_1.Projects, project_2));
            Assert.That(contains(model_1.Projects, project_3));
            Assert.That(contains(model_1.Projects, project_4));

            #endregion

            #region project_2

            SummaryModel model_2 = new SummaryModel();
            model_2.FilledBy(project_2);

            summary_fill_by(model_2.Abstract, project_2);

            Assert.That(model_2.Authorizations.Count, Is.EqualTo(4));
            Assert.That(contains(model_2.Authorizations, authorization_1));
            Assert.That(contains(model_2.Authorizations, authorization_2));
            Assert.That(contains(model_2.Authorizations, authorization_3));
            Assert.That(contains(model_2.Authorizations, authorization_4));  
          
            #endregion
        }

        private void summary_fill_by(AbstractModel model, Project project)
        {
            Assert.That(model.Description, Is.EqualTo(project.Description));
            Assert.That(model.Name, Is.EqualTo(project.Name));
            Assert.That(model.Founder.Id, Is.EqualTo(project.Founder.Id));
            Assert.That(model.Founder.Name, Is.EqualTo(project.Founder.Name));
        }
    }
}
