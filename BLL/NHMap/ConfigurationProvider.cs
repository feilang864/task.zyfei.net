using System;
using FFLTask.BLL.NHMap.Account;
using FluentNHibernate.Cfg;
using FluentNHibernate.Conventions.Helpers;

namespace FFLTask.BLL.NHMap
{
    public class ConfigurationProvider
    {
        public static Action<MappingConfiguration> Action
        {
            get
            {
                return m => m.FluentMappings.AddFromAssemblyOf<UserMap>()
                    .Conventions.Add(DefaultCascade.SaveUpdate());
            }
        }
    }
}
