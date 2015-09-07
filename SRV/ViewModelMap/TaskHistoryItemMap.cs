using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModelMap
{
    public static class TaskHistoryItemMap
    {
        public static TaskHistoryItemModel FilledBy(this TaskHistoryItemModel model, HistoryItem item)
        {
            model.CreateTime = item.CreateTime;

            UserModel executor = new UserModel();
            executor.FilledBy(item.Executor);
            model.Executor = executor;
            model.Comment = item.Comment;
            model.Description = item.Description;

            return model;
        }
    }
}
