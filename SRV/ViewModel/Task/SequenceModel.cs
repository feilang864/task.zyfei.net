using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Task
{
    public class SequenceModel
    {
        public LiteItemModel Parent { get; set; }
        public int[] SelectedChildrenSequences { get; set; }
        public IList<LiteItemModel> Children { get; set; }
    }
}
