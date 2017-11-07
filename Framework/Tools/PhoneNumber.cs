using System.Text.RegularExpressions;

namespace Framework.Tools
{
    public static class PhoneNumber
    {
        public static bool Check(int area, int num, ref bool isMobile)
        {
            return Check(string.Format("{0}-{1}", area, num), ref isMobile);
        }
        public static bool Check(RealWorldPhone phone, ref bool isMobile)
        {
            return Check(phone.ToString(), ref isMobile);
        }
        public static bool Check(string area, string num, ref bool isMobile)
        {
            return Check(string.Format("{0}-{1}", area, num), ref isMobile);
        }
        public static bool Check(string str, ref bool isMobile)
        {
            /**
            * 手机号码
            * 移动：134[0-8],135,136,137,138,139,150,151,157,158,159,182,187,188
            * 联通：130,131,132,152,155,156,185,186
            * 电信：133,1349,153,180,189
            */
            //string mobile = @"^1(3[0-9]|5[0-35-9]|8[025-9])\\d{8}$";
            /**
            10 * 中国移动：China Mobile
            11 * 134[0-8],135,136,137,138,139,150,151,157,158,159,182,187,188
            12 */
            //string cm = @"^1(34[0-8]|(3[5-9]|5[017-9]|8[278])\\d)\\d{7}$";
            /**
            15 * 中国联通：China Unicom
            16 * 130,131,132,152,155,156,185,186
            17 */
            //string cu = @"^1(3[0-2]|5[256]|8[56])\\d{8}$";
            /**
            20 * 中国电信：China Telecom
            21 * 133,1349,153,180,189
            22 */
            //string ct = @"^1((33|53|8[09])[0-9]|349)\\d{7}$";
            /**
            25 * 大陆地区固话及小灵通
            26 * 区号：010,020,021,022,023,024,025,027,028,029
            27 * 号码：七位或八位
            28 */
            //string phs = @"^0(10|2[0-5789]|\\d{3})\\d{7,8}$";
            string mobile = @"^(13[0-9]|15[012356789]|17[678]|18[0-9]|14[57])[0-9]{8}$";
            //string mobile = @"^[1]+[3,5,7,8,4]+\d{9}";
            string phs = @"\d{3}-\d{8}|\d{4}-\d{7}";
            // 评注：匹配形式如 0511 - 4405222 或 021 - 87888822
            bool ret = Regex.IsMatch(str, mobile);
            //ret |= Regex.IsMatch(str, cm);
            //ret |= Regex.IsMatch(str, cu);
            //ret |= Regex.IsMatch(str, ct);
            isMobile = ret;
            ret |= Regex.IsMatch(str, phs);
            return ret;
        }
    }
}
