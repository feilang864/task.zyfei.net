using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Project
{
    public class _DropdownlistLinkedModel
    {
        public LinkedList<_DropdownlistLinkedNodeModel> LinkedProject { get; set; }

        /// <summary>
        /// this is the only point to retrieve the current selected project
        /// </summary>
        public LiteItemModel TailSelectedProject { get; set; }
        public bool SelectedProjectHasChild { get; set; }
    }
}
