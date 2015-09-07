using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Task;
using Global.Core.ExtensionMethod;
using Global.Core.Helper;

namespace FFLTask.SRV.ViewModelMap
{
    public static class TaskMap
    {
        public static void FilledBy(this FullItemModel model, Task task)
        {
            model.LiteItem = new LiteItemModel();
            model.LiteItem.FilledBy(task);

            model.ActualComplete = task.ActualCompleteTime;
            model.ActualWorkPeriod = task.ActualWorkPeriod;
            model.Assign = task.AssignTime;

            //TODO: is this check necessary?
            if (task.Publisher != null)
            {
                model.Publisher = new UserModel();
                model.Publisher.FilledBy(task.Publisher);
            }
            //TODO: walkaround
            if (task.Accepter != null)
            {
                model.Accepter = new UserModel();
                model.Accepter.FilledBy(task.Accepter);
            }

            model.Created = task.CreateTime;
            model.Difficulty = task.Difficulty;

            model.ExpectedComplete = task.ExpectCompleteTime.Split();
            model.ExpectedWorkPeriod = task.ExpectWorkPeriod;
            model.LatestUpdate = task.LatestUpdateTime;
            model.OverDue = task.OverDue;
            model.Delay = task.Delay;
            model.Own = task.OwnTime;

            if (task.Owner != null)
            {
                model.Owner = new UserModel();
                model.Owner.FilledBy(task.Owner);
            }

            model.Priority = task.Priority;
            model.Quality = task.Quality;

            model.Body = task.Body;
            model.HasAccepted = task.HasAccepted;
        }

        public static void FilledBy(this LiteItemModel model, Task task)
        {
            model.Id = task.Id;
            model.Sequence = task.Sequence;
            model.Title = task.Title;
            model.CurrentStatus = new StatusModel();
            model.CurrentStatus.FilledBy(task.CurrentStatus);
            model.Virtual = task.IsVirtual;
            model.HasChild = !task.Children.IsNullOrEmpty();
            model.NodeType = task.NodeType;
        }

        public static void FilledBy(this EditModel model, Task task)
        {
            model.TaskItem = new FullItemModel();
            model.TaskItem.FilledBy(task);


            if (task.Parent != null)
            {
                model.TaskItem.Parent = new LiteItemModel();
                model.TaskItem.Parent.FilledBy(task.Parent);
            }

            model.ExpectedComplete = task.ExpectCompleteTime.Split();
            model.TaskItem.HasAccepted = task.HasAccepted;

            if (task.GetPrevious() != null)
            {
                model.PreviousTaskId = task.GetPrevious().Id;
            }
            if (task.GetNext() != null)
            {
                model.NextTaskId = task.GetNext().Id;
            }
        }

        public static void Fill(this FullItemModel model, Task task)
        {
            task.Title = model.LiteItem.Title;
            task.Body = model.Body;
            //TODO:
            //task.Body = model.HtmlTrim().FixTags();
            task.IsVirtual = model.LiteItem.Virtual;
            task.Priority = model.Priority;
            task.Difficulty = model.Difficulty;
            task.ExpectCompleteTime = model.ExpectedComplete.Combine();
            task.ExpectWorkPeriod = model.ExpectedWorkPeriod;

            task.LatestUpdateTime = SystemTime.Now();
        }

        public static void FilledBy(this TaskRelationModel model, Task task)
        {
            model.Ancestor = new LinkedList<LiteItemModel>();
            model.Children = new List<LiteItemModel>();
            model.Brothers = new List<LiteItemModel>();
            model.Current = new LiteItemModel();

            model.Current.FilledBy(task);

            if (task.Parent != null)
            {
                model.Ancestor.filledByTail(task.Parent);

                IList<Task> brothers = task.Parent.Children.Where(x => x != task).ToList();
                model.Brothers.filledBy(brothers);
            }
            if (task.Children != null)
            {
                model.Children.filledBy(task.Children);
            }
        }

        private static void filledByTail(this  LinkedList<LiteItemModel> models, Task tail)
        {
            while (tail.Parent != null)
            {
                LiteItemModel item = new LiteItemModel();
                item.FilledBy(tail);
                models.AddFirst(item);
                tail = tail.Parent;
            }
            LiteItemModel first = new LiteItemModel();
            first.FilledBy(tail);
            models.AddFirst(first);
        }

        private static void filledBy(this IList<LiteItemModel> models, IList<Task> tasks)
        {
            foreach (Task task in tasks)
            {
                LiteItemModel model = new LiteItemModel();
                model.FilledBy(task);
                models.Add(model);
            }
        }

        public static void FilledBy(this AttachmentModel model, Attachment attachment)
        {
            model.FileName = attachment.FileName;
            model.Uploader = new UserModel();
            model.Uploader.FilledBy(attachment.Uploader);
        }
    }
}

