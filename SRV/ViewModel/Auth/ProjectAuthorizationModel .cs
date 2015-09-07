using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Project;
using FFLTask.SRV.ViewModel.Project;

namespace FFLTask.SRV.ViewModel.Auth
{
    public class ProjectAuthorizationModel 
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public IList<AuthorizationModel> Authorizations { get; set; }
    }
}
