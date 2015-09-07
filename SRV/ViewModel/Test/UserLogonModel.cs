using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModel.Test
{
    public class UserLogonModel
    {
        public FakeUsers SelectedUserId { get; set; }
        public bool Remember { get; set; }
    }

    public enum FakeUsers
    {
        心晴 = 1,
        自由飞 = 2,
        叶子 = 3,
        科技改变生活 = 4,
        //拥抱世界 = 5,

        技术宅 = 6,
        //极客梦想 = 7,
        //美轮美奂 = 8,
        //张三 = 9,
        //李四 = 10
    }
}
