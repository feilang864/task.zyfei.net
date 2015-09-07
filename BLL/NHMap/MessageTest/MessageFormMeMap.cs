using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap
{
    class MessageFormMeMap : EntityMap<MessageFormMe>
    {
        public MessageFormMeMap()
        {
            Map(m => m.Content);
            References(m => m.Task).ForeignKey("FK_MessageFormMe_Task");
            References(m => m.Project).ForeignKey("FK_MessageFormMe_Project");
            References(m => m.Addressee).ForeignKey("FK_MessageFormMe_Addressee");
            Map(m => m.ReadTime);
        }
    }
}
