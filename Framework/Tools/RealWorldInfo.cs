using System;
using System.Collections.Generic;

namespace Framework.Tools
{
    [Serializable]
    public static class RealWorldSiteList
    {
        static List<string> mCountryList = new List<string>();
        static List<string> mProvinceList = new List<string>();
        static List<string> mCityList = new List<string>();
        static List<string> mAreaList = new List<string>();
        public static void InitList(string[] countrys, string[] provinces, string[] citys, string[] areas)
        {
            if (mCountryList.Count <= 0)
                mCountryList.AddRange(countrys);
            if (mProvinceList.Count <= 0)
                mProvinceList.AddRange(provinces);
            if (mCityList.Count <= 0)
                mCityList.AddRange(citys);
            if (mAreaList.Count <= 0)
                mAreaList.AddRange(areas);
        }
        public static short IndexOfCountrys(string c)
        {
            return (short)mCountryList.IndexOf(c);
        }
        public static short IndexOfProvinces(string c)
        {
            return (short)mProvinceList.IndexOf(c);
        }
        public static short IndexOfCitys(string c)
        {
            return (short)mCityList.IndexOf(c);
        }
        public static short IndexOfAreas(string c)
        {
            return (short)mAreaList.IndexOf(c);
        }
        public static string CountryWithIndex(int idx)
        {
            if (idx < 0 || idx >= mCountryList.Count)
                return "";
            return mCountryList[idx];
        }
        public static string ProvinceWithIndex(int idx)
        {
            if (idx < 0 || idx >= mProvinceList.Count)
                return "";
            return mProvinceList[idx];
        }
        public static string CityWithIndex(int idx)
        {
            if (idx < 0 || idx >= mCityList.Count)
                return "";
            return mCityList[idx];
        }
        public static string AreaWithIndex(int idx)
        {
            if (idx < 0 || idx >= mAreaList.Count)
                return "";
            return mAreaList[idx];
        }
        public static string Combin(RealWorldSite site)
        {
            return string.Format("{0}|{1}|{2}|{3}|{4}", site.Country, site.Province, site.City, site.Area, site.Addr);
        }
        public static RealWorldSite Split(string s)
        {
            string[] ss = s.Split("|".ToCharArray());
            if (ss.Length != 5)
                return null;
            string errMsg = "";
            RealWorldSite ret = RealWorldSite.New(ref errMsg, IndexOfCountrys(ss[0]), IndexOfProvinces(ss[1]), IndexOfCitys(ss[2]), IndexOfAreas(ss[3]), ss[4]);
            return ret;
        }
    }
    [Serializable]
    public class RealWorldSite : ReflectionCreater<RealWorldSite>
    {
        public short mCountryIdx = -1;      // 国家
        public short mProvinceIdx = -1;     // 省/洲
        public short mCityIdx = -1;         // 市
        public short mAreaIdx = -1;         // 区/县
        public string mAddr = "";           // 地址
        public string Country { get { return RealWorldSiteList.CountryWithIndex(mCountryIdx); } }
        public string Province { get { return RealWorldSiteList.ProvinceWithIndex(mProvinceIdx); } }
        public string City { get { return RealWorldSiteList.CityWithIndex(mCityIdx); } }
        public string Area { get { return RealWorldSiteList.AreaWithIndex(mAreaIdx); } }
        public string Addr { get { return mAddr; } }
    }
    
    [Serializable]
    public static class RealWorldPhoneHelper
    {
        public static string Combin(RealWorldPhone phone)
        {
            return string.Format("{0}-{1}", phone.mAreaCode, phone.mPhoneNum);
        }
        public static RealWorldPhone Split(string s)
        {
            string[] ss = s.Split("-".ToCharArray());
            if (ss.Length != 2)
                return null;
            string errMsg = "";
            ushort areaCode = 0;
            int phone = 0;
            if (!ushort.TryParse(ss[0], out areaCode))
                return null;
            if (!int.TryParse(ss[1], out phone))
                return null;
            RealWorldPhone ret = RealWorldPhone.New(ref errMsg, areaCode, phone);
            return ret;
        }
    }
    [Serializable]
    public class RealWorldPhone : ReflectionCreater<RealWorldPhone>
    {
        public ushort mAreaCode = 0;    // 区号
        public int mPhoneNum = 0;       // 电话号

        public override string ToString()
        {
            if (mAreaCode != 0)
                return string.Format("{0}-{1}", mAreaCode, mPhoneNum);
            return mPhoneNum.ToString();
        }
    }
}
