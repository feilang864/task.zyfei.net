using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Auth;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Test;

namespace FFLTask.SRV.UIDevService
{
    public class AuthorizationService : IAuthroizationService
    {
        public IList<Token> GetTokens(int userId, int projectId)
        {
            List<Token> Tokens = new List<Token>();
            FakeUsers user = (FakeUsers)userId;
            FakeProjects Project = (FakeProjects)projectId;
            if (Project == FakeProjects.美工)
            {
                switch (user)
                {
                    case FakeUsers.心晴:
                        Tokens.Add(Token.Founder);
                        Tokens.Add(Token.Owner);
                        break;
                    case FakeUsers.叶子:
                        Tokens.Add(Token.Assinger);
                        Tokens.Add(Token.Owner);
                        break;
                    default:
                        break;
                }

            }
            if (Project == FakeProjects.UI)
            {
                switch (user)
                {
                    case FakeUsers.心晴:
                        Tokens.Add(Token.Owner);
                        break;
                    case FakeUsers.叶子:
                        Tokens.Add(Token.Assinger);
                        Tokens.Add(Token.Owner);
                        Tokens.Add(Token.Founder);
                        break;
                    case FakeUsers.自由飞:
                        Tokens.Add(Token.Founder);
                        Tokens.Add(Token.Owner);
                        break;
                    default:
                        break;

                }
                if (Project == FakeProjects.后台)
                {
                    switch (user)
                    {
                        case FakeUsers.自由飞:
                            Tokens.Add(Token.Owner);
                            Tokens.Add(Token.Founder);
                            break;
                        case FakeUsers.叶子:
                            Tokens.Add(Token.Owner);
                            Tokens.Add(Token.Founder);
                            break;
                        case FakeUsers.技术宅:
                            Tokens.Add(Token.Owner);
                            break;
                        case FakeUsers.科技改变生活:
                            break;
                        default:
                            break;
                    }
                }
            }
            return Tokens;
        }

        public IList<ProjectMemberModel> Get(int projectId)
        {
            return new List<ProjectMemberModel>
            {
                new ProjectMemberModel 
                {
                    User = new UserModel { Name = "叶子", Id = 7},
                    CanManage = true, CanOwn = true, CanPublish = true
                },
                new ProjectMemberModel
                {
                    User = new UserModel { Name ="技术宅", Id = 13},
                    CanManage = false, CanOwn = true, CanPublish = false
                }
            };
        }

        public IList<ProjectAuthorizationModel> GetAdmined(int userId)
        {
            IList<AuthorizationModel> auths = new List<AuthorizationModel> 
            {
                new AuthorizationModel
                {
                     CanAdmin=false,
                     CanOwn=true,
                     CanPublish=true,
                     User=new UserModel{ Id=1, Name="心情"},
                     Id=3
                },
                new AuthorizationModel
                {
                     CanAdmin=false,
                     CanOwn=true,
                     CanPublish=true,
                     User=new UserModel{ Id=1, Name="自由飞"},
                     Id=4
                }
            };
            ProjectAuthorizationModel model1 = new ProjectAuthorizationModel
            {
                Id = 1,
                Name = "首顾科技",
                Authorizations = auths
            };
            ProjectAuthorizationModel model2 = new ProjectAuthorizationModel
            {
                Id = 2,
                Name = "任务管理",
                Authorizations = auths
            };
            IList<ProjectAuthorizationModel> models = new List<ProjectAuthorizationModel> { model1, model2 };
            return models;
        }

        public void Update(AuthorizationModel model)
        {
            throw new System.NotImplementedException();
        }


        public void Dismiss(int userId, IList<int> projectIds)
        {
            throw new System.NotImplementedException();
        }


        public IList<int> GetAdminedProjectIds(int userId)
        {
            throw new System.NotImplementedException();
        }
    }
}
