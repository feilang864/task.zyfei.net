using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace FFLTask.GLB.Global
{
    public class AddressHelper
    {
        public static string GetLiteral(string code)
        {

            if (code == "BJ") return "北京";
            if (code == "SH") return "上海";
            if (code == "TJ") return "天津";
            if (code == "JS") return "江苏";
            if (code == "ZJ") return "浙江";
            if (code == "AH") return "安徽";
            if (code == "HEB") return "河北";
            if (code == "SX") return "山西";
            if (code == "NMG") return "内蒙古";
            if (code == "LN") return "辽宁";
            if (code == "JL") return "吉林";
            if (code == "HLJ") return "黑龙江";
            if (code == "FJ") return "福建";
            if (code == "JX") return "江西";
            if (code == "SD") return "山东";
            if (code == "HEN") return "河南";
            if (code == "HUB") return "湖北";
            if (code == "HUN") return "湖南";
            if (code == "GD") return "广东";
            if (code == "GX") return "广西";
            if (code == "HAN") return "海南";
            if (code == "CQ") return "重庆";
            if (code == "SC") return "四川";
            if (code == "GZ") return "贵州";
            if (code == "YN") return "云南";
            if (code == "XZ") return "西藏";
            if (code == "SHX") return "陕西";
            if (code == "GS") return "甘肃";
            if (code == "QH") return "青海";
            if (code == "NX") return "宁夏";
            if (code == "XJ") return "新疆";

            throw new Exception(string.Format("the code for '{}' is not valid", code));

        }
    }
}
