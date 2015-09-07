using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MySql.Data.MySqlClient;

namespace FFLTask.DAL.ADO
{
    public class BuildDatabase
    {
        private string _connStr;

        /// <summary>
        /// the connection string is fixed since it's only for development 
        /// </summary>
        public BuildDatabase()
        {
            _connStr = "server=localhost;Uid=root;";
        }

        public void Create(string dbName)
        {
            var helper = new Helper(_connStr);
            helper.executeNonQuery("DROP DATABASE IF EXISTS " + dbName);
            
            string create = string.Format("CREATE DATABASE {0} default charset utf8 COLLATE utf8_general_ci", dbName);
            helper.executeNonQuery(create);
        }

    }
}