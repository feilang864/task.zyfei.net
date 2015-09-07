using System.Collections.Generic;

namespace FFLTask.SRV.ViewModel.Task
{
    public class _SumModel
    {
        public Dictionary<string, int> Priorities { get; set; }
        public Dictionary<string, int> Publish { get; set; }
        public Dictionary<string, int> Own { get; set; }
        public Dictionary<string, int> Accept { get; set; }
        public Dictionary<string, int> Types { get; set; }
        public Dictionary<string, int> Difficulties { get; set; }
        public Dictionary<string, int> ConsumeTime { get; set; }
        public Dictionary<string, int> Status { get; set; }
        public Dictionary<string, int> OverDue { get; set; }
        public Dictionary<string, int> Qualities { get; set; }
        public Dictionary<string, int> Doubt { get; set; }
        public Dictionary<string, int> RefuseAccept { get; set; }
    }
}
