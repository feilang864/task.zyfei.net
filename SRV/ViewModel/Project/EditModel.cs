using System.ComponentModel.DataAnnotations;
using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.SRV.ViewModel.Project
{
    public class EditModel
    {
        public int Id { get; set; }

        [FflRequired]
        [FflStringLength(12)]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [FflStringLength(255)]
        [Display(Name = "描述")]
        public string Description { get; set; }

        public int? ParentId { get; set; }
    }
}
