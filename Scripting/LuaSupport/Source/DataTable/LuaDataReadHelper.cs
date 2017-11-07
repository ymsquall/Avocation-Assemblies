using System;
using System.Collections.Generic;
using Scripting.UniLua;
using Framework.Tools;

namespace Scripting.LuaSupport.DataTable
{
    using Shared;
    using UnityEngine;
    using ItemBaseConst = TwoData<int, byte>;
    public class LuaDataReadHelper
    {
        public static bool ReadVector3(LuaTable tbl, string fieldName, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            TValue val;
            if (!LuaReadHelper.GetTValue2String(tbl, fieldName, out val))
                return false;
            return ReadVector3(val, out value);
        }
        public static bool ReadVector3_Tbl(LuaTable tbl, string fieldName, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            LuaTable val;
            if (!LuaReadHelper.GetTValue2Table(tbl, fieldName, out val))
                return false;
            return ReadVector3(val, out value);
        }
        public static bool ReadTransform_Tbl(LuaTable tbl, string fieldName, out UnityEngine.Vector3 pos, out UnityEngine.Vector3 euler, out UnityEngine.Vector3 scale)
        {
            pos = euler = UnityEngine.Vector3.zero;
            scale = UnityEngine.Vector3.one;
            LuaTable val;
            if (!LuaReadHelper.GetTValue2Table(tbl, fieldName, out val))
                return false;
            if (val.ArrayPart.Length != 9)
                return false;
            pos.x = (float)val.ArrayPart[0].V.NValue;
            pos.y = (float)val.ArrayPart[1].V.NValue;
            pos.z = (float)val.ArrayPart[2].V.NValue;
            euler.x = (float)val.ArrayPart[3].V.NValue;
            euler.y = (float)val.ArrayPart[4].V.NValue;
            euler.z = (float)val.ArrayPart[5].V.NValue;
            scale.x = (float)val.ArrayPart[6].V.NValue;
            scale.y = (float)val.ArrayPart[7].V.NValue;
            scale.z = (float)val.ArrayPart[8].V.NValue;
            return true;
        }
        public static bool ReadNumberArray_Tbl<T>(LuaTable tbl, string fieldName, out T[] value)
        {
            value = null;
            LuaTable val;
            if (!LuaReadHelper.GetTValue2Table(tbl, fieldName, out val))
                return false;
            if (val.ArrayPart.Length <= 0)
            {
                value = new T[0];
                return true;
            }
            value = new T[val.ArrayPart.Length];
            for (int i = 0; i < val.ArrayPart.Length; ++i)
            {
                value[i] = (T)Convert.ChangeType(val.ArrayPart[i].V.NValue, typeof(T));
            }
            return true;
        }
        public static bool ReadVector3(TValue v, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            float[] number = null;

            string numStr = ReadUtf8String(v);
            string sp = ",";
            string[] numStrs = numStr.Split(sp.ToCharArray());
            if (numStrs.Length == 3)
            {
                number = new float[3];
                if (!float.TryParse(numStrs[0], out number[0]))
                    return false;
                if (!float.TryParse(numStrs[1], out number[1]))
                    return false;
                if (!float.TryParse(numStrs[2], out number[2]))
                    return false;
            }
            else
            {
                number = ReadNumberArray(v, out number);
                if (number.Length != 3)
                    return false;
            }
            value = new UnityEngine.Vector3(number[0], number[1], number[2]);
            return true;
        }
        public static bool ReadVector3(LuaTable v, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            if (v.ArrayPart.Length != 3)
                return false;
            value.x = (float)v.ArrayPart[0].V.NValue;
            value.y = (float)v.ArrayPart[1].V.NValue;
            value.z = (float)v.ArrayPart[2].V.NValue;
            return true;
        }
        public static string ReadUtf8String(TValue v)
        {
            string utf8 = StringUtils.UTF8Decode(v.OValue as string);
            if (null == utf8)
                return "";
            return utf8;
        }
        public static string[] ReadStringArray(TValue v)
        {
            string[] strArray = null;
            if (v.TtIsString())
            {
                string partName = StringUtils.UTF8Decode(v.OValue as string);
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
                            tmpList.Add(StringUtils.UTF8Decode(partStr));
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
            if (!LuaDataReadHelper.ReadNumber(v, out temp))
                return false;
            value = (float)temp * 0.001f;
            return true;
        }
        public static bool ReadPixel2U3DNumber(TValue v, out float value)
        {
            value = 0.0f;
            System.Int32 temp = 0;
            if (!LuaDataReadHelper.ReadNumber(v, out temp))
                return false;
            value = SharedGlobalParams.PixelUnit2U3D(temp);
            return true;
        }
        public static bool ReadNumber<T>(TValue v, out T value)// where T : IConvertible
        {
            value = default(T);
            StringUtils.ToValueT(v.OValue as string, out value);
            return true;
        }
        public static bool ReadU3DVector3(TValue v, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            float[] number = null;

            string numStr = ReadUtf8String(v);
            string sp = ",";
            string[] numStrs = numStr.Split(sp.ToCharArray());
            if (numStrs.Length == 3)
            {
                number = new float[3];
                if (!float.TryParse(numStrs[0], out number[0]))
                    return false;
                if (!float.TryParse(numStrs[1], out number[1]))
                    return false;
                if (!float.TryParse(numStrs[2], out number[2]))
                    return false;
            }
            else
            {
                number = ReadNumberArray(v, out number);
                if (number.Length != 3)
                    return false;
            }
            value = new UnityEngine.Vector3(number[0], number[1], number[2]);
            return true;
        }
        public static bool ReadPixel2U3DVector3(TValue v, out UnityEngine.Vector3 value)
        {
            value = UnityEngine.Vector3.zero;
            float[] number = null;

            string numStr = ReadUtf8String(v);
            string sp = ",";
            string[] numStrs = numStr.Split(sp.ToCharArray());
            if (numStrs.Length == 3)
            {
                number = new float[3];
                if (!float.TryParse(numStrs[0], out number[0]))
                    return false;
                if (!float.TryParse(numStrs[1], out number[1]))
                    return false;
                if (!float.TryParse(numStrs[2], out number[2]))
                    return false;
            }
            else
            {
                number = ReadNumberArray(v, out number);
                if (number.Length != 3)
                    return false;
            }
            value = new UnityEngine.Vector3(SharedGlobalParams.PixelUnit2U3D(number[0]),
                                    SharedGlobalParams.PixelUnit2U3D(number[1]), 
                                    SharedGlobalParams.PixelUnit2U3D(number[2]));
            return true;
        }
        public static bool ReadU3DColor255(TValue v, out UnityEngine.Color value)
        {
            value = UnityEngine.Color.white;
            float[] number = null;

            string numStr = ReadUtf8String(v);
            string sp = ",";
            string[] numStrs = numStr.Split(sp.ToCharArray());
            if (numStrs.Length == 3 || numStrs.Length == 4)
            {
                number = new float[4];
                if (!float.TryParse(numStrs[0], out number[0]))
                    return false;
                if (!float.TryParse(numStrs[1], out number[1]))
                    return false;
                if (!float.TryParse(numStrs[2], out number[2]))
                    return false;
                if (numStrs.Length == 4)
                {
                    if (!float.TryParse(numStrs[3], out number[3]))
                        return false;
                }
                else
                    number[3] = 255.0f;
                value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, number[3] / 255.0f);
            }
            else
            {
                number = ReadNumberArray(v, out number);
                if (number.Length == 3)
                    value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, 255.0f);
                else if (number.Length == 4)
                    value = new UnityEngine.Color(number[0] / 255.0f, number[1] / 255.0f, number[2] / 255.0f, number[3] / 255.0f);
                else return false;
            }
            return true;
        }
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
                StringUtils.ToValueT(strArray[i], out val);
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
                StringUtils.ToValueT(strArray[i], out val);
                array.Add(val);
            }
            value = array.ToArray();
            return value;
        }
        public static bool ReadPropData(TValue v, out TwoData<Byte, Double> value)
        {
            value = null;
            string numStr = ReadUtf8String(v);
            string[] ss = numStr.Split(":".ToCharArray());
            if (ss.Length <= 0)
                return true;
            if (ss.Length != 2)
                return true;
            Byte id = 0;
            Double val = 0;
            if (!StringUtils.ToValueT(ss[0], out id))
                return false;
            if (!StringUtils.ToValueT(ss[1], out val))
                return false;
            value = new TwoData<byte, double>(id, val);
            return true;
        }
        public static bool ReadPropDataArray(TValue v, out TwoData<Byte, Double>[] value)
        {
            value = null;
            string[] strArray = ReadStringArray(v);
            List<TwoData<Byte, Double>> array = new List<TwoData<byte, double>>();
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                string[] ss = strArray[i].Split(":".ToCharArray());
                if (ss.Length <= 0)
                    continue;
                if (ss.Length != 2)
                    continue;
                if (string.IsNullOrEmpty(ss[0]))
                    continue;
                if (string.IsNullOrEmpty(ss[1]))
                    continue;
                Byte id = 0;
                Double val = 0;
                if (!StringUtils.ToValueT(ss[0], out id))
                    return false;
                if (!StringUtils.ToValueT(ss[1], out val))
                    return false;
                array.Add(new TwoData<byte, double>(id, val));
            }
            value = array.ToArray();
            return true;
        }
        public static bool ReadItemConstData(TValue v, out ItemBaseConst value)
        {
            value = null;
            string numStr = ReadUtf8String(v);
            string[] ss = numStr.Split(":".ToCharArray());
            if (ss.Length <= 0)
                return true;
            if (ss.Length != 2)
                return true;
            Int32 id = 0;
            Byte num = 0;
            if (!StringUtils.ToValueT(ss[0], out id))
                return false;
            if (!StringUtils.ToValueT(ss[1], out num))
                return false;
            value = new ItemBaseConst(id, num);
            return true;
        }
        public static bool ReadItemConstArray(TValue v, out ItemBaseConst[] value)
        {
            value = null;
            string[] strArray = ReadStringArray(v);
            List<ItemBaseConst> array = new List<ItemBaseConst>();
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                string[] ss = strArray[i].Split(":".ToCharArray());
                if (ss.Length <= 0)
                    continue;
                if (ss.Length != 2)
                    continue;
                Int32 id = 0;
                Byte num = 0;
                if (!StringUtils.ToValueT(ss[0], out id))
                    return false;
                if (!StringUtils.ToValueT(ss[1], out num))
                    return false;
                array.Add(new ItemBaseConst(id, num));
            }
            value = array.ToArray();
            return true;
        }
        public static bool ReadNumber3Value<T1, T2, T3>(TValue v, out ThreeData<T1, T2, T3> value)
        //    where T1 : IConvertible
        //    where T2 : IConvertible
        //    where T3 : IConvertible
        {
            value = null;
            string numStr = ReadUtf8String(v);
            string[] ss = numStr.Split(":".ToCharArray());
            if (ss.Length <= 0)
                return true;
            if (ss.Length != 3)
                return true;
            T1 p1 = default(T1);
            T2 p2 = default(T2);
            T3 p3 = default(T3);
            if (!StringUtils.ToValueT(ss[0], out p1))
                return false;
            if (!StringUtils.ToValueT(ss[1], out p2))
                return false;
            if (!StringUtils.ToValueT(ss[2], out p3))
                return false;
            value = new ThreeData<T1, T2, T3>(p1, p2, p3);
            return true;
        }
        public static bool ReadNumber3Array<T1, T2, T3>(TValue v, out ThreeData<T1, T2, T3>[] value)
        //    where T1 : IConvertible
        //    where T2 : IConvertible
        //    where T3 : IConvertible
        {
            value = null;
            string[] strArray = ReadStringArray(v);
            List<ThreeData<T1, T2, T3>> array = new List<ThreeData<T1, T2, T3>>();
            for (int i = 0; i < strArray.Length; ++i)
            {
                if (string.IsNullOrEmpty(strArray[i]))
                    continue;
                string[] ss = strArray[i].Split(":".ToCharArray());
                if (ss.Length <= 0)
                    continue;
                if (ss.Length != 3)
                    continue;
                T1 p1 = default(T1);
                T2 p2 = default(T2);
                T3 p3 = default(T3);
                if (!StringUtils.ToValueT(ss[0], out p1))
                    return false;
                if (!StringUtils.ToValueT(ss[1], out p2))
                    return false;
                if (!StringUtils.ToValueT(ss[2], out p3))
                    return false;
                array.Add(new ThreeData<T1, T2, T3>(p1, p2, p3));
            }
            value = array.ToArray();
            return true;
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
        public static Vector3 GetLuaVector3(ILuaState lua)
        {
            if (!lua.IsTable(-1)) return new Vector3(0, 0, 0);
            //往栈里面压入一个key:name
            lua.PushString("x");
            //取得-2位置的table，然后把栈顶元素弹出，取出table[name]的值并压入栈
            lua.GetTable(-2);
            //输出栈顶的name
            float x = (float)lua.ToNumber(-1);
            //把栈顶元素弹出去
            lua.Pop(1);
            lua.PushString("y");
            lua.GetTable(-2);
            float y = (float)lua.ToNumber(-1);
            lua.Pop(1);
            lua.PushString("z");
            lua.GetTable(-2);
            float z = (float)lua.ToNumber(-1);
            lua.Pop(1);
            return new Vector3(x, y, z);
        }
    }
}
