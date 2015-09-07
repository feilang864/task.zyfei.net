using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.GLB.Global.UrlParameter;
using FFLTask.SRV.ViewModel.Task;

namespace FFLTask.SRV.Query
{
    public static class TaskQuery
    {
        public static IQueryable<Task> AsAccepter(this IQueryable<Task> source, int userId)
        {
            return source.Where(t => t.Accepter.Id == userId);
        }

        public static IQueryable<Task> AsOwner(this IQueryable<Task> source, int userId)
        {
            return source.Where(t => t.Owner.Id == userId);
        }

        public static IQueryable<Task> AsPublisher(this IQueryable<Task> source, int userId)
        {
            return source.Where(t => t.Publisher.Id == userId);
        }

        public static IQueryable<Task> ChargedBy(this IQueryable<Task> source, int userId)
        {
            return source.Where(t => t.Publisher.Id == userId ||
                t.Owner.Id == userId ||
                t.Accepter.Id == userId
            );
        }

        //TODO: jsut a walkaround, need refactor: see http://task.zyfei.net/Task/Edit/2386
        public static IQueryable<Task> Get(this IQueryable<Task> source, ListModel model)
        {
            return source.Get(model, new List<int> { model.CurrentProject.TailSelectedProject.Id });
        }

        //TODO: should make use of InFilter()
        public static IQueryable<Task> Get(this IQueryable<Task> source, ListModel model, IList<int> projectIds)
        {
            IQueryable<Task> items = source.GetInProject(projectIds);

            if (model.GreaterOverDue.HasValue)
            {
                items = items.Where(t => t.OverDue > model.GreaterOverDue);
            }
            if (model.LessOverDue.HasValue)
            {
                items = items.Where(t => t.OverDue < model.LessOverDue);
            }
            if (model.GreaterWorkPeriod.HasValue)
            {
                items = items.Where(t => t.ActualWorkPeriod > model.GreaterWorkPeriod);
            }
            if (model.LessWorkPeriod.HasValue)
            {
                items = items.Where(t => t.ActualWorkPeriod < model.LessWorkPeriod);
            }

            #region TimeSpan related, should be abstracted

            if (model.TimeSpan.FromPublish.HasValue)
            {
                items = items.Where(t => t.CreateTime > model.TimeSpan.FromPublish);
            }
            if (model.TimeSpan.ToPublish.HasValue)
            {
                items = items.Where(t => t.CreateTime < model.TimeSpan.ToPublish);
            }
            if (model.TimeSpan.FromAssign.HasValue)
            {
                items = items.Where(t => t.AssignTime > model.TimeSpan.FromAssign);
            }
            if (model.TimeSpan.ToAssign.HasValue)
            {
                items = items.Where(t => t.AssignTime < model.TimeSpan.ToAssign);
            }
            if (model.TimeSpan.FromOwn.HasValue)
            {
                items = items.Where(t => t.OwnTime > model.TimeSpan.FromOwn);
            }
            if (model.TimeSpan.ToOwn.HasValue)
            {
                items = items.Where(t => t.OwnTime < model.TimeSpan.ToOwn);
            }
            if (model.TimeSpan.FromExpectComplete.HasValue)
            {
                items = items.Where(t => t.ExpectCompleteTime > model.TimeSpan.FromExpectComplete);
            }
            if (model.TimeSpan.ToExpectComplete.HasValue)
            {
                items = items.Where(t => t.ExpectCompleteTime < model.TimeSpan.ToExpectComplete);
            }
            if (model.TimeSpan.FromActualComplete.HasValue)
            {
                items = items.Where(t => t.ActualCompleteTime > model.TimeSpan.FromActualComplete);
            }
            if (model.TimeSpan.ToActualComplete.HasValue)
            {
                items = items.Where(t => t.ActualCompleteTime < model.TimeSpan.ToActualComplete);
            }
            if (model.TimeSpan.FromLastestUpdateTime.HasValue)
            {
                items = items.Where(t => t.LatestUpdateTime > model.TimeSpan.FromLastestUpdateTime);
            }
            if (model.TimeSpan.ToLastestUpdateTime.HasValue)
            {
                items = items.Where(t => t.LatestUpdateTime < model.TimeSpan.ToLastestUpdateTime);
            }

            #endregion

            if (model.SelectedPriority.HasValue)
            {
                items = items.Where(t => t.Priority == (TaskPriority)model.SelectedPriority);
            }
            if (model.SelectedDifficulty.HasValue)
            {
                items = items.Where(t => t.Difficulty == model.SelectedDifficulty);
            }
            if (model.SelectedOwnerId.HasValue)
            {
                items = items.Where(t => t.Owner.Id == model.SelectedOwnerId);
            }
            if (model.SelectedAccepterId.HasValue)
            {
                items = items.Where(t => t.Accepter.Id == model.SelectedAccepterId);
            }
            if (model.SelectedPublisherId.HasValue)
            {
                items = items.Where(t => t.Publisher.Id == model.SelectedPublisherId);
            }
            if (model.SelectedStages != null)
            {
                items = items.Where(t => model.SelectedStages.Contains((int)t.CurrentStatus));
            }
            else
            {
                if (model.SelectedStage.HasValue)
                {
                    items = items.Where(t => t.CurrentStatus == (Status)model.SelectedStage);
                }
            }
            if (model.SelectedQuality.HasValue)
            {
                items = items.Where(t => t.Quality == model.SelectedQuality);
            }
            if (model.SelectedNodeType.HasValue)
            {
                if (model.SelectedNodeType == NodeType.Leaf)
                {
                    items = items.Where(t => t.Parent != null && t.Children.Count == 0);
                }
                else if (model.SelectedNodeType == NodeType.Branch)
                {
                    items = items.Where(t => t.Parent != null && t.Children.Count > 0);
                }
                else if (model.SelectedNodeType == NodeType.Root)
                {
                    items = items.Where(t => t.Parent == null);
                }
            }

            return items;
        }

