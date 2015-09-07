using System.Collections.Generic;
using System.Web.Mvc;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Task
{
    public class ListModel
    {
        public IList<FullItemModel> Items { get; set; }

        //TODO: need complete all the filter condition here
        public int? TaskId { get; set; }

        public int? GreaterOverDue { get; set; }
        public int? LessOverDue { get; set; }
        public bool AbsolutOrPercentage { get; set; }

        public int? GreaterWorkPeriod { get; set; }
        public int? LessWorkPeriod { get; set; }

        public _TimeSpanModel TimeSpan { get; set; }

        public _DropdownlistLinkedModel CurrentProject { get; set; }

        public IList<UserModel> Publishers { get; set; }
        public int? SelectedPublisherId { get; set; }

        public IList<TaskPriority> AllPriorities { get; set; }
        public TaskPriority? SelectedPriority { get; set; }

        public IList<TaskDifficulty> AllDifficulties { get; set; }
        public TaskDifficulty? SelectedDifficulty { get; set; }

        public IList<StatusModel> AllStatus { get; set; }
        public IList<int> SelectedStages { get; set; }
        public int? SelectedStage { get; set; }

        public IList<TaskQuality> AllQualities { get; set; }
        public TaskQuality? SelectedQuality { get; set; }

        public IList<UserModel> Owners { get; set; }
        public int? SelectedOwnerId { get; set; }

        public IList<UserModel> Accepters { get; set; }
        public int? SelectedAccepterId { get; set; }

        public Dictionary<string, NodeType> NodeTypes { get; set; }
        public NodeType? SelectedNodeType { get; set; }

        public PagerModel Pager { get; set; }

        public int? PageIndex { get; set; }

        public bool CanOwn { get; set; }
        public bool SyncRefresh { get; set; }
        public bool SetShownColoumns { get; set; }

        public IList<SelectListItem> ShowColumns { get; set; }
    }

    //TODO: should be used into ListModel
    public class FilterModel
    {
        public NodeType? SelectedNodeType { get; set; }

        public int? SelectedPublisherId { get; set; }
        public int? SelectedOwnerId { get; set; }
        public int? SelectedAccepterId { get; set; }

        public TaskPriority? SelectedPriority { get; set; }
        public TaskDifficulty? SelectedDifficulty { get; set; }

        public IList<int> SelectedStages { get; set; }
        public int? SelectedStage { get; set; }

        #region still not used

        public int? GreaterOverDue { get; set; }
        public int? LessOverDue { get; set; }
        public bool AbsolutOrPercentage { get; set; }

        public int? GreaterWorkPeriod { get; set; }
        public int? LessWorkPeriod { get; set; }

        public bool CanOwn { get; set; }
        public bool SyncRefresh { get; set; }
        public bool SetShownColoumns { get; set; }

        public TaskQuality? SelectedQuality { get; set; }

        #endregion

    }
}
