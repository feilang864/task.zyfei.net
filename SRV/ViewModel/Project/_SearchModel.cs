using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Project
{
    public class _SearchModel
    {
        //TODO: should use enum
        public bool IdOrName { get; set; }
        public string Input { get; set; }
        public IList<_LiteralLinkedModel> Projects { get; set; }
    }
}
