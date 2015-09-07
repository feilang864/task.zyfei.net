using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MySql.Data.MySqlClient;

namespace FFLTask.DAL.ADO
{
    class Helper
    {
        private string _connStr;
        public Helper(string connStr)
        {
            _connStr = connStr;
        }

        internal void executeNonQuery(string commandStr)
        {
            using (MySqlConnection connection = new MySqlConnection(_connStr))
            {
                connection.Open();
                MySqlCommand command = new MySqlCommand(commandStr, connection);
                command.ExecuteNonQuery();
            }
        }

    }
}
