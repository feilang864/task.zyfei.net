using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap
{
    class ProjectMap : EntityMap<Project>
    {
        public ProjectMap()
        {
            Map(m => m.Name).Index("IX_Name");
            Map(m => m.Description).Length(Const.Length_Text);
            HasMany(m => m.Authorizations).KeyColumn("Project_Id").Inverse().Cache.ReadWrite();
            Component(m => m.Config);
            References(m => m.Parent).ForeignKey("FK_Project_Parent");
            HasMany(m => m.Children).KeyColumn("Parent_Id").Inverse().Cache.ReadWrite();

            Cache.ReadWrite();
        }
    }
}
