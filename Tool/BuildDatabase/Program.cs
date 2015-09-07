using System;
using System.IO;
using FFLTask.Tool.BuildDatabase;
using FFLTask.Tool.BuildDatabase.Factory.Special;
using NHibernate;
using NHibernate.Tool.hbm2ddl;

namespace FFLTask.BuildDatabase
{
    class Program
    {
        static void Main(string[] args)
        {
            log4net.Config.XmlConfigurator.Configure();
#if Release
            UpdateSchema();
#else
            CreateDatabase();
            BuildSchema();
            TellStory();
#endif
            Console.WriteLine("finished!");
            Console.Read();
        }

        private static void TellStory()
        {
            ISession session = NHProvider.session;
            using (var tran = session.BeginTransaction())
            {
                UserFactory.Create();
                ProjectFactory.Create();
                ProjectFactory.Join();
                ProjectFactory.SetAssigner();
                TaskRelationFactory.Create();
                TaskListFactory.Create();

                ProjectEditFactory.Create();
                ProjectJoinTestFactory.Create();
                TaskEditProcessTestFactory.Create();

                NoPrivilegeInProject.Create();
                TaskPreAndNextFactory.Create();
                TaskSumFactory.Create();
                ToMeListFactory.Create();
                FromMeListFactory.Create();

                SearchFactory.Create();

                try
                {
                    tran.Commit();
                }
                catch (Exception)
                {
                    tran.Rollback();
                    throw;
                }

            }
        }

        static NHibernate.Cfg.Configuration config = NHProvider.getConfig().BuildConfiguration();

        static void BuildSchema()
        {
            var schema = new SchemaExport(config);
            schema.Create(true, true);    //TODO: don't know why the two parameter must be true?
        }

        static void UpdateSchema()
        {
            string path = Environment.CurrentDirectory.Substring(0, Environment.CurrentDirectory.IndexOf("bin")) + "Update\\sqlscript.txt";
            using (var file = new FileStream(path, FileMode.Create, FileAccess.ReadWrite))
            {
                using (var sw = new StreamWriter(file))
                {
                    new SchemaUpdate(config)
                        .Execute(sw.Write, false);
                }
            };
        }

        static void CreateDatabase()
        {
            new DAL.ADO.BuildDatabase().Create("task_dev");
        }
    }
}
