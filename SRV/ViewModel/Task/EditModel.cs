using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Task
{
    public class EditModel
    {
        public FullItemModel TaskItem { get; set; }

        public int ProjectId { get; set; }
        public _DropdownlistLinkedModel CurrentProject { get; set; }

        public UserModel CurrentUser { get; set; }
        public bool CanOwn { get; set; }
        public bool Own { get; set; }
        public SplitDateTimeModel ExpectedComplete { get; set; }

        public IList<UserModel> Accepters { get; set; }
        public IList<TaskQuality> AllQualities { get; set; }
        public IList<TaskPriority> AllPriorities { get; set; }
        public IList<TaskDifficulty> AllDifficulties { get; set; }
        public IList<UserModel> Owners { get; set; }
        public IList<StatusModel> QualifiedStatus { get; set; }
        public StatusModel SelectedQualifiedStatus { get; set; }

        public string Comment { get; set; }
        public IList<AttachmentModel> Attachments { get; set; }

        public RedirectPage Redirect { get; set; }

        public int? PreviousTaskId { get; set; }
        public int? NextTaskId { get; set; }

        public bool CanAutoCompleteParent { get; set; }
        public bool AutoCompleteParent { get; set; }
        public bool CanAutoAccepterParent { get; set; }
        public bool AutoAcceptParent { get; set; }

        public int? AddresseeId { get; set; }

        public SearchBy IdOrName { get; set; }
        public IList<LiteItemModel> QualifiedResult { get; set; }
    }

    #region //TODO: used later: http://task.zyfei.net/Task/Edit/2423

    public class _PublishModel 
    {
        public int ProjectId { get; set; }
        public _DropdownlistLinkedModel CurrentProject { get; set; }

        public FullItemModel TaskItem { get; set; }


        public int? SelectedPriority { get; set; }
        public IList<TaskPriority> AllPriorities { get; set; }

        public int? SelectedDifficulty { get; set; }
        public IList<TaskDifficulty> AllDifficulties { get; set; }



        public SplitDateTimeModel ExpectedComplete { get; set; }

        public IList<UserModel> Accepters { get; set; }
        public IList<AttachmentModel> Attachments { get; set; }
    }

    public class _AssignOwnModel
    {
        public bool CanOwn { get; set; }
        public bool Own { get; set; }
        public IList<UserModel> Owners { get; set; }

    }

    public class _ProgressModel
    {
        public IList<StatusModel> QualifiedStatus { get; set; }
        public StatusModel SelectedQualifiedStatus { get; set; }

        public bool CanAutoCompleteParent { get; set; }
        public bool AutoCompleteParent { get; set; }
    }

    public class _AcceptModel
    {
        public IList<TaskQuality> AllQualities { get; set; }
        public bool CanAutoAccepterParent { get; set; }
        public bool AutoAcceptParent { get; set; }
    }

    public class _CommmentModel
    {
        public string Comment { get; set; }
        public int? AddresseeId { get; set; }
    }

    #endregion

}
