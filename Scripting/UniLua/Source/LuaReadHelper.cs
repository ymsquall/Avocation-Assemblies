using System;
using System.Collections.Generic;
using System.Text;

namespace Scripting.UniLua
{
    public class LuaReadHelper
    {
        public static bool StkIDIsString(StkId stkID)
        {
            if ((null == stkID) || (default(TValue) == stkID.V))
                return false;
            if (!stkID.V.TtIsString())
                return false;
            return true;
        }
        public static bool StkIDIsNumber(StkId stkID)
        {
            if ((null == stkID) || (default(TValue) == stkID.V))
                return false;
            if (!stkID.V.TtIsNumber())
                return false;
            return true;
        }
        public static bool StkIDIsTable(StkId stkID)
        {
            if ((null == stkID) || (default(TValue) == stkID.V))
                return false;
            if (!stkID.V.TtIsTable())
                return false;
            return true;
        }
        public static bool GetTValue2String(LuaTable tbl, string fieldName, out TValue val)
        {
            val = default(TValue);
            TValue tvk = new TValue();
            tvk.SetSValue(fieldName);
            StkId hKeyID = tbl.Get(ref tvk);
            if (!StkIDIsString(hKeyID))
                return false;
            val = hKeyID.V;
            return true;
        }
        public static bool GetTValue2Number<T>(LuaTable tbl, string fieldName, out T val)
        {
            val = default(T);
            TValue tvk = new TValue();
            tvk.SetSValue(fieldName);
            StkId hKeyID = tbl.Get(ref tvk);
            if (!StkIDIsNumber(hKeyID))
                return false;
            val = (T)Convert.ChangeType(hKeyID.V.NValue, typeof(T));
            return true;
        }
        public static bool GetTValue2Table(LuaTable tbl, string fieldName, out LuaTable val)
        {
            val = null;
            TValue tvk = new TValue();
            tvk.SetSValue(fieldName);
            StkId hKeyID = tbl.Get(ref tvk);
            if (!StkIDIsTable(hKeyID))
                return false;
            if (null == hKeyID.V.OValue)
                return false;
            val = hKeyID.V.OValue as LuaTable;
            return true;
        }
        public static LuaTable GetSubTable(LuaTable tbl, string fieldName)
        {
            LuaTable val;
            if (!GetTValue2Table(tbl, fieldName, out val))
                return null;
            return val;
        }
        public static Dictionary<string, int> ReadHeaderKey(LuaTable tbl, string fieldName)
        {
            LuaTable hTbl = GetSubTable(tbl, fieldName);
            if (null == hTbl)
                return null;
            Dictionary<string, int> dict = new Dictionary<string, int>();
            for (int i = 0; i < hTbl.HashPart.Length; ++i)
            {
                LuaTable.HNode node = hTbl.HashPart[i];
                string key = node.Key.V.OValue as string;
                key = UTF8Decode(key);
                if(null != key)
                    dict.Add(key, (int)node.Val.V.NValue);
            }
            return dict;
        }
        public static string ReadUtf8String(LuaTable tbl, string fieldName)
        {
            TValue val;
            if (!GetTValue2String(tbl, fieldName, out val))
                return null;
            return ReadUtf8String(val);
        }
        public static string ReadUtf8String(TValue v)
        {
            string utf8 = UTF8Decode(v.OValue as string);
            if (null == utf8)
                return "";
            return utf8;
        }
        public static string[] ReadStringArray(TValue v)
        {
            string[] strArray = null;
            if (v.TtIsString())
            {
                string partName = UTF8Decode(v.OValue as string);
                if (string.IsNullOrEmpty(partName))
                    strArray = new string[] { };
                else
                    strArray = new string[] { partName };
            }
            else
            {
                LuaTable psTbl = v.OValue as LuaTable;
                if (psTbl.ArrayPart.Length > 0)
                {
                    List<string> tmpList = new List<string>();
                    for (int i = 0; i < psTbl.ArrayPart.Length; ++i)
                    {
                        if (!psTbl.ArrayPart[i].V.TtIsString())
                            continue;
                        string partStr = psTbl.ArrayPart[i].V.OValue as string;
                        if (!string.IsNullOrEmpty(partStr))
                            tmpList.Add(UTF8Decode(partStr));
                    }
                    strArray = tmpList.ToArray();
                }
            }
            return strArray;
        }
        public static bool ReadNumber2Float_MS(TValue v, out float value)
        {
            value = 0.0f;
            System.Int32 temp = 0;
            if (!ReadNumber(v, out temp))
                return false;
            value = (float)temp * 0.001f;
            return true;
        }
        public static bool ReadNumber<T>(LuaTable tbl, string fieldName, out T value)// where T : IConvertible
        {
            value = default(T);
            TValue val;
            if (GetTValue2Number(tbl, fieldName, out value))
                return true;
            if (!GetTValue2String(tbl, fieldName, out val))
                return false;
            return ReadNumber<T>(val, out value);
        }
        public static bool ReadNumber<T>(TValue v, out T value)// where T : IConvertible
        {
            value = default(T);
            ToValueT(v.OValue as string, out value);
            return true;
        }
        //public static bool ReadU3DColor255(TValue v, out UnityEngine.Color value)
        //{
        //    value = UnityEngine.Color.white;
        //    float[] number = null;

