using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Message
{
    public class ToMeModel
    {
        public IList<ToMeItemModel> Messages { get; set; }

        public IList<_LiteralLinkedModel> Projects { get; set; }
        public int? SelectedProjectId { get; set; }

        public IList<UserModel> Addressers { get; set; }
        public int? SelectedAddresserId { get; set; }

        public PagerModel Pager { get; set; }

        public MessageMark MessageMark { get; set; }
    }
}
