using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace FFLTask.SRV.ViewModel.Shared
{
    public class PagerModel
    {
       
        public int SumOfItems { get; set; }

        public int PageSize { get; set; }

        public int PageIndex { get; set; }

        private int _rowSize;
        public int RowSize 
        {
            get
            {
                if (_rowSize == 0)
                {
                    _rowSize = 10;
                }
                return _rowSize;
            }
            set
            {
                value = RowSize;
            }
        }

        public int SumOfPage
        {
            get { return (SumOfItems - 1) / PageSize + 1; }
        }

        public string FormatUrl { get; set; }
    }
}