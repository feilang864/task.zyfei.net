using System;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Team;
using System.Collections.Generic;
using FFLTask.GLB.Global.Enum;

namespace FFLTask.SRV.UIDevService
{
    public class TeamService : ITeamService
    {
        public SearchModel GroupedByRole(int userId)
        {
            return new SearchModel
            {
                UserId = 23,
                TransferResult = new TransferSearchResultModel
                {
                    AsAccepter = new List<TransferSearchResultItemModel> { 
                        new TransferSearchResultItemModel
                        {
                            InCompleted = 23,
                            ProjectId = 11,
                            Role = GLB.Global.Enum.Role.Accepter,
                            Total = 245
                        },
                        new TransferSearchResultItemModel
                        {
                            InCompleted = 23,
                            ProjectId = 11,
                            Role = GLB.Global.Enum.Role.Accepter,
                            Total = 245
                        }
                    },
                    AsPublisher = new List<TransferSearchResultItemModel> { 
                        new TransferSearchResultItemModel
                        {
                            InCompleted = 23,
                            ProjectId = 10,
                            Role = Role.Publisher,
                            Total = 245
                        }
                    }
                }
            };
        }

        public IList<TransferItemModel> GetTasks(TransferModel transferModel)
        {
            return new List<TransferItemModel>
            {
                new TransferItemModel
                { 
                    CurrentStatus = Status.Publish,
                    Id = 23,
                    Title = "根据文档，显示正确的页面信息"
                },
                new TransferItemModel
                { 
                    CurrentStatus = Status.Publish,
                    Id = 24,
                    Title = "bug：任务编辑页面提交时显示错误信息"
                }
            };
        }

        public void HandOver(TransferItemModel model,
            Role role, int succesorId, int operaterId)
        {
            throw new NotImplementedException();
        }

        public IList<Status?> GetAllStatus(TransferModel transferModel)
        {
            return new List<Status?> 
            { 
                Status.BeginWork, Status.Publish, Status.Pause 
            };
        }


        public IList<DismissSearchResultItemModel> GroupedByProject(int userId)
        {
            return new List<DismissSearchResultItemModel>
            {
                new DismissSearchResultItemModel
                {
                    ProjectId = 23,
                    Charge = 8
                },
                new DismissSearchResultItemModel
                {
                    ProjectId = 24,
                    Charge = 0
                }
            };
        }
    }
}
