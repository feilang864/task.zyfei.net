using System;
using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.SRV.ViewModel.Task
{
    public class _TimeSpanModel
    {
        const string DATETIME_GREATER_THAN_ERROR = "* 截止时间小于开始时间";

        [FflDateValidation]
        public DateTime? FromPublish { get; set; }

        [FflDateValidation]
        [GreaterThan("FromPublish", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToPublish { get; set; }

        [FflDateValidation]
        public DateTime? FromAssign { get; set; }

        [FflDateValidation]
        [GreaterThan("FromAssign", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToAssign { get; set; }

        [FflDateValidation]
        public DateTime? FromOwn { get; set; }

        [FflDateValidation]
        [GreaterThan("FromOwn", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToOwn { get; set; }

        [FflDateValidation]
        public DateTime? FromExpectComplete { get; set; }

        [FflDateValidation]
        [GreaterThan("FromExpectComplete", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToExpectComplete { get; set; }

        [FflDateValidation]
        public DateTime? FromActualComplete { get; set; }

        [FflDateValidation]
        [GreaterThan("FromActualComplete", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToActualComplete { get; set; }

        [FflDateValidation]
        public DateTime? FromLastestUpdateTime { get; set; }

        [FflDateValidation]
        [GreaterThan("FromLastestUpdateTime", ErrorMessage = DATETIME_GREATER_THAN_ERROR)]
        public DateTime? ToLastestUpdateTime { get; set; }
    }
}
