using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;
using NUnit.Framework;
using FFLTask.SRV.Query;

namespace FFLTask.SRV.QueryTest
{
    [TestFixture]
    class ProjectQueryTest
    {
        [Test]
        public void Get_By_Name()
        {
            string project_name_1 = "zyfei";
            string project_name_2 = "DBA";
            Project project_1 = new Project { Name = project_name_1 };
            Project project_2 = new Project { Name = project_name_2 };
            Project project_3 = new Project { Name = project_name_1 };

            IQueryable<Project> iqueryable = new List<Project> 
            {
                project_1,project_2,project_3
            }.AsQueryable();

            ProjectQuery query = new ProjectQuery(iqueryable);
            IList<Project> projects = query.GetByName(project_name_2).ToList();

            Assert.That(projects.Count, Is.EqualTo(1));
            Assert.That(projects.Contains(project_2));
        }
    }
}
