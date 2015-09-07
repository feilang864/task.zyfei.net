using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap.Account
{
    class AuthorizationMap : EntityMap<Authorization>
    {
        public AuthorizationMap()
        {
            References(m => m.User).ForeignKey("FK_Auth_User");
            References(m => m.Project).ForeignKey("FK_Auth_Project");
            Map(m => m.IsFounder);
            Map(m => m.IsPublisher);
            Map(m => m.IsOwner);
            Map(m => m.IsAdmin);

            Cache.ReadWrite();
        }
    }
}
