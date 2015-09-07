using System;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Task
{
    public class FullItemModel
    {
        public string Body { get; set; }
        public LiteItemModel LiteItem { get; set; }

        public FFLTask.SRV.ViewModel.Project.FullItemModel Project { get; set; }
        public LiteItemModel Parent { get; set; }

        public TaskPriority? Priority { get; set; }

        public TaskDifficulty? Difficulty { get; set; }

        public int? ExpectedWorkPeriod { get; set; }
        public int? ActualWorkPeriod { get; set; }

        public SplitDateTimeModel ExpectedComplete { get; set; }
        public DateTime? ActualComplete { get; set; }

        public DateTime Created { get; set; }
        public DateTime? Assign { get; set; }
        public DateTime? Own { get; set; }
        public DateTime LatestUpdate { get; set; }
        public int? OverDue { get; set; }
        public int? Delay { get; set; }
        public UserModel Publisher { get; set; }
        public UserModel Owner { get; set; }
        public UserModel Accepter { get; set; }
        public TaskQuality? Quality { get; set; }
        public bool HasAccepted { get; set; }
    }
}
