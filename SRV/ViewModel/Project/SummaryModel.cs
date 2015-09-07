using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Shared;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.ViewModel.Project
{
    public class SummaryModel
    {
        public AbstractModel Abstract { get; set; }
        public IList<FullItemModel> Projects { get; set; }
        public IList<AuthorizationModel> Authorizations { get; set; }
    }

    public class AuthorizationModel
    {
        public virtual int Id { get; set; }

        public virtual UserModel User { get; set; }
        public virtual bool CanAdmin { get; set; }
        public virtual bool CanPublish { get; set; }
        public virtual bool CanOwn { get; set; }
        public virtual bool IsEdit { get; set; }
    }
}
