using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.SRV.Query
{
    public class ProjectQuery
    {
        private IQueryable<Project> _querySource;
        public ProjectQuery(IQueryable<Project> querySource)
        {
            _querySource = querySource;
        }

        public IQueryable<Project> GetByName(string name)
        {
            return _querySource
                .Where(u => u.Name == name);
        }

        public IQueryable<Project> GetByPartialName(string name)
        {
            return _querySource
                .Where(u => u.Name.Contains(name));
        }
    }
}
