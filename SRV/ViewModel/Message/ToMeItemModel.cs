using System;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Task;

namespace FFLTask.SRV.ViewModel.Message
{
    public class ToMeItemModel
    {
        public int Id { get; set; }
        public DateTime PublishTime { get; set; }
        public DateTime? ReadTime { get; set; }

        public Project._LiteralLinkedModel Project { get; set; }
        public LiteItemModel Task { get; set; }
        public UserModel Addresser { get; set; }

        public string Content { get; set; }

        public bool Checked { get; set; }
    }
}
