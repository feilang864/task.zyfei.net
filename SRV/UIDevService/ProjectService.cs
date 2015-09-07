using System;
using System.Collections.Generic;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.UIDevService
{
    public class ProjectService : IProjectService
    {
        public int Create(CreateModel model, int? parentId, int userId)
        {
            return -1;
        }

        public int Join(int projectId, int userId)
        {
            return -1;
        }

        public int GetByName(string name)
        {
            FakeProjects project;
            if (Enum.TryParse<FakeProjects>(name, out project))
            {
                return (int)project;
            }
            return 0;
        }

        public IList<FullItemModel> GetByParent(int parentId)
        {
            throw new NotImplementedException();
        }

        public SummaryModel GetSummary(int projectId)
        {
            throw new NotImplementedException();
        }

        public _DropdownlistLinkedModel GetDropdownlistLink(int userId, int projectId)
        {
            _DropdownlistLinkedModel model = new _DropdownlistLinkedModel();

            model.LinkedProject = new LinkedList<_DropdownlistLinkedNodeModel>();
            model.TailSelectedProject = getProjectById(projectId);
            addFromLast(model, model.LinkedProject, projectId);

            return model;
        }

        private void addFromLast(_DropdownlistLinkedModel model, LinkedList<_DropdownlistLinkedNodeModel> linkedList, int projectId)
        {
            _DropdownlistLinkedNodeModel first = null;
            if (linkedList.Last == null)
            {
                first = new _DropdownlistLinkedNodeModel();
                linkedList.AddLast(first);
            }
            else
            {
                first = linkedList.First.Value;
            }

            first.CurrentProject = getProjectById(projectId);

            _DropdownlistLinkedNodeModel pre = getParent(projectId);
            if (pre != null)
            {
                linkedList.AddBefore(linkedList.First, pre);
                first.Projects = getFromParent(pre.CurrentProject.Id);

                addFromLast(model, linkedList, pre.CurrentProject.Id);
            }
            else
            {
                model.TailSelectedProject = getParentGroup(projectId);
                first.Projects = getFromGroup(model.TailSelectedProject.Id);
            }

        }

        private IList<LiteItemModel> getFromGroup(int groupId)
        {
            IList<LiteItemModel> models = new List<LiteItemModel>();
            switch (groupId)
            {
                case 1:
                    models.Add(new LiteItemModel { Id = 2, Name = "创业" });
                    models.Add(new LiteItemModel { Id = 3, Name = "任务管理" });
                    break;
                case 2:
                    models.Add(new LiteItemModel { Id = 15, Name = "行政" });
                    break;
            }
            return models;
        }

        private LiteItemModel getParentGroup(int projectId)
        {
            LiteItemModel model = null;
            switch (projectId)
            {
                case 2:
                case 3:
                    model = new LiteItemModel { Id = 1, Name = "自由飞" };
                    break;
                case 15:
                    model = new LiteItemModel { Id = 2, Name = "首顾科技" };
                    break;
            }
            return model;
        }

        private IList<LiteItemModel> getFromParent(int projectId)
        {
            IList<LiteItemModel> models = new List<LiteItemModel>();
            switch (projectId)
            {
                case 2:
                    models.Add(new LiteItemModel { Id = 5, Name = "前台" });
                    models.Add(new LiteItemModel { Id = 7, Name = "后台" });
                    models.Add(new LiteItemModel { Id = 9, Name = "UI" });
                    break;
                case 3:
                    models.Add(new LiteItemModel { Id = 11, Name = "SEO" });
                    models.Add(new LiteItemModel { Id = 12, Name = "设计" });
                    models.Add(new LiteItemModel { Id = 14, Name = "DBA" });
                    break;
            }
            return models;
        }

        private _DropdownlistLinkedNodeModel getParent(int projectId)
        {
            _DropdownlistLinkedNodeModel model = new _DropdownlistLinkedNodeModel();
            switch (projectId)
            {
                case 2:
                case 3:
                case 15:
                    return null;
                case 5:
                case 7:
                case 9:
                    model = new _DropdownlistLinkedNodeModel
                    {
                        CurrentProject = new LiteItemModel { Id = 2, Name = "创业" }
                    };
                    break;
                case 11:
                case 12:
                case 14:
                    model = new _DropdownlistLinkedNodeModel
                    {
                        CurrentProject = new LiteItemModel { Id = 3, Name = "任务管理" }
                    };
                    break;
            }
            return model;
        }

        private LiteItemModel getProjectById(int projectId)
        {
            LiteItemModel model = new LiteItemModel();
            switch (projectId)
            {
                case 2:
                    model = new LiteItemModel { Id = 2, Name = "创业" };
                    break;
                case 3:
                    model = new LiteItemModel { Id = 3, Name = "任务管理" };
                    break;
                case 5:
                    model = new LiteItemModel { Id = 5, Name = "前台" };
                    break;
                case 7:
                    model = new LiteItemModel { Id = 7, Name = "后台" };
                    break;
                case 9:
                    model = new LiteItemModel { Id = 9, Name = "UI" };
                    break;
                case 11:
                    model = new LiteItemModel { Id = 11, Name = "SEO" };
                    break;
                case 12:
                    model = new LiteItemModel { Id = 12, Name = "设计" };
                    break;
                case 14:
                    model = new LiteItemModel { Id = 14, Name = "DBA" };
                    break;
                case 15:
                    model = new LiteItemModel { Id = 15, Name = "行政" };
                    break;
            }
            return model;
        }

        public _DropdownlistLinkedModel GetDropdownlistLink(int userId)
        {
            LiteItemModel zyfei = new LiteItemModel { Id = 1, Name = "自由飞" };
            LiteItemModel sgkj = new LiteItemModel { Id = 2, Name = "首顾科技" };
            LiteItemModel baichuan = new LiteItemModel { Id = 3, Name = "百川开源" };
            _DropdownlistLinkedModel model = new _DropdownlistLinkedModel
            {
                LinkedProject = new LinkedList<_DropdownlistLinkedNodeModel>()
            };

            LiteItemModel chuangye = new LiteItemModel { Id = 21, Name = "创业" };
            LiteItemModel task = new LiteItemModel { Id = 22, Name = "任务管理" };
            LiteItemModel zhuangshi = new LiteItemModel { Id = 23, Name = "装饰点评" };
            model.LinkedProject = new LinkedList<_DropdownlistLinkedNodeModel>();
            _DropdownlistLinkedNodeModel first = new _DropdownlistLinkedNodeModel
            {
                CurrentProject = task,
                Projects = new List<LiteItemModel> { chuangye, task, zhuangshi }
            };
            model.LinkedProject.AddFirst(first);

            LiteItemModel front = new LiteItemModel { Id = 31, Name = "前台" };
            LiteItemModel backend = new LiteItemModel { Id = 32, Name = "后台" };
            LiteItemModel ui = new LiteItemModel { Id = 33, Name = "UI" };
            LiteItemModel SEO = new LiteItemModel { Id = 34, Name = "SEO" };
            LiteItemModel design = new LiteItemModel { Id = 35, Name = "设计" };
            LiteItemModel DBA = new LiteItemModel { Id = 36, Name = "DBA" };
            LiteItemModel Admin = new LiteItemModel { Id = 37, Name = "行政" };
            _DropdownlistLinkedNodeModel second = new _DropdownlistLinkedNodeModel
            {
                CurrentProject = front,
                Projects = new List<LiteItemModel> { front, backend, ui }
            };
            model.LinkedProject.AddAfter(model.LinkedProject.Last, second);

            LiteItemModel csharp_4 = new LiteItemModel { Id = 41, Name = "C#" };
            LiteItemModel DBA_4 = new LiteItemModel { Id = 42, Name = "DBA" };
            _DropdownlistLinkedNodeModel third = new _DropdownlistLinkedNodeModel
            {
                CurrentProject = csharp_4,
                Projects = new List<LiteItemModel> { csharp_4, DBA_4 }
            };
            model.LinkedProject.AddAfter(model.LinkedProject.Last, third);

            model.TailSelectedProject = csharp_4;

            model.SelectedProjectHasChild = true;

            return model;
        }

        public bool HasChild(int projectId)
        {
            switch (projectId)
            {
                case 21:
                case 22:
                case 31:
                    return true;
                case 23:
                    return false;
                default:
                    return false;
            }
        }

        public _SearchModel GetLinked(int projectId)
        {
            _SearchModel model = new _SearchModel();
            model.Projects = new List<_LiteralLinkedModel>();

            return model;
        }

        public _SearchModel GetByPartialName(string projectName)
        {
            _SearchModel model = new _SearchModel();
            model.Projects = new List<_LiteralLinkedModel>();

            _LiteralLinkedModel nodeModel = new _LiteralLinkedModel();
            nodeModel.LinkedList = new LinkedList<LiteItemModel>();

            LiteItemModel nodemodel_1 = new LiteItemModel { Id = 1, Name = "首顾科技" };
            LiteItemModel nodemodel_2 = new LiteItemModel { Id = 2, Name = "创业家园" };
            LiteItemModel nodemodel_3 = new LiteItemModel { Id = 3, Name = "后台" };
            LiteItemModel nodemodel_4 = new LiteItemModel { Id = 4, Name = "DBA" };

            if ("DBA".Contains(projectName))
            {
                nodeModel.LinkedList.AddLast(nodemodel_1);
                nodeModel.LinkedList.AddLast(nodemodel_2);
                nodeModel.LinkedList.AddLast(nodemodel_3);
            }
            if ("DBA".Contains(projectName))
            {
                nodeModel.LinkedList.AddLast(nodemodel_1);
                nodeModel.LinkedList.AddLast(nodemodel_2);
                nodeModel.LinkedList.AddLast(nodemodel_3);
                nodeModel.LinkedList.AddLast(nodemodel_4);
            }
            model.Projects.Add(nodeModel);

            return model;
        }

        public _DropdownlistLinkedNodeModel GetNextNode(int projectId)
        {
            _DropdownlistLinkedNodeModel model = new _DropdownlistLinkedNodeModel
            {
                CurrentProject = new LiteItemModel { Id = 8, Name = "DBA" },
                Projects = new List<LiteItemModel> 
                { 
                    new LiteItemModel{ Id = 10, Name = "C#"},
                    new LiteItemModel{ Id = 12, Name = "javascript"},
                }
            };
            return model;
        }

        public JoinModel GetJoinTree(int projectId, int currentUserId)
        {
            JoinModel model_1_1 = new JoinModel
            {
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 18, Name = "MS SQL" } }
            };
            JoinModel model_1_2 = new JoinModel
            {
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 19, Name = "MySQL" } }
            };
            JoinModel model_1 = new JoinModel
            {
                HasJoined = false,
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 7, Name = "DBA" } },
                Children = new List<JoinModel> { model_1_1, model_1_2 }
            };
            JoinModel model_2_1 = new JoinModel
            {
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 22, Name = "BLL" } }
            };
            JoinModel model_2_2 = new JoinModel
            {
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 23, Name = "Service层" } }
            };
            JoinModel model_2 = new JoinModel
            {
                HasJoined = true,
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 8, Name = "C#" } },
                Children = new List<JoinModel> { model_2_1, model_2_2 }
            };
            JoinModel model = new JoinModel
            {
                HasJoined = true,
                Item = new FullItemModel { LiteItem = new LiteItemModel { Id = 3, Name = "后台" } },
                Children = new List<JoinModel> { model_1, model_2 }
            };

            return model;
        }

        public EditModel GetEdit(int projectId)
        {
            EditModel model = new EditModel
            {
                Id = projectId,
                Name = "自由飞",
                Description = "让你的梦想只有的飞起来吧"
            };

            return model;
        }

        public bool HasJoined(int projectId, int userId)
        {
            throw new NotImplementedException();
        }

        public bool ParentIsOffspring(int childId, int parentId)
        {
            return parentId <= childId;
        }

        public LinkedList<LiteItemModel> GetLinkedProject(int tailProjectId)
        {
            throw new NotImplementedException();
        }

        public void Modify(EditModel model)
        {
        }
    }

    //TODO: need remove
    public enum FakeProjects
    {
        美工 = 1,
        UI = 2,
        后台 = 3,
        //集成 = 4,
        //DBA = 5,
        //程序 = 6,
        //策划 = 7,
        //SEO = 8
    }
}
