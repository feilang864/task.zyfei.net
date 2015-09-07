using System;
using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Project
{
    public class FullItemModel
    {
        public string Description { get; set; }
        public LiteItemModel LiteItem { get; set; }

        public DateTime CreatedTime { get; set; }
        public IList<FullItemModel> Children { get; set; }
    }
}
