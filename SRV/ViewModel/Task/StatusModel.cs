using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.SRV.ViewModel.Task
{
    public class StatusModel
    {
        public int Stage { get; set; }
        public string Name { get; set; }
        public bool Checked { get; set; }
    }
    public enum FakeStatus
    {
        发布 = 1,
        分配 = 2,
        承接 = 3,
        //暂停 = 4,
        完成 = 5,
        //取消 = 6,
        验收通过 = 7,
        验收失败 = 8
    }
}
