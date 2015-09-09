using FFLTask.BLL.Entity;
using FluentNHibernate.Mapping;

namespace FFLTask.BLL.NHMap.Account
{
    class ProfileMap : ComponentMap<Profile>
    {
        public ProfileMap()
        {
            Map(m => m.Birthday);
            Map(m => m.City);
            Map(m => m.Female);
            Map(m => m.Greet);
            Map(m => m.Interested);
            Map(m => m.OtherContact);
            Map(m => m.Phone);
            Map(m => m.Province);
            Map(m => m.QQ);
            Map(m => m.SelfDescription).Length(Const.Length_Text);
        }
    }
}
