using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModel.Task
{
    public class TaskHistoryItemModel
    {
        public DateTime CreateTime { get; set; }
        public UserModel Executor { get; set; }
        public string Description { get; set; }
        public string Comment { get; set; }
    }
}
