using System.Collections.Generic;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Message
{
    public class FromMeModel
    {
        public IList<FromMeItemModel> Messages { get; set; }

        public IList<_LiteralLinkedModel> Projects { get; set; }
        public int? SelectedProjectId { get; set; }

        public IList<UserModel> Addressees { get; set; }
        public int? SelectedAddresseeId { get; set; }

        public PagerModel Pager { get; set; }
    }
}
