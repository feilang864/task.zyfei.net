using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.GLB.Global.Enum;
using FFLTask.GLB.Global.UrlParameter;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.SRV.ViewModel.Test;
using Global.Core.ExtensionMethod;
using FFLTask.SRV.ViewModel.Team;

namespace FFLTask.SRV.UIDevService
{
    public class TaskService : ITaskService
    {
        private ProjectService _projectService;
        private UserService _userService;
        private ProjectConfigService _projectConfigService;
        public TaskService()
        {
            _projectService = new ProjectService();
            _userService = new UserService();
            _projectConfigService = new ProjectConfigService();
        }

        #region Public Methods Implementation

        public FullItemModel Get(int taskId)
        {
            return getAll()
                .Where(t => t.LiteItem.Id == taskId)
                .SingleOrDefault();
        }

        public int GetCount(ListModel model)
        {
            return getAll().Where(t => t.Project.LiteItem.Id == model.CurrentProject.TailSelectedProject.Id).ToList().Count;
        }

        public IList<FullItemModel> Get(string sort, bool des, ListModel model)
        {
            //TODO: not sure which one should be check first, projectId or taskId
            var items = getAll().Where(t => t.Project.LiteItem.Id == model.CurrentProject.TailSelectedProject.Id);
            if (model.TaskId != null)
            {
                items = getAll().Where(t => t.LiteItem.Id == model.TaskId);
                return items.ToList();
            }
            if (model.GreaterOverDue != null)
            {
                items = items.Where(t => t.OverDue > model.GreaterOverDue);
            }
            if (model.LessOverDue != null)
            {
                items = items.Where(t => t.OverDue < model.LessOverDue);
            }
            if (model.GreaterWorkPeriod != null)
            {
                items = items.Where(t => t.ActualWorkPeriod > model.GreaterWorkPeriod);
            }
            if (model.LessWorkPeriod != null)
            {
                items = items.Where(t => t.ActualWorkPeriod < model.LessWorkPeriod);
            }
            //TODO: ExpectedComplete is not DateTime
            //if (model.FromExpectComplete != null)
            //{
            //    items = items.Where(t => t.ExpectedComplete > model.FromExpectComplete);
            //}
            //if (model.ToExpectComplete != null)
            //{
            //    items = items.Where(t => t.ExpectedComplete < model.ToExpectComplete);
            //}
            if (model.TimeSpan.FromPublish != null)
            {
                items = items.Where(t => t.Created > model.TimeSpan.FromPublish);
            }
            if (model.TimeSpan.ToPublish != null)
            {
                items = items.Where(t => t.Created < model.TimeSpan.ToPublish);
            }
            if (model.TimeSpan.FromAssign != null)
            {
                items = items.Where(t => t.Assign > model.TimeSpan.FromAssign);
            }
            if (model.TimeSpan.ToAssign != null)
            {
                items = items.Where(t => t.Assign > model.TimeSpan.ToAssign);
            }
            if (model.TimeSpan.FromOwn != null)
            {
                items = items.Where(t => t.Own > model.TimeSpan.FromOwn);
            }
            if (model.TimeSpan.ToOwn != null)
            {
                items = items.Where(t => t.Own > model.TimeSpan.ToOwn);
            }
            //if (model.FromExpectComplete != null)
            //{
            //    items = items.Where(t => t.ExpectedComplete > model.FromExpectComplete);
            //}
            //if (model.ToExpectComplete != null)
            //{
            //    items = items.Where(t => t.ExpectedComplete > model.ToExpectComplete);
            //}
            if (model.TimeSpan.FromActualComplete != null)
            {
                items = items.Where(t => t.ActualComplete > model.TimeSpan.FromActualComplete);
            }
            if (model.TimeSpan.ToActualComplete != null)
            {
                items = items.Where(t => t.ActualComplete < model.TimeSpan.ToActualComplete);
            }
            if (model.TimeSpan.FromLastestUpdateTime != null)
            {
                items = items.Where(t => t.LatestUpdate > model.TimeSpan.FromLastestUpdateTime);
            }
            if (model.TimeSpan.ToLastestUpdateTime != null)
            {
                items = items.Where(t => t.LatestUpdate > model.TimeSpan.ToLastestUpdateTime);
            }

            if (model.SelectedDifficulty != null)
            {
                items = items.Where(t => t.Difficulty != null &&
                    t.Difficulty == (TaskDifficulty)model.SelectedDifficulty);
            }
            if (model.SelectedOwnerId != null)
            {
                items = items.Where(t => t.Owner != null &&
                    t.Owner.Id == model.SelectedOwnerId.Value);
            }
            if (model.SelectedPublisherId != null)
            {
                items = items.Where(t => t.Publisher != null &&
                    t.Publisher.Id == model.SelectedPublisherId.Value);
            }
            if (model.SelectedStage != null)
            {
                items = items.Where(t => t.LiteItem.CurrentStatus != null &&
                    t.LiteItem.CurrentStatus.Stage == model.SelectedStage.Value);
            }
            if (model.SelectedQuality != null)
            {
                items = items.Where(t => t.Quality != null &&
                    t.Quality == (TaskQuality)model.SelectedQuality);
            }

            items = items.Sort(sort, des);

            return Paged(items, model.Pager)
                .ToList();
        }

