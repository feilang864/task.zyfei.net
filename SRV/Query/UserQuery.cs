using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.SRV.Query
{
    public class UserQuery
    {
        private IQueryable<User> _querySource;
        public UserQuery(IQueryable<User> querySource)
        {
            _querySource = querySource;
        }

        public IQueryable<User> getByName(string name)
        {
            return _querySource
                .Where(u => u.Name == name);
        }
    }
}
