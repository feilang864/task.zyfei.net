using FFLTask.BLL.Entity;

namespace FFLTask.BLL.NHMap
{
    class MessageMap : EntityMap<Message>
    {
        public MessageMap()
        {
            Map(m => m.Content).Length(Const.Length_Text);
            References(m => m.Task).ForeignKey("FK_Message_Task");
            References(m => m.Project).ForeignKey("FK_Message_Project");
            References(m => m.Addresser).ForeignKey("FK_Message_Addresser");
            References(m => m.Addressee).ForeignKey("FK_Message_Addressee");
            Map(m => m.ReadTime);
            Map(m => m.HideForAddresser);
            Map(m => m.HideForAddressee);
        }
    }
}
