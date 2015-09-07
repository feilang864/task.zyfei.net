using FFLTask.BLL.NHMap;
using NUnit.Framework;
using Helper = Global.NHibernateTestHelper;

namespace FFLTask.SRV.QueryTest
{
    [SetUpFixture]
    public class BuildDatabase
    {
        [SetUp]
        public void Build()
        {

            Helper.BuildDatabase.CreateDatabase(Constant.DB_Name);
            Helper.BuildDatabase.CreateTables(ConfigurationProvider.Action, Constant.DB_Name, false);
        }
    }
}
