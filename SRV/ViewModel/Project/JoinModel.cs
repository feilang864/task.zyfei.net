using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Project
{
    public class JoinModel
    {
        public IList<JoinModel> Children { get; set; }
        public FullItemModel Item { get; set; }
        public bool HasJoined { get; set; }
        public bool Selected { get; set; }
    }
}