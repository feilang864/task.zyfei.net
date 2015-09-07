using System.Collections.Generic;
using FFLTask.BLL.Entity;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    [TestFixture]
    class ProjectMapTest : BaseMapTest<Project>
    {
        [Test]
        public void Normal()
        {
            string project_description = "this is DBA's description";
            string project_name = "DBA";

            Project project_1 = new Project();
            Project project_2 = new Project();
            Authorization Authorization_1 = new Authorization();
            Authorization Authorization_2 = new Authorization();
            string project_config_str_difficulties = "this is project difficulties";
            string project_config_str_prioritys = "this is project prioritys";
            string project_config_str_qualities = "this is project qualities";
            ProjectConfig project_config_1 = new ProjectConfig
            {
                StrDifficulties = project_config_str_difficulties,
                StrPrioritys = project_config_str_prioritys,
                StrQualities = project_config_str_qualities
            };
            Project project = new Project
            {
                Authorizations = new List<Authorization> { Authorization_1, Authorization_2 },
                Children = new List<Project> { project_1, project_2 },
                Parent = new Project(),
                Config = project_config_1,
                Name = project_name,
                Description = project_description,
            };
            Authorization_1.Project = project;
            Authorization_2.Project = project;
            project_1.Parent = project;
            project_2.Parent = project;

            Project load_project = Save(project);

            Assert.That(load_project.Description, Is.EqualTo(project_description));
            Assert.That(load_project.Name, Is.EqualTo(project_name));
            Assert.That(load_project.Authorizations.Count, Is.EqualTo(2));
            Assert.That(load_project.Children.Count, Is.EqualTo(2));
            DBAssert.AreInserted(load_project.Parent);
            Assert.That(load_project.Config.StrDifficulties, Is.EqualTo(project_config_str_difficulties));
            Assert.That(load_project.Config.StrPrioritys, Is.EqualTo(project_config_str_prioritys));
            Assert.That(load_project.Config.StrQualities, Is.EqualTo(project_config_str_qualities));
        }
    }
}
