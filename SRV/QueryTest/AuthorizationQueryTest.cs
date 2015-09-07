using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.Query;
using NHibernate.Linq;
using NUnit.Framework;

namespace FFLTask.SRV.QueryTest
{
    [TestFixture]
    public class AuthorizationQueryTest : BaseQueryTest
    {
        [Test]
        public void Get_By_ProjectIds()
        {
            Project project_1 = new Project();
            Project project_2 = new Project();

            Authorization auth_project_1_1 = create_authorization(project_1);
            Authorization auth_project_1_2 = create_authorization(project_1);
            Authorization auth_project_2 = create_authorization(project_2);

            var query = session.Query<Authorization>();

            IList<int> projectIds;

            projectIds = new List<int>();
            var result_no_projectIds = query.InProjects(projectIds).ToList();
            Assert.That(result_no_projectIds.Count, Is.EqualTo(0));

            projectIds = new List<int> { project_1.Id };
            var result_project_1 = query.InProjects(projectIds).ToList();
            Assert.That(result_project_1.Count, Is.EqualTo(2));
            Contains(result_project_1, auth_project_1_1);
            Contains(result_project_1, auth_project_1_2);

            projectIds = new List<int> { project_2.Id };
            var result_project_2 = query.InProjects(projectIds).ToList();
            Assert.That(result_project_2.Count, Is.EqualTo(1));
            Contains(result_project_2, auth_project_2);

            projectIds = new List<int> { project_1.Id, project_2.Id };
            var result_project_1_and_project_2 = query.InProjects(projectIds).ToList();
            Assert.That(result_project_1_and_project_2.Count, Is.EqualTo(3));
            Contains(result_project_1_and_project_2, auth_project_1_1);
            Contains(result_project_1_and_project_2, auth_project_1_2);
            Contains(result_project_1_and_project_2, auth_project_2);


        }

        private Authorization create_authorization(Project project)
        {
            Authorization auth = new Authorization
            {
                Project = project
            };
            Save(auth);

            return auth;
        }
    }
}
