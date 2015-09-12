using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.SRV.ViewModel.User
{
    public class SummaryModel
    {
        public bool IsCurrentUser { get; set; }
        public ProfileModel Profile { get; set; }
        public IEnumerable<JoinedProjectItemModel> JoinedProjects { get; set; }
    }

    public class JoinedProjectItemModel
    {
        public int ProjectId { get; set; }
        public bool IsPublisher { get; set; }
        public bool IsOwner { get; set; }
        public bool IsAdmin { get; set; }
    }
}
