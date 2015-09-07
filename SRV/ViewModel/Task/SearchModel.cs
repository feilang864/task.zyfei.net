using System;
using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModel.Task
{
    public class SearchModel
    {
        public IList<FullItemModel> Projects { get; set; }
        public int? SelectedProjectId { get; set; }

        public int? TaskNumber { get; set; }

        public IList<UserModel> Publisher { get; set; }
        public int? SelectedPublishId { get; set; }

        public IList<UserModel> Owners { get; set; }
        public int? SelecteOwnerId { get; set; }

        public DateTime? StartPublishDate { get; set; }
        public DateTime? EndPublishDate { get; set; }
        public DateTime? StartAssignerDate { get; set; }
        public DateTime? EndAssignerDate { get; set; }
        public DateTime? StartExpectCompleteDate { get; set; }
        public DateTime? EndExpectCompleteDate { get; set; }
        public DateTime? StartActaulCompleteDate { get; set; }
        public DateTime? EndActualCompleteDate { get; set; }

        public IList<TaskPriority> AllPriotity { get; set; }
        public TaskPriority? Priority { get; set; }

        public int? GreaterOverDue { get; set; }
        public int? LessOverDue { get; set; }
        public DateTime? StartLastUpdateTime { get; set; }
        public DateTime? EndLastUpdateTime { get; set; }
    }
}
