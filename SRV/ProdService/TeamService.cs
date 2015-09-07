using AutoMapper;
using FFLTask.BLL.Entity;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.Query;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Team;
using NHibernate;
using NHibernate.Linq;
using NHibernate.Transform;
using System;
using System.Collections.Generic;
using System.Linq;

namespace FFLTask.SRV.ProdService
{
    public class TeamService : BaseService, ITeamService
    {
        private IQueryable<Task> _taskQuery;
        public TeamService()
        {
            _taskQuery = session.Query<Task>();
        }

        public SearchModel GroupedByRole(int userId)
        {
            SearchModel model = new SearchModel
            {
                TransferResult = new TransferSearchResultModel()
            };

            var groupedAsAccepters = groupedInRole(userId, Role.Accepter).ToList();
            groupedAsAccepters.ForEach(m => m.Role = Role.Accepter);
            model.TransferResult.AsAccepter = groupedAsAccepters;

            var groupedAsOwner = groupedInRole(userId, Role.Owner).ToList();
            groupedAsOwner.ForEach(m => m.Role = Role.Owner);
            model.TransferResult.AsOwner = groupedAsOwner;

            var groupedAsPublisher = groupedInRole(userId, Role.Publisher).ToList();
            groupedAsPublisher.ForEach(m => m.Role = Role.Publisher);
            model.TransferResult.AsPublisher = groupedAsPublisher;

            return model;
        }

        public IList<DismissSearchResultItemModel> GroupedByProject(int userId)
        {
            string queryShell = "SELECT" +
                " cast(Project_id as SIGNED) AS ProjectId," +
                " cast(SUM(CASE" +
                        " WHEN CurrentStatus <> 'Accept' AND CurrentStatus <> 'Remove'" +
                        " THEN 1 ELSE 0" +
                     " END) as SIGNED) AS Charge" +
                " FROM task WHERE " +
                " Publisher_Id=" + userId +
                " OR Owner_Id=" + userId +
                " OR Accepter_Id=" + userId +
                " GROUP BY Project_id";

            var result = session.CreateSQLQuery(queryShell)
                .AddScalar("ProjectId", NHibernateUtil.Int32)
                .AddScalar("Charge", NHibernateUtil.Int32)
                .SetResultTransformer(
                    Transformers.AliasToBean<DismissSearchResultItemModel>())
                .List<DismissSearchResultItemModel>();

            return setDismissed(result, userId);
        }

        private IList<DismissSearchResultItemModel> setDismissed(
            IList<DismissSearchResultItemModel> projectGroupedInfo, int userId)
        {
            IList<int> projectIds = projectGroupedInfo.Select(p => p.ProjectId).ToList();

            var auths = session.Query<Authorization>()
                .WithUser(userId)
                .InProjects(projectIds)
                ;

            for (int i = 0; i < projectGroupedInfo.Count; i++)
            {
                projectGroupedInfo[i].Dismissed =
                    !auths.Any(a =>
                        a.User.Id == userId &&
                        a.Project.Id == projectGroupedInfo[i].ProjectId);
            }

            return projectGroupedInfo;
        }

        public void HandOver(TransferItemModel model,
            Role role, int succesorId, int operaterId)
        {
            Task task = session.Load<Task>(model.Id);
            User successor = session.Load<User>(succesorId);
            User operater = session.Load<User>(operaterId);

            switch (role)
            {
                case Role.Publisher:
                    task.ChangePublisher(operater, successor);
                    break;
                case Role.Owner:
                    task.ChangeOwner(operater, successor);
                    break;
                case Role.Accepter:
                    task.ChangeAccepter(operater, successor);
                    break;
            }

        }

        public IList<TransferItemModel> GetTasks(TransferModel transferModel)
        {
            IQueryable<Task> tasks = get(transferModel);

            IList<TransferItemModel> models = null;
            return Mapper.Map(tasks.ToList(), models);
        }

        public IList<Status?> GetAllStatus(TransferModel transferModel)
        {
            IQueryable<Task> tasks = get(transferModel);
            return tasks.GroupBy(t => t.CurrentStatus)
                .Select(g => g.Key)
                .Cast<Status?>()
                .ToList()
                ;
        }

        private IQueryable<Task> get(TransferModel transferModel)
        {
            IQueryable<Task> tasks = null;
            switch (transferModel.CurrentRole)
            {
                case Role.Publisher:
                    tasks = _taskQuery.AsPublisher(transferModel.LeaverId);
                    break;
                case Role.Owner:
                    tasks = _taskQuery.AsOwner(transferModel.LeaverId);
                    break;
                case Role.Accepter:
                    tasks = _taskQuery.AsAccepter(transferModel.LeaverId);
                    break;
            }
            tasks = tasks.GetInProject(transferModel.ProjectId).InFilter(
                new FFLTask.SRV.ViewModel.Task.FilterModel{ 
                    SelectedStage = (int?)transferModel.SelectedStatus});

            return tasks;
        }

        private IEnumerable<TransferSearchResultItemModel> groupedInRole(int userId, Role role)
        {
            string queryShell = "SELECT" +
                " cast(Project_id as SIGNED) AS ProjectId," +
                " cast(COUNT(Project_id) as SIGNED) AS Total," +
                " cast(SUM(CASE" +
                        " WHEN CurrentStatus <> 'Accept' AND CurrentStatus <> 'Remove'" +
                        " THEN 1 ELSE 0" +
                     " END) as SIGNED) AS InCompleted" +
                " FROM task WHERE {0}" +
                " GROUP BY Project_id";

            string roleFilter = string.Empty;

            switch (role)
            {
                case Role.Publisher:
                    roleFilter = "Publisher_Id=" + userId;
                    break;
                case Role.Owner:
                    roleFilter = "Owner_Id=" + userId;
                    break;
                case Role.Accepter:
                    roleFilter = "Accepter_Id=" + userId;
                    break;
            }

            return session.CreateSQLQuery(string.Format(queryShell, roleFilter))
                .AddScalar("ProjectId", NHibernateUtil.Int32)
                .AddScalar("Total", NHibernateUtil.Int32)
                .AddScalar("InCompleted", NHibernateUtil.Int32)
                .SetResultTransformer(Transformers.AliasToBean(
                    typeof(TransferSearchResultItemModel)))
                .List<TransferSearchResultItemModel>();

        }

        private IList<int> getChargedStatus()
        {
            return Enum.GetValues(typeof(Status)).Cast<int>()
                    .Where(s => (s != (int)Status.Accept && s != (int)Status.Remove))
                    .ToList();
        }
    }
}
