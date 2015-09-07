using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Task;
using FFLTask.SRV.ViewModel.Account;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Validations;
using System.ComponentModel;

namespace FFLTask.SRV.ViewModel.Team
{
    public class TransferModel
    {
        public int LeaverId { get; set; }
        public int ProjectId { get; set; }
        public Role CurrentRole { get; set; }
        public IList<TransferItemModel> Tasks { get; set; }
        public IList<Status?> AllStatus { get; set; }
        public Status? SelectedStatus { get; set; }
        public UserModel Predecessor { get; set; }

        [FflRequired]
        [DisplayName("接手人")]
        public string SuccessorName { get; set; }
    }

    public class TransferItemModel
    {
        public int Id { get; set; }
        public string Title { get; set; }
        public Status CurrentStatus { get; set; }

        public bool Selected { get; set; }
    }

}
