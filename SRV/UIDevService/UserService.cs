using System;
using System.Collections.Generic;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Test;

namespace FFLTask.SRV.UIDevService
{
    public class UserService : IUserService
    {
        public UserModel GetUser(int userId)
        {
            UserModel model = new UserModel();

            model.Id = userId;
            model.Name = ((FakeUsers)userId).ToString();
            model.HasLogon = true;

            return model;
        }

        private IList<UserModel> allUsers()
        {
            throw new NotImplementedException();
        }

        public IList<UserModel> GetOwners(int projectId)
        {
            IList<UserModel> owners = new List<UserModel>();
            FakeProjects project = (FakeProjects)projectId;
            switch (project)
            {
                case FakeProjects.美工:
                    owners.Add(new UserModel { Id = (int)FakeUsers.心晴, Name = FakeUsers.心晴.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    break;
                case FakeProjects.UI:
                    owners.Add(new UserModel { Id = (int)FakeUsers.心晴, Name = FakeUsers.心晴.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.自由飞, Name = FakeUsers.自由飞.ToString() });
                    break;
                case FakeProjects.后台:
                    owners.Add(new UserModel { Id = (int)FakeUsers.自由飞, Name = FakeUsers.自由飞.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.技术宅, Name = FakeUsers.技术宅.ToString() });
                    owners.Add(new UserModel { Id = (int)FakeUsers.科技改变生活, Name = FakeUsers.科技改变生活.ToString() });
                    break;
                default:
                    break;
            }
            return owners;
        }

        public IList<UserModel> GetAllOwners(int projectId)
        {
            throw new NotImplementedException();
        }

        public IList<UserModel> GetPublishers(int projectId)
        {
            IList<UserModel> assigner = new List<UserModel>();
            FakeProjects project = (FakeProjects)projectId;
            switch (project)
            {
                case FakeProjects.美工:
                    assigner.Add(new UserModel { Id = (int)FakeUsers.心晴, Name = FakeUsers.心晴.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    break;
                case FakeProjects.UI:
                    assigner.Add(new UserModel { Id = (int)FakeUsers.心晴, Name = FakeUsers.心晴.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.自由飞, Name = FakeUsers.自由飞.ToString() });
                    break;
                case FakeProjects.后台:
                    assigner.Add(new UserModel { Id = (int)FakeUsers.自由飞, Name = FakeUsers.自由飞.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.叶子, Name = FakeUsers.叶子.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.技术宅, Name = FakeUsers.技术宅.ToString() });
                    assigner.Add(new UserModel { Id = (int)FakeUsers.科技改变生活, Name = FakeUsers.科技改变生活.ToString() });
                    break;
                default:
                    break;
            }
            return assigner;
        }

        public IList<UserModel> GetAllPublishers(int projectId)
        {
            throw new NotImplementedException();
        }

        public IList<UserModel> GetAccepters(int projectId)
        {
            throw new NotImplementedException();
        }

        public IList<UserModel> GetAllAccepters(int projectId)
        {
            throw new NotImplementedException();
        }

        //TODO: comment first
        //public IList<GroupModel> GetGroupsWithProjects(int userId)
        //{
        //    return new List<GroupModel>
        //        {
        //            new GroupModel 
        //            {
        //                 Item = new GroupItemModel { Id = 1, Name = "首顾科技"},
        //                 Projects = new List<ProjectModel>
        //                 {
        //                     new ProjectModel{ Item = new ProjectItemModel{ Name = "前端", Id = 1}, HasJoined = true},
        //                     new ProjectModel{ Item = new ProjectItemModel{ Name = "后台", Id = 2}, HasJoined = false},
        //                     new ProjectModel{ Item = new ProjectItemModel{ Name = "测试", Id = 3}, HasJoined = false},
        //                 }
        //            },
        //            new GroupModel 
        //            {
        //                 Item = new GroupItemModel { Id = 2, Name = "自由飞"},
        //                 Projects = new List<ProjectModel>
        //                 {
        //                     new ProjectModel{ Item = new ProjectItemModel{ Name = "业务", Id = 4}, HasJoined = true},
        //                     new ProjectModel{ Item = new ProjectItemModel{ Name = "程序", Id = 5}, HasJoined = false}
        //                 }
        //            }
        //        };

        //}

        //public IList<GroupItemModel> GetGroups(int userId)
        //{
        //    throw new NotImplementedException();
        //}

        public bool HasUnknownMessage(int userId)
        {
            return true;
        }

        public bool HasJoinedProject(int userId)
        {
            return true;
        }
    }
}
