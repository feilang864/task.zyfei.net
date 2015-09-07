using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.SRV.ViewModel.Shared
{
    public class SplitDateTimeModel
    {
        [FflDateValidation]
        public string Date { get; set; }
        public int Hour { get; set; }
        public int Minute { get; set; }
    }
}
