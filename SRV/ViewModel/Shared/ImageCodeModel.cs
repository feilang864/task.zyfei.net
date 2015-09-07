using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel;
using FFLTask.SRV.ViewModel.Validations;

namespace FFLTask.SRV.ViewModel.Shared
{
    public class ImageCodeModel
    {
        [FflRequired]
        [DisplayName("验证码")]
        [FflStringLength(4, MinimumLength = 4)]
        public string InputImageCode { get; set; }

        public ImageCodeError ImageCodeError { get; set; }
    }

    public enum ImageCodeError
    {
        NoError = 0,
        Wrong = 1,
        Expired = 2
    }
}