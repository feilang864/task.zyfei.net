using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Validations;
using System.ComponentModel.DataAnnotations;
using FFLTask.SRV.ViewModel.Shared;

namespace FFLTask.SRV.ViewModel.Project
{
    public class CreateModel
    {
        [FflRequired]
        [Display(Name = "名称")]
        public string Name { get; set; }

        [FflStringLength(256)]
        [Display(Name = "简介")]
        public string Introduction { get; set; }

        public bool Continue { get; set; }

        public IList<int> Parents { get; set; }
        public int? SelectedParent { get; set; }
    }
}
