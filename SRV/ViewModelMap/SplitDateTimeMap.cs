using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FFLTask.SRV.ViewModel.Shared;
using Global.Core.ExtensionMethod;

namespace FFLTask.SRV.ViewModelMap
{
    public static class SplitDateTimeMap
    {
        public static DateTime? Combine(this SplitDateTimeModel model)
        {
            if (model != null && model.IsValid())
            {
                return Convert.ToDateTime(model.Date)
                    .AddHours(model.Hour)
                    .AddMinutes(model.Minute);
            }
            return null;
        }

        public static SplitDateTimeModel Split(this DateTime? time)
        {
            if (time.HasValue)
            {
                return new SplitDateTimeModel
                {
                    Date = time.Value.ToChineseDate(),
                    Hour = time.Value.Hour,
                    Minute = time.Value.Minute
                };
            }
            else
            {
                return new SplitDateTimeModel();
            }

        }

        public static bool IsValid(this SplitDateTimeModel model)
        {
            if (string.IsNullOrEmpty(model.Date) && (model.Hour > 0 || model.Minute > 0))
            {
                //TODO: 
                throw new Exception("date is null but hours/minute is not 0 in SplitDateTimeModel");
            }
            return !string.IsNullOrEmpty(model.Date);
        }
    }
}