        //    string numStr = ReadUtf8String(v);
        //    string sp = ",";
        //    string[] numStrs = numStr.Split(sp.ToCharArray());
        //    if (numStrs.Length == 3 || numStrs.Length == 4)
        //    {
        //        number = new float[4];
        //        if (!float.TryParse(numStrs[0], out number[0]))
        //            return false;
        //        if (!float.TryParse(numStrs[1], out number[1]))
        //            return false;
        //        if (!float.TryParse(numStrs[2], out number[2]))
        //            return false;
        //        if (numStrs.Length == 4)
        //        {
        //            if (!float.TryParse(numStrs[3], out number[3]))
        //                return false;
        //        }
        //        else
        //            number[3] = 255.0f;
        //        value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, number[3] / 255.0f);
        //    }
        //    else
        //    {
        //        number = ReadNumberArray(v, out number);
        //        if (number.Length == 3)
        //            value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, 255.0f);
        //        else if (number.Length == 4)
        //            value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, number[3] / 255.0f);
        //        else return false;
        //    }
        //    return true;
        //}
        public static T[] ReadNumberArray<T>(TValue v, string tag, out T[] value)// where T : IConvertible
        {
            value = default(T[]);
            string str = ReadUtf8String(v);
            string[] strArray = str.Split(tag.ToCharArray());
            List<T> array = new List<T>();
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                T val = default(T);
                ToValueT(strArray[i], out val);
                array.Add(val);
            }
            value = array.ToArray();
            return value;
        }
        public static T[] ReadNumberArray<T>(TValue v, out T[] value)// where T : IConvertible
        {
            value = default(T[]);
            string[] strArray = ReadStringArray(v);
            List<T> array = new List<T>();
            for (int i = 0; i < strArray.Length; ++ i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                T val = default(T);
                ToValueT(strArray[i], out val);
                array.Add(val);
            }
            value = array.ToArray();
            return value;
        }
        public static List<T> ReadEnumList<T>(TValue v)
        //    where T : IConvertible
        {
            List<T> array = new List<T>();
            string[] strArray = ReadStringArray(v);
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                int val = 0;
                if (!int.TryParse(strArray[i], out val))
                    continue;
                T ev = (T)(object)val;
                if (!Enum.IsDefined(typeof(T), ev.ToString()))
                    continue;
                array.Add(ev);
            }
            return array;
        }
        public static string UTF8Encode(string s)
        {
            var bytes = Encoding.UTF8.GetBytes(s);
            var sb = new StringBuilder();
            for (var i = 0; i < bytes.Length; ++i)
            {
                sb.Append((char)bytes[i]);
            }
            return sb.ToString();
        }
        public static string UTF8Decode(string s)
        {
            if (null == s)
                return null;
            var bytes = new Byte[s.Length];
            for (int i = 0; i < s.Length; ++i)
            {
                bytes[i] = (byte)s[i];
            }
            return Encoding.UTF8.GetString(bytes);
        }
        public static bool ToValueT<T>(string str, out T value)// where T : IConvertible
        {
            value = default(T);
            if (null != str && str.Length > 0)
            {
                if (value is Int64)
                {
                    Int64 r = default(Int64);
                    if (!Int64.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt64)
                {
                    UInt64 r = default(UInt64);
                    if (!UInt64.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Int32)
                {
                    Int32 r = default(Int32);
                    if (!Int32.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt32)
                {
                    UInt32 r = default(UInt32);
                    if (!UInt32.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Int16)
                {
                    Int16 r = default(Int16);
                    if (!Int16.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt16)
                {
                    UInt16 r = default(UInt16);
                    if (!UInt16.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Single)
                {
                    Single r = default(Single);
                    if (!Single.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Double)
                {
                    Double r = default(Double);
                    if (!Double.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Char)
                {
                    Char r = default(Char);
                    if (!Char.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Boolean)
                {
                    Boolean r = default(Boolean);
                    if (!Boolean.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Byte)
                {
                    Byte r = default(Byte);
                    if (!Byte.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is SByte)
                {
                    SByte r = default(SByte);
                    if (!SByte.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else
                {
                    Console.WriteLine(string.Format("ToValueT failed, is {0} Type...", value.GetType().ToString()));
                    return false;
                }
            }
            else if (null == str)
            {
                Console.WriteLine("ToValueT failed, is null str");
                return false;
            }
            //else if (str.Length == 0)
            //    UnityEngine.Debug.LogWarning("ToValueT warning, is empty str");
            return true;
        }
    }
}
