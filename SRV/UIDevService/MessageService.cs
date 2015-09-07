using System;
using System.Collections.Generic;
using System.Linq;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ServiceInterface;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Message;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.UIDevService
{
    public class MessageService : IMessageService
    {
        public ToMeModel GetToMe(int userId, MessageSort sortedBy, bool des, PagerModel pager, int? projectId = null, int? addresserId = null)
        {
            ToMeModel model = new ToMeModel();

            model.Messages = new List<ToMeItemModel>
            {
                new ToMeItemModel
                {
                    Id=11,
                    PublishTime=new DateTime(2015,1,13,12,11,0),
                    Project=getProject(1),
                    Task=new ViewModel.Task.LiteItemModel{ Id=3, Title="编辑任务时不能修改项目组" },
                    Addresser=new UserModel{Id=14, Name="叶子" },
                    Content="发布时改变下拉列表值也不能生效"
                },
                new ToMeItemModel
                {
                    Id=21,
                    PublishTime=new DateTime(2015,1,12,12,31,0),
                    Project=getProject(2),
                    Task=new ViewModel.Task.LiteItemModel{ Id=23, Title="博客页面的“上一篇”“下一篇”部分报错" },
                    Addresser=new UserModel{ Id=24,Name="叶子" },
                    Content="ViewModel冲突：Main.Blog.BlogPreAndNextModel和Main.PreAndNextModel"
                },
                new ToMeItemModel
                {
                    Id=31,
                    PublishTime=new DateTime(2015,1,11,1,11,0),
                    ReadTime=new DateTime(2015,1,11,11,12,10),
                    Project=getProject(3),
                    Task=new ViewModel.Task.LiteItemModel{ Id=33, Title="准备数据" },
                    Addresser=new UserModel{ Id=34,Name="自由飞" },
                    Content="重心明显移后，劳动人口的绝对数量开始步入下降通道"
                },
                new ToMeItemModel
                {
                    Id=41,
                    PublishTime=new DateTime(2015,1,11,1,11,0),
                    ReadTime=new DateTime(2015,1,11,1,13,0),
                    Project=getProject(4),
                    Task=new ViewModel.Task.LiteItemModel{ Id=43, Title="整理/Blog/{bloggerName} " },
                    Addresser=new UserModel{ Id=44,Name="心情" },
                    Content="整理/Blog/{bloggerName}，能跑起来，先"
                }
            };
            model.Addressers = model.Messages.Select(m => m.Addresser).Distinct().ToList();
            model.Projects = new List<_LiteralLinkedModel>();

            return model;
        }

        public FromMeModel GetFromMe(int userId, MessageSort sortedBy, bool des, PagerModel pager, int? projectId = null, int? addresseeId = null)
        {
            FromMeModel model = new FromMeModel();

            model.Messages = new List<FromMeItemModel>
            {
                new FromMeItemModel
                {
                    Id=11,
                    PublishTime=new DateTime(2015,1,13,12,11,0),
                    Project=getProject(1),
                    Task=new ViewModel.Task.LiteItemModel{ Id=3, Title="引入留言功能" },
                    Addressee=new UserModel{Id=14, Name="叶子" },
                    Content="人口结构失调为代价的，后者带来的问题"
                },
                new FromMeItemModel
                {
                    Id=21,
                    PublishTime=new DateTime(2015,1,12,12,31,0),
                    Project=getProject(2),
                    Task=new ViewModel.Task.LiteItemModel{ Id=23, Title="在BuildDatabase中准备好数据" },
                    Addressee=new UserModel{ Id=24,Name="叶子" },
                    Content="2010年，第六次人口普查显示：0—14岁儿童占总人口的16.60%。"
                },
                new FromMeItemModel
                {
                    Id=31,
                    PublishTime=new DateTime(2015,1,11,1,11,0),
                    ReadTime=new DateTime(2015,1,11,11,12,10),
                    Project=getProject(3),
                    Task=new ViewModel.Task.LiteItemModel{ Id=33, Title="准备数据" },
                    Addressee=new UserModel{ Id=34,Name="自由飞" },
                    Content="重心明显移后，劳动人口的绝对数量开始步入下降通道"
                },
                new FromMeItemModel
                {
                    Id=41,
                    PublishTime=new DateTime(2015,1,11,1,11,0),
                    ReadTime=new DateTime(2015,1,11,1,13,0),
                    Project=getProject(4),
                    Task=new ViewModel.Task.LiteItemModel{ Id=43, Title="实现统计的NHQuery方法" },
                    Addressee=new UserModel{ Id=44,Name="心情" },
                    Content="农村“一孩半”生育政策恰是出生性别比失调的主要原因"
                }
            };
            model.Addressees = model.Messages.Select(m => m.Addressee).Distinct().ToList();
            model.Projects = model.Messages.Select(m => m.Project).ToList();

            return model;
        }

        public int GetFromMeCount(int userId, int? projectId, int? addresseeId)
        {
            if (projectId.HasValue)
            {
                return 55;
            }
            if (addresseeId.HasValue)
            {
                return 44;
            }

            return 99;
        }

        public int GetToMeCount(int userId, int? projectId, int? addresserId)
        {
            if (projectId.HasValue && addresserId.HasValue)
            {
                return 11;
            }
            else if (projectId.HasValue)
            {
                return 22;
            }
            else if (addresserId.HasValue)
            {
                return 33;
            }

            return 55;
        }

        public void DeleteForAddresser(int messageId)
        {
            throw new NotImplementedException();
        }

        public void DeleteForAddressee(int messageId)
        {
            throw new NotImplementedException();
        }

        public void Read(int messageId)
        {
            throw new NotImplementedException();
        }

        public _LiteralLinkedModel getProject(int tailId)
        {
            _LiteralLinkedModel model = new _LiteralLinkedModel
            {
                LinkedList = new LinkedList<LiteItemModel>()
            };

            LiteItemModel zyf = new LiteItemModel { Id = 1, Name = "自由飞" };
            LiteItemModel sgkj = new LiteItemModel { Id = 2, Name = "首顾科技" };
            LiteItemModel cyjy = new LiteItemModel { Id = 3, Name = "创业家园" };
            LiteItemModel ui = new LiteItemModel { Id = 4, Name = "美工" };
            LiteItemModel backed = new LiteItemModel { Id = 5, Name = "后台" };

            model.LinkedList.AddFirst(zyf);
            switch (tailId)
            {
                case 2: model.LinkedList.AddLast(sgkj); break;
                case 3: model.LinkedList.AddLast(cyjy); break;
                case 4: model.LinkedList.AddLast(sgkj); model.LinkedList.AddLast(ui); break;
                case 5: model.LinkedList.AddLast(sgkj); model.LinkedList.AddLast(backed); break;
                default: ; break;
            }
            model.TailedId = model.LinkedList.Last.Value.Id;

            return model;
        }
    }
}