        public int Create(NewModel model, int creatorId)
        {
            return -1;
        }

        public void Own(int taskId, int ownerId)
        {
        }

        private IList<StatusModel> getQualifiedStatus(int status)
        {
            IList<StatusModel> qualified = new List<StatusModel>();

            //TODO:
            StatusModel ownModel = new StatusModel();
            //ownModel.FilledBy(Status.Own);

            StatusModel cancelModel = new StatusModel();
            //cancelModel.FilledBy(Status.Cancel);

            StatusModel completeModel = new StatusModel();
            //completeModel.FilledBy(Status.Complete);

            StatusModel pauseModel = new StatusModel();
            //pauseModel.FilledBy(Status.Pause);

            if (status == (int)Status.Assign)
            {
                qualified.Add(ownModel);
                qualified.Add(cancelModel);
            }
            else if (status == (int)Status.Own)
            {
                qualified.Add(pauseModel);
                qualified.Add(completeModel);
                qualified.Add(cancelModel);
            }
            else if (status == (int)Status.Pause)
            {
                qualified.Add(ownModel);
                qualified.Add(cancelModel);
            }
            else if (status == (int)Status.Complete)
            {
                qualified.Add(ownModel);
            }
            return qualified;
        }

        public IList<TaskHistoryItemModel> GetHistory(int taskId)
        {
            //gao,yan: build a list of TaskHistoryItemModel here, needn't consider taskIdtor
            IList<TaskHistoryItemModel> Histortys = new List<TaskHistoryItemModel>(){
            new TaskHistoryItemModel(){ CreateTime=new DateTime (2014,01,02,23,23,23),Executor=new UserModel(){ Name= FakeUsers.科技改变生活.ToString()},Description="做修改改改改改改改改……"},
            new TaskHistoryItemModel(){ CreateTime=new DateTime (2015,04,05,23,23,23),Executor=new UserModel(){Name =FakeUsers.技术宅.ToString()} ,Description="做修改改改改改改改改……"},
            new TaskHistoryItemModel(){ CreateTime=new DateTime (2001,01,11,23,23,23),Executor= new UserModel(){Name =FakeUsers.心晴.ToString()},Description="做修改改改改改改改改……"},
            new TaskHistoryItemModel(){ CreateTime=new DateTime (2009,11,12,03,33,23),Executor= new UserModel(){Name= FakeUsers.叶子.ToString()},Description="做修改改改改改改改改……"}
     };
            return Histortys;
        }

