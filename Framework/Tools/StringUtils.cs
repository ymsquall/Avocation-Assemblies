using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Framework.Tools
{
    public partial class StringUtils
    {
        public static string Format(string format, params object[] ps)
        {
            string str = "";
            if (ps.Length > 0)
                str = string.Format(format, ps);
            else
                str = format;
            return str;
        }
        public static string Builder(byte[] strBuffer)
        {
            int lengthSize = 0;
            ushort strLength = BitConverter.ToUInt16(strBuffer, lengthSize);
            lengthSize += sizeof(ushort);
            string str = BitConverter.ToString(strBuffer, lengthSize, strBuffer.Length - strLength);
            return str;
        }
        //public static byte[] Serializer(string str)
        //{
        //    byte[] byteStr = System.Text.Encoding.UTF8.GetBytes(str);
        //    byte[] length = BitConverter.GetBytes((ushort)byteStr.Length);
        //    byte[] buffer = length.Concat(byteStr).ToArray();
        //    return buffer;
        //}
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
        public static bool ToArrayValueT<T>(string str, out T[] value, string s = "#")// where T : IConvertible
        {
            value = null;
            if (string.IsNullOrEmpty(str))
            {
                T v;
                List<T> list = new List<T>();
                string[] ss = str.Split(s.ToCharArray());
                for (int i = 0; i < ss.Length; ++i)
                {
                    if (!ToValueT(ss[i], out v))
                        return false;
                    list.Add(v);
                }
                value = list.ToArray();
            }
            return true;
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
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt64)
                {
                    UInt64 r = default(UInt64);
                    if (!UInt64.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Int32)
                {
                    Int32 r = default(Int32);
                    if (!Int32.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt32)
                {
                    UInt32 r = default(UInt32);
                    if (!UInt32.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Int16)
                {
                    Int16 r = default(Int16);
                    if (!Int16.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is UInt16)
                {
                    UInt16 r = default(UInt16);
                    if (!UInt16.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Single)
                {
                    Single r = default(Single);
                    if (!Single.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Double)
                {
                    Double r = default(Double);
                    if (!Double.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Char)
                {
                    Char r = default(Char);
                    if (!Char.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Boolean)
                {
                    Boolean r = default(Boolean);
                    if (!Boolean.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is Byte)
                {
                    Byte r = default(Byte);
                    if (!Byte.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else if (value is SByte)
                {
                    SByte r = default(SByte);
                    if (!SByte.TryParse(str, out r))
                    {
                        Console.WriteLine(string.Format("StringUtils.ToValueT failed, '{0}' not a {1} Type...", str, value.GetType().ToString()));
                        return false;
                    }
                    value = (T)(Object)r;
                }
                else
                {
                    Console.WriteLine(string.Format("StringUtils.ToValueT failed, is {0} Type...", value.GetType().ToString()));
                    return false;
                }
            }
            else if (null == str)
            {
                Console.WriteLine("StringUtils.ToValueT failed, is null str");
                return false;
            }
            //else if (str.Length == 0)
            //    UnityEngine.Debug.LogWarning("StringUtils.ToValueT warning, is empty str");
            return true;
        }
#if UNITY3D
        public static UnityEngine.Color ColorStr2U3DColor(string str)
        {
            UnityEngine.Color color = UnityEngine.Color.white;
            Regex regex = new Regex(@"([0-9A-Fa-f]{2})");
            MatchCollection results = regex.Matches(str);
            int index = 0;
            if (results.Count == 4)
                color.a = (float)int.Parse(results[index++].Value, System.Globalization.NumberStyles.HexNumber) / 255f;
            color.r = (float)int.Parse(results[index++].Value, System.Globalization.NumberStyles.HexNumber) / 255f;
            color.g = (float)int.Parse(results[index++].Value, System.Globalization.NumberStyles.HexNumber) / 255f;
            color.b = (float)int.Parse(results[index++].Value, System.Globalization.NumberStyles.HexNumber) / 255f;
            return color;
        }
        public static UnityEngine.Vector3 V3Str2U3DVector3(string str)
        {
            UnityEngine.Vector3 ret = UnityEngine.Vector3.zero;
            string[] v3s = str.Split(",".ToCharArray());
            if (v3s.Length < 2)
                return ret;
            if (!float.TryParse(v3s[0], out ret.x))
                return ret;
            if (!float.TryParse(v3s[1], out ret.y))
                return ret;
            if (v3s.Length == 3)
            {
                if (!float.TryParse(v3s[2], out ret.z))
                    return ret;
            }
            return ret;
        }
#endif
    }
}
