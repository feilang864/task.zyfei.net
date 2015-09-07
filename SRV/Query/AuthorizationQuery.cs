using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;

namespace FFLTask.SRV.Query
{
    public static class AuthorizationQuery
    {
        public static IQueryable<Authorization> InProjects(
            this IQueryable<Authorization> querySource, IList<int> projectIds)
        {
            return querySource.Where(a => projectIds.Contains(a.Project.Id));
        }

        public static IQueryable<Authorization> WithUser(
            this IQueryable<Authorization> querySource, int userId)
        {
            return querySource.Where(a => a.User.Id == userId);
        }
    }
}
