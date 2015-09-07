using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;

namespace FFLTask.BLL.NHMapTest
{
    [SetUpFixture]
    public class BuildDatabase
    {
        [SetUp]
        public void Build()
        {
            //umcomment it when debug
            //log4net.Config.XmlConfigurator.Configure();

            new DAL.ADO.BuildDatabase().Create("task_map_test");
            createTables();
        }

        protected void createTables()
        {
            var schema = new SchemaExport(NHConfigProvider.Get());
            schema.Create(false, true);
        }
    }
}
