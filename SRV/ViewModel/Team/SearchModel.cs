using System.Collections.Generic;
using System.ComponentModel;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Validations;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Team
{
    public class SearchModel
    {
        [FflRequired]
        [DisplayName("用户名")]
        public string UserName { get; set; }
        public int UserId { get; set; }
        
        public ResultFor ResultFor { get; set; }
        
        public TransferSearchResultModel TransferResult { get; set; }
        public DismissSearchResultModel DismissResult { get; set; }
    }

    public enum ResultFor
    {
        Transfer,
        Dismiss
    }

    public class DismissSearchResultModel
    {
        public IList<DismissSearchResultItemModel> Items { get; set; }
        public PagerModel Pager { get; set; }
    }

    public class DismissSearchResultItemModel
    {
        public int ProjectId { get; set; }
        public int Charge { get; set; }
        public bool Selected { get; set; }
        public bool Dismissed { get; set; }
    }

    public class TransferSearchResultModel
    {
        public IEnumerable<TransferSearchResultItemModel> AsPublisher { get; set; }
        public IEnumerable<TransferSearchResultItemModel> AsOwner { get; set; }
        public IEnumerable<TransferSearchResultItemModel> AsAccepter { get; set; }
        public PagerModel Pager { get; set; } 
    }

    public class TransferSearchResultItemModel
    {
        public int ProjectId { get; set; }
        public int Total { get; set; }
        public int InCompleted { get; set; }
        public Role Role { get; set; }
    }


}
