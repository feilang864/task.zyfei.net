using System.ComponentModel;
using FFLTask.GLB.Global.Enum;
using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.SRV.ViewModel.Task
{
    public class LiteItemModel
    {
        public int? Id { get; set; }
        public int Sequence { get; set; }

        [FflRequired]
        [DisplayName("标题")]
        [FflStringLength(255)]
        public string Title { get; set; }

        public StatusModel CurrentStatus { get; set; }

        public bool Virtual { get; set; }
        public bool HasChild { get; set; }
        public NodeType NodeType { get; set; }
    }
}
