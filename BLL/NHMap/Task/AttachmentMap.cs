
namespace FFLTask.BLL.NHMap.Task
{
    class AttachmentMap : EntityMap<BLL.Entity.Attachment>
    {
        public AttachmentMap()
        {
            References(m => m.Task).ForeignKey("FK_Attachment_Task");
            References(m => m.Uploader).ForeignKey("FK_Attachment_Uploader");
            Map(x => x.FileName);
        }
    }
}
