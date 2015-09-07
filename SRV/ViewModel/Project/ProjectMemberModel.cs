using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModel.Project
{
    public class ProjectMemberModel
    {
        public UserModel User { get; set; }
        public bool CanPublish { get; set; }
        public bool CanOwn { get; set; }
        public bool CanManage { get; set; }
    }
}
