
namespace FFLTask.BLL.Entity
{
    public class Attachment : BaseEntity
    {
        public virtual Task Task { get; set; }
        public virtual User Uploader { get; set; }
        public virtual string FileName { get; set; }
    }
}
