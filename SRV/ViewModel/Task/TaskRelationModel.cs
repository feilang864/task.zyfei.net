using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Task
{
    public class TaskRelationModel
    {
        public LinkedList<LiteItemModel> Ancestor { get; set; }
        public LiteItemModel Current { get; set; }
        public IList<LiteItemModel> Brothers { get; set; }
        public IList<LiteItemModel> Children { get; set; }
    }
}