        public void Expire(int taskId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskProperty(FullItemModel model)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Private Methods

        private static IEnumerable<FullItemModel> _all;
        private IEnumerable<FullItemModel> getAll()
        {
            if (_all != null)
            {
                return _all;
            }

            IList<FullItemModel> tasks = new List<FullItemModel>();
            for (int i = 1; i <= 256; i++)
            {
                DateTime created = new DateTime().Random(DateTime.Now.AddDays(i - 300));
                FullItemModel task = new FullItemModel
                {
                    Created = created,
                    LiteItem = new LiteItemModel
                    {
                        Id = i,
                        Title = string.Format("[ {0} ]balabalabdafdfasd", i)
                    }
                };
                tasks.Add(task);

                if (i <= 98)
                {
                    task.Project = new FFLTask.SRV.ViewModel.Project.FullItemModel
                    {
                        LiteItem = new FFLTask.SRV.ViewModel.Project.LiteItemModel
                        {
                            Id = (int)FakeProjects.美工,
                            Name = FakeProjects.美工.ToString()
                        },
                        Description = ""
                    };
                }
                else if (i > 98 & i < 116)
                {
                    task.Project = new FFLTask.SRV.ViewModel.Project.FullItemModel
                    {
                        LiteItem = new FFLTask.SRV.ViewModel.Project.LiteItemModel
                        {
                            Id = (int)FakeProjects.UI,
                            Name = FakeProjects.UI.ToString()
                        },
                        Description = ""
                    };
                }
                else
                {
                    task.Project = new FFLTask.SRV.ViewModel.Project.FullItemModel
                    {
                        LiteItem = new FFLTask.SRV.ViewModel.Project.LiteItemModel
                        {
                            Id = (int)FakeProjects.后台,
                            Name = FakeProjects.后台.ToString()
                        },
                        Description = ""
                    };
                }

                int maxPriority = _projectConfigService.GetPriorities(task.Project.LiteItem.Id).Count;
                task.Priority = (TaskPriority)(i % maxPriority + 1);

                var publishers = _userService.GetPublishers(task.Project.LiteItem.Id);
                setPublisher(publishers, task);

                var difficulties = _projectConfigService.GetDifficulties(task.Project.LiteItem.Id);
                setDifficulty(difficulties, task);

                task.ExpectedWorkPeriod = new Random().Next(60);
                //TODO: need reference ViewModelMap?
                //task.ExpectedComplete = new DateTime().Random(task.Created);

                var status = _projectConfigService.GetStatus(task.Project.LiteItem.Id);
                setStatus(status, task);

                if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.发布)
                {
                    task.LatestUpdate = task.Created;
                }
                else
                {
                    var owners = _userService.GetOwners(task.Project.LiteItem.Id);
                    if (i % 3 != 0)
                    {
                        setPublisher(owners, task);
                        task.Assign = new DateTime().Random(task.Created);
                    }

                    if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.分配)
                    {
                        task.LatestUpdate = task.Assign.Value;
                    }
                    else
                    {
                        setOwner(owners, task);

                        DateTime previousOwn = task.Assign ?? task.Created;
                        task.Own = new DateTime().Random(previousOwn);

                        if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.承接)
                        {
                            task.LatestUpdate = task.Own.Value;
                        }
                        else
                        {
                            task.ActualComplete = new DateTime().Random(task.Own.Value);
                            task.ActualWorkPeriod = (int)Math.Floor((task.ActualComplete.Value - task.Own.Value).TotalDays * 24 * 60);
                            task.OverDue = task.ExpectedWorkPeriod.Value - task.ActualWorkPeriod.Value;

                            //complete the task
                            if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.完成)
                            {
                                task.LatestUpdate = task.ActualComplete.Value;
                            }

                            if (task.LiteItem.CurrentStatus.Stage >= (int)FakeStatus.验收通过)
                            {
                                task.LatestUpdate = new DateTime().Random(task.ActualComplete.Value);
                                var qualities = _projectConfigService.GetQualities(task.Project.LiteItem.Id);
                                int quality = qualities.Count / 2 + 1;

                                int qualityIndex = 1;
                                if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.验收通过)
                                {
                                    qualityIndex = new Random().Next(quality, qualities.Count);
                                }
                                else if (task.LiteItem.CurrentStatus.Stage == (int)FakeStatus.验收失败)
                                {
                                    qualityIndex = new Random().Next(qualityIndex, quality);
                                }
                                task.Quality = qualities[qualityIndex - 1];
                            }
                        }
                    }
                }

            }

            _all = tasks;

            return tasks;
        }

        private IEnumerable<FullItemModel> Paged(IEnumerable<FullItemModel> items, PagerModel pager)
        {
            return items
                .Skip(pager.PageSize * (pager.PageIndex - 1))
                .Take(pager.PageSize);
        }

        private void setStatus(IList<StatusModel> status, FullItemModel task)
        {
            int i = task.LiteItem.Id.Value % status.Count;
            task.LiteItem.CurrentStatus = status[i];
        }

        private void setDifficulty(IList<TaskDifficulty> difficulties, FullItemModel task)
        {
            int i = task.LiteItem.Id.Value % difficulties.Count;
            task.Difficulty = difficulties[i];
        }

        private void setPublisher(IList<UserModel> publishers, FullItemModel task)
        {
            int i = task.LiteItem.Id.Value % publishers.Count;
            task.Publisher = publishers[i];
        }

        private void setOwner(IList<UserModel> owners, FullItemModel task)
        {
            int i = task.LiteItem.Id.Value % owners.Count;
            task.Owner = owners[i];
        }

        //private void setAssigner(IList<UserModel> owners, TaskItemModel task)
        //{
        //    int i = task.Id % owners.Count;
        //    task.Assigner = owners[i];
        //}

        #endregion



        public void Assign(FullItemModel model)
        {
            throw new NotImplementedException();
        }

        public void Own(FullItemModel model)
        {
            throw new NotImplementedException();
        }

        public void Accept(FullItemModel model)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskProperty(EditModel model)
        {
            throw new NotImplementedException();
        }

        public void Assign(EditModel model)
        {
            throw new NotImplementedException();
        }

        public void Own(EditModel model, int currentStatus)
        {
            throw new NotImplementedException();
        }

        public EditModel GetEdit(int taskId, UserModel currentUser)
        {
            throw new NotImplementedException();
        }

        public void Accept(EditModel model)
        {
            throw new NotImplementedException();
        }

        public void Handle(EditModel model)
        {
            throw new NotImplementedException();
        }

        public void Refuse(EditModel model)
        {
            throw new NotImplementedException();
        }

        public string GetTitle(int taskId)
        {
            throw new NotImplementedException();
        }

        public TaskRelationModel GetRelation(int taskId)
        {
            TaskRelationModel model = new TaskRelationModel();
            model.Current = new LiteItemModel
            {
                Id = 1,
                Title = "当前任务"
            };

            model.Ancestor = new LinkedList<LiteItemModel>();
            LiteItemModel root = new LiteItemModel
            {
                Id = 2,
                Title = "这是根任务 "
            };
            LiteItemModel parent = new LiteItemModel
            {
                Id = 3,
                Title = "这个是父任务 "
            };
            model.Ancestor.AddFirst(parent);
            model.Ancestor.AddFirst(root);

            model.Brothers = new List<LiteItemModel>();
            LiteItemModel brother_1 = new LiteItemModel
            {
                Id = 4,
                Title = "这个是兄弟任务1"
            };
            LiteItemModel brother_2 = new LiteItemModel
            {
                Id = 5,
                Title = "这个是兄弟任务2 "
            };
            model.Brothers.Add(brother_1);
            model.Brothers.Add(brother_2);


            model.Children = new List<LiteItemModel>();
            LiteItemModel child_1 = new LiteItemModel
            {
                Id = 6,
                Title = "这个是子任务1"
            };
            LiteItemModel child_2 = new LiteItemModel
            {
                Id = 7,
                Title = "这个是子任务2"
            };
            LiteItemModel child_3 = new LiteItemModel
            {
                Id = 8,
                Title = "这个是子任务3"
            };
            model.Children.Add(child_1);
            model.Children.Add(child_2);
            model.Children.Add(child_2);

            return model;
        }

        public LiteItemModel GetLite(int taskId)
        {
            throw new NotImplementedException();
        }

        public void UpdateTaskSequence(IList<int> taskIds)
        {
            throw new NotImplementedException();
        }

        public void UploadFiles(IList<string> files, int taskId, int uploaderId)
        {
            throw new NotImplementedException();
        }

        public void Remove(int taskId, string comment)
        {
            throw new NotImplementedException();
        }

        public void Resume(int taskId, string comment)
        {
            throw new NotImplementedException();
        }

        public SequenceModel GetSequence(int taskId)
        {
            throw new NotImplementedException();
        }

        public void Comment(EditModel model)
        {
            throw new NotImplementedException();
        }

        public _SumModel GetSum(ListModel model)
        {
            _SumModel sumModel = new _SumModel
            {
                Types = new Dictionary<string, int> 
                { 
                    { "虚", 2 }, 
                    { "实", 12 }
                },
                Priorities = new Dictionary<string, int>
                { 
                    { "高", 2 },
                    { "中", 12 }, 
                    { "低", 3 } 
                },
                Difficulties = new Dictionary<string, int> 
                { 
                    { "困难", 2 }, 
                    { "普通", 11 } 
                },
                ConsumeTime = new Dictionary<string, int> 
                { 
                    { "预计", 235 }, 
                    { "实际", 386 } 
                },
                Status = new Dictionary<string, int> 
                {
                    { "发布", 1 }, 
                    { "承接", 2 }, 
                    { "质疑", 1 },
                    { "完成", 3 },
                    { "合格", 8 }, 
                    { "不合格", 1 }
                },
                OverDue = new Dictionary<string, int>
                { 
                    { "条数", 3 }, 
                    { "次数", 7 }
                },
                Qualities = new Dictionary<string, int>
                { 
                    { "良好", 2 }, 
                    { "合格", 8 }, 
                    { "不合格", 1 } 
                },
                Doubt = new Dictionary<string, int>
                { 
                    { "条数", 3 }, 
                    { "次数", 9 },
                },
                RefuseAccept = new Dictionary<string, int>
                { 
                    { "良好", 2 }, 
                    { "合格", 8 }, 
                    { "不合格", 1 } 
                },
            };

            return sumModel;
        }

        public bool ParentIsOffspring(int childId, int parentId)
        {
            throw new NotImplementedException();
        }

        public bool CanAutoComplete(int taskId)
        {
            throw new NotImplementedException();
        }

        public bool CanAutoAccept(int taskId)
        {
            throw new NotImplementedException();
        }

        public IList<LiteItemModel> GetStartWith(string title)
        {
            return new List<LiteItemModel>
            {
                new LiteItemModel{ Id= 12, Title = "parent task - 12"},
                new LiteItemModel{Id= 36, Title = "parent task - 36"}
            };
        }
    }

    internal static class ExtensionMethods
    {
        internal static IEnumerable<FullItemModel> Sort(this IEnumerable<FullItemModel> items, string sort, bool des)
        {
            //TODO: just a walkaround, not sure how to set the default
            if (string.IsNullOrEmpty(sort))
            {
                return items.OrderByDescending(t => t.Created);
            }

            if (des)
            {
                return items.sortDescend(sort);
            }
            else
            {
                return items.sortAscend(sort);
            }

        }

        internal static IEnumerable<FullItemModel> sortDescend(this IEnumerable<FullItemModel> items, string sort)
        {
            if (sort == TaskList.Sort_By_ActualComplete)
            {
                return items.OrderByDescending(t => t.ActualComplete);
            }
            else if (sort == TaskList.Sort_By_ExpectedComplete)
            {
                return items.OrderByDescending(t => t.ExpectedComplete);
            }
            else if (sort == TaskList.Sort_By_ActualWorkPeriod)
            {
                return items.OrderByDescending(t => t.ActualWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_ExpectedWorkPeriod)
            {
                return items.OrderByDescending(t => t.ExpectedWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_Created)
            {
                return items.OrderByDescending(t => t.Created);
            }
            else if (sort == TaskList.Sort_By_Own)
            {
                return items.OrderByDescending(t => t.Own);
            }
            else if (sort == TaskList.Sort_By_Assign)
            {
                return items.OrderByDescending(t => t.Assign);
            }
            else if (sort == TaskList.Sort_By_LatestUpdate)
            {
                return items.OrderByDescending(t => t.LatestUpdate);
            }
            else if (sort == TaskList.Sort_By_OverDue)
            {
                return items.OrderByDescending(t => t.OverDue);
            }
            else if (sort == TaskList.Sort_By_Priority)
            {
                return items.OrderByDescending(t => t.Priority);
            }
            else
            {
                throw new Exception(string.Format("the sort parameter ({0}) is not correct.", sort));
            }
        }

        internal static IEnumerable<FullItemModel> sortAscend(this IEnumerable<FullItemModel> items, string sort)
        {
            if (sort == TaskList.Sort_By_ActualComplete)
            {
                return items.OrderBy(t => t.ActualComplete);
            }
            else if (sort == TaskList.Sort_By_ExpectedComplete)
            {
                return items.OrderBy(t => t.ExpectedComplete);
            }
            else if (sort == TaskList.Sort_By_ActualWorkPeriod)
            {
                return items.OrderBy(t => t.ActualWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_ExpectedWorkPeriod)
            {
                return items.OrderBy(t => t.ExpectedWorkPeriod);
            }
            else if (sort == TaskList.Sort_By_Created)
            {
                return items.OrderBy(t => t.Created);
            }
            else if (sort == TaskList.Sort_By_Own)
            {
                return items.OrderBy(t => t.Own);
            }
            else if (sort == TaskList.Sort_By_Assign)
            {
                return items.OrderBy(t => t.Assign);
            }
            else if (sort == TaskList.Sort_By_LatestUpdate)
            {
                return items.OrderBy(t => t.LatestUpdate);
            }
            else if (sort == TaskList.Sort_By_OverDue)
            {
                return items.OrderBy(t => t.OverDue);
            }
            else if (sort == TaskList.Sort_By_Priority)
            {
                return items.OrderBy(t => t.Priority);
            }
            else
            {
                throw new Exception(string.Format("the sort parameter ({0}) is not correct.", sort));
            }
        }
    }
}