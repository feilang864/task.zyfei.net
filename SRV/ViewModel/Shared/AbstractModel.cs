using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Account;

namespace FFLTask.SRV.ViewModel.Shared
{
    public class AbstractModel
    {
        public string Name { get; set; }
        public UserModel Founder { get; set; }
        public string Description { get; set; }
    }
}
