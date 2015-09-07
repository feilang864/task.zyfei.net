using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.ViewModel.Task
{
    public class NewModel
    {
        public FullItemModel TaskItem { get; set; }
        public _DropdownlistLinkedModel CurrentProject { get; set; }
        public IList<TaskDifficulty> AllDifficulties { get; set; }
        public IList<UserModel> Owners { get; set; }
        public IList<UserModel> Accepters { get; set; }
        public IList<TaskPriority> AllPriorities { get; set; }
        public IList<string> Attachments { get; set; }

        public SearchBy IdOrName { get; set; }
        public IList<LiteItemModel> QualifiedResult { get; set; }

        public RedirectPage Redirect { get; set; }
    }
}