        public static IQueryable<Task> GetStartWith(this IQueryable<Task> source, string value)
        {
            return source.Where(t => t.Title.StartsWith(value));
        }
        
        public static IQueryable<Task> GetInProject(this IQueryable<Task> source, int projectId)
        {
            return source.Where(t => t.Project.Id == projectId);
        }

        public static IQueryable<Task> GetInProject(this IQueryable<Task> source, IList<int> projectIds)
        {
            return source.Where(t => projectIds.Contains(t.Project.Id));
        }

        public static IQueryable<Task> InFilter(this IQueryable<Task> items, FilterModel model)
        {
            #region still not used

            if (model.GreaterOverDue.HasValue)
            {
                items = items.Where(t => t.OverDue > model.GreaterOverDue);
            }
            if (model.LessOverDue.HasValue)
            {
                items = items.Where(t => t.OverDue < model.LessOverDue);
            }
            if (model.GreaterWorkPeriod.HasValue)
            {
                items = items.Where(t => t.ActualWorkPeriod > model.GreaterWorkPeriod);
            }
            if (model.LessWorkPeriod.HasValue)
            {
                items = items.Where(t => t.ActualWorkPeriod < model.LessWorkPeriod);
            }

            #endregion

            if (model.SelectedPriority.HasValue)
            {
                items = items.Where(t => t.Priority == (TaskPriority)model.SelectedPriority);
            }
            if (model.SelectedDifficulty.HasValue)
            {
                items = items.Where(t => t.Difficulty == model.SelectedDifficulty);
            }
            if (model.SelectedOwnerId.HasValue)
            {
                items = items.Where(t => t.Owner.Id == model.SelectedOwnerId);
            }
            if (model.SelectedAccepterId.HasValue)
            {
                items = items.Where(t => t.Accepter.Id == model.SelectedAccepterId);
            }
            if (model.SelectedPublisherId.HasValue)
            {
                items = items.Where(t => t.Publisher.Id == model.SelectedPublisherId);
            }
            if (model.SelectedStages != null)
            {
                items = items.Where(t => model.SelectedStages.Contains((int)t.CurrentStatus));
            }
            else
            {
                if (model.SelectedStage.HasValue)
                {
                    items = items.Where(t => t.CurrentStatus == (Status)model.SelectedStage);
                }
            }
            if (model.SelectedQuality.HasValue)
            {
                items = items.Where(t => t.Quality == model.SelectedQuality);
            }
            if (model.SelectedNodeType.HasValue)
            {
                if (model.SelectedNodeType == NodeType.Leaf)
                {
                    items = items.Where(t => t.Parent != null && t.Children.Count == 0);
                }
                else if (model.SelectedNodeType == NodeType.Branch)
                {
                    items = items.Where(t => t.Parent != null && t.Children.Count > 0);
                }
                else if (model.SelectedNodeType == NodeType.Root)
                {
                    items = items.Where(t => t.Parent == null);
                }
            }

            return items;
        }

        public static IQueryable<Task> Sort(this IQueryable<Task> source, string sort, bool des)
        {
            if (sort == TaskList.Sort_By_ActualComplete)
            {
                source = source.OrderBy(t => t.ActualCompleteTime);
            }
            else if (sort == TaskList.Sort_By_ExpectedComplete)
            {
                source = source.OrderBy(t => t.ExpectCompleteTime);
            }
            else if (sort == TaskList.Sort_By_ActualWorkPeriod)
            {
                source = source.OrderBy(t => t.ActualWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_ExpectedWorkPeriod)
            {
                source = source.OrderBy(t => t.ExpectWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_Created)
            {
                source = source.OrderBy(t => t.CreateTime);
            }
            else if (sort == TaskList.Sort_By_Own)
            {
                source = source.OrderBy(t => t.OwnTime);
            }
            else if (sort == TaskList.Sort_By_Assign)
            {
                source = source.OrderBy(t => t.AssignTime);
            }
            else if (sort == TaskList.Sort_By_LatestUpdate)
            {
                source = source.OrderBy(t => t.LatestUpdateTime);
            }
            else if (sort == TaskList.Sort_By_OverDue)
            {
                source = source.OrderBy(t => t.OverDue);
            }
            else if (sort == TaskList.Sort_By_Delay)
            {
                source = source.OrderBy(t => t.Delay);
            }
            else if (sort == TaskList.Sort_By_Priority)
            {
                source = source.OrderBy(t => t.Priority);
            }
            else
            {
                throw new Exception(string.Format("the sort parameter ({0}) is not correct.", sort));
            }

            if (des)
            {
                source = source.AsEnumerable().Reverse().AsQueryable();
            }

            return source;
        }
    }
}
