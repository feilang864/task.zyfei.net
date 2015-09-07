using System;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Task;

namespace FFLTask.SRV.ViewModel.Message
{
    public class FromMeItemModel
    {
        public int Id { get; set; }
        public DateTime PublishTime { get; set; }

        public Project._LiteralLinkedModel Project { get; set; }
        public LiteItemModel Task { get; set; }
        public DateTime? ReadTime { get; set; }
        public UserModel Addressee { get; set; }

        public string Content { get; set; }

        public bool Checked { get; set; }
    }
}
