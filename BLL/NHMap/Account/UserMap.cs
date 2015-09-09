using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap.Account
{
    class UserMap : EntityMap<User>
    {
        public UserMap()
        {
            Map(m => m.Name).Index("IX_Name");
            Map(m => m.Password);
            Component(m => m.Profile);
            Map(m => m.RealName);
            Map(m => m.AuthenticationCode);
            HasMany(m => m.Authorizations).KeyColumn("User_id").Inverse()
                .Cache.ReadWrite();
            HasMany(m => m.MessagesToMe).KeyColumn("Addressee_id").Inverse()
                .ExtraLazyLoad();
            HasMany(m => m.MessagesFromMe).KeyColumn("Addresser_id").Inverse()
                .ExtraLazyLoad();

            Cache.ReadWrite();
        }
    }
}
