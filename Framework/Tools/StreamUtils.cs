using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Framework.Tools
{
    public interface IFieldReader
    {
        void Read(Stream s);
    }
    public interface IFieldWriter
    {
        void Write(Stream s);
    }
    public interface IFieldReaderWriter : IFieldReader, IFieldWriter
    {
    }
    public interface IArrayReader
    {
        void ReadFields(Stream s);
    }
    public interface IArrayWriter
    {
        void WriteFields(Stream s);
    }
    public interface IArrayReaderWrite : IArrayReader, IArrayWriter
    {
    }
    public class StreamUtils
    {
        private int SwapInt32(int int32)
        {
            return (int32 & 0xFF) << 24 | (int32 >> 8 & 0xFF) << 16 |
                (int32 >> 16 & 0xFF) << 8 | (int32 >> 24 & 0xFF);
        }

        static byte[] ReadBytes(Stream s, int count)
        {
            byte[] buffer = new byte[count];
            int l = s.Read(buffer, 0, count);
            if (l != count)
                Console.WriteLine("StreamUtils.ReadBytes failed...");
            return buffer;
        }
        public static String Read(Stream s, ref String value, params int[] count)
        {
            Byte[] strBuf = null;
            if (null != count && count.Length > 0 && count[0] > 0)
            {
                strBuf = new Byte[count[0]];
                s.Read(strBuf, 0, count[0]);
            }
            else
            {
                strBuf = new Byte[sizeof(ushort)];
                s.Read(strBuf, 0, strBuf.Length);
                // 高低位交换
                //Array.Reverse(strBuf);
                ushort strLength = BitConverter.ToUInt16(strBuf, 0);
                strBuf = new Byte[strLength];
                s.Read(strBuf, 0, strBuf.Length);
            }
            value = Encoding.UTF8.GetString(strBuf, 0, strBuf.Length);
            return value;
        }
        public static void Read(Stream s, out List<String> values, params int[] count)
        {
            Byte[] strBuf = null;
            strBuf = new Byte[sizeof(ushort)];
            s.Read(strBuf, 0, strBuf.Length);
            // 高低位交换
            //Array.Reverse(strBuf);
            ushort strNum = BitConverter.ToUInt16(strBuf, 0);
            values = new List<string>(strNum);
            for (ushort i = 0; i < strNum; ++i)
            {
                string value = "";
                Read(s, ref value, count);
                values[i] = value;
            }
        }
        public static void Read(Stream s, out String[] values, params int[] count)
        {
            Byte[] strBuf = null;
            strBuf = new Byte[sizeof(ushort)];
            s.Read(strBuf, 0, strBuf.Length);
            // 高低位交换
            //Array.Reverse(strBuf);
            ushort strNum = BitConverter.ToUInt16(strBuf, 0);
            values = new string[strNum];
            for (ushort i = 0; i < strNum; ++i)
            {
                string value = "";
                Read(s, ref value, count);
                values[i] = value;
            }
        }
        public static Byte[] Read(Stream s, Byte[] value = null, params int[] count)
        {
            Byte[] buf = null;
            if (null != count && count.Length > 0 && count[0] > 0)
            {
                buf = new Byte[count[0]];
                s.Read(buf, 0, count[0]);
            }
            else
            {
                buf = new Byte[sizeof(ushort)];
                s.Read(buf, 0, buf.Length);
                // 高低位交换
                //Array.Reverse(buf);
                ushort strLength = BitConverter.ToUInt16(buf, 0);
                buf = new Byte[strLength];
                s.Read(buf, 0, buf.Length);
            }
            value = buf;
            return value;
        }
        public static Byte Read(Stream s, Byte value)
        {
            Byte[] buf = new Byte[1];
            s.Read(buf, 0, 1);
            value = buf[0];
            return value;
        }
        public static Byte[] ReadByteArray4(Stream s, out Byte[] value)
        {
            int len = 0;
            Read(s, ref len);
            value = ReadBytes(s, len);
            return value;
        }
        public static T Read<T>(Stream s, ref T value)
        {
            if (null == value)
            {
                Console.WriteLine(string.Format("StreamUtils.Read<Number> failed, value is null"));
                return value;
            }
            if (value is IFieldReader)
            {
                (value as IFieldReader).Read(s);
                return value;
            }
            int len = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            byte[] buffer = ReadBytes(s, len);
            // 高低位交换
            //Array.Reverse(buffer);
            if (value is Int64)
                value = (T)(Object)BitConverter.ToInt64(buffer, 0);
            else if (value is UInt64)
                value = (T)(Object)BitConverter.ToUInt64(buffer, 0);
            else if (value is Int32)
                value = (T)(Object)BitConverter.ToInt32(buffer, 0);
            else if (value is UInt32)
                value = (T)(Object)BitConverter.ToUInt32(buffer, 0);
            else if (value is Int16)
                value = (T)(Object)BitConverter.ToInt16(buffer, 0);
            else if (value is UInt16)
                value = (T)(Object)BitConverter.ToUInt16(buffer, 0);
            else if (value is Single)
                value = (T)(Object)BitConverter.ToSingle(buffer, 0);
            else if (value is Double)
                value = (T)(Object)BitConverter.ToDouble(buffer, 0);
            else if (value is Char)
                value = (T)(Object)BitConverter.ToChar(buffer, 0);
            else if (value is Boolean)
                value = (T)(Object)BitConverter.ToBoolean(buffer, 0);
            else if (value is Byte)
                value = (T)(Object)(buffer[0]);
            else if (value is SByte)
                value = (T)(Object)(buffer[0]);
            else
                Console.WriteLine(string.Format("StreamUtils.Read<Number> failed, is {0} Type...", value.ToString()));
            return value;
        }
        public static bool Read(Stream s, Type t, out object value)
        {
            int len = 0;
            byte[] buffer = null;
            if (t.IsEnum)
            {
                len = System.Runtime.InteropServices.Marshal.SizeOf(typeof(Int32));
                buffer = ReadBytes(s, len);
                Int32 v = BitConverter.ToInt32(buffer, 0);
                value = Enum.ToObject(t, v);
                return true;
            }
            if (t == typeof(String))
            {
                Byte[] strBuf = null;
                strBuf = new Byte[sizeof(ushort)];
                s.Read(strBuf, 0, strBuf.Length);
                // 高低位交换
                //Array.Reverse(strBuf);
                ushort strLength = BitConverter.ToUInt16(strBuf, 0);
                strBuf = new Byte[strLength];
                s.Read(strBuf, 0, strBuf.Length);
                value = Encoding.UTF8.GetString(strBuf, 0, strBuf.Length);
                return true;
            }
            if (t == typeof(DateTime))
            {
                long ticks = 0;
                Read(s, ref ticks);
                value = new DateTime(ticks);
                return true;
            }
            if (t.IsClass)
            {
                Type[] ts = t.GetInterfaces();
                for(int i = 0; i < ts.Length; ++ i)
                {
                    if(ts[i] == typeof(IFieldReader))
                    {
                        value = Activator.CreateInstance(t);
                        (value as IFieldReader).Read(s);
                        return true;
                    }
                }
            }
            if(t.IsArray)
            {
                ushort count = 0;
                Read(s, ref count);
                Type at = t.GetElementType();
                Array array = Array.CreateInstance(at, count);
                if (count > 0)
                {
                    for (int i = 0; i < count; ++ i)
                    {
                        object obj = null;
                        Read(s, at, out obj);
                        array.SetValue(obj, i);
                    }
                }
                value = array;
                return true;
            }
            if (t == typeof(Boolean))
            {
                buffer = ReadBytes(s, 1);
                value = BitConverter.ToBoolean(buffer, 0);
                return true;
            }
            len = System.Runtime.InteropServices.Marshal.SizeOf(t);
            buffer = ReadBytes(s, len);
            // 高低位交换
            //Array.Reverse(buffer);
            if (t == typeof(Int64))
                value = BitConverter.ToInt64(buffer, 0);
            else if (t == typeof(UInt64))
                value = BitConverter.ToUInt64(buffer, 0);
            else if (t == typeof(Int32))
                value = BitConverter.ToInt32(buffer, 0);
            else if (t == typeof(UInt32))
                value = BitConverter.ToUInt32(buffer, 0);
            else if (t == typeof(Int16))
                value = BitConverter.ToInt16(buffer, 0);
            else if (t == typeof(UInt16))
                value = BitConverter.ToUInt16(buffer, 0);
            else if (t == typeof(Single))
                value = BitConverter.ToSingle(buffer, 0);
            else if (t == typeof(Double))
                value = BitConverter.ToDouble(buffer, 0);
            else if (t == typeof(Char))
                value = BitConverter.ToChar(buffer, 0);
            else if (t == typeof(Boolean))
                value = BitConverter.ToBoolean(buffer, 0);
            else if (t == typeof(Byte))
                value = (buffer[0]);
            else if (t == typeof(SByte))
                value = (buffer[0]);
            else
            {
                value = null;
                Console.WriteLine(string.Format("StreamUtils.Read failed, is {0} Type...", t.ToString()));
                return false;
            }
            return true;
        }
        public static T[] Read1Array<T>(Stream s, out T[] value) where T : IArrayReader, new()
        {
            value = null;
            byte len = 0;
            Read(s, ref len);
            if (len > 0)
            {
                value = new T[len];
                for (int i = 0; i < len; ++i)
                {
                    value[i] = new T();
                    value[i].ReadFields(s);
                }
            }
            return value;
        }
        public static T[] Read2Array<T>(Stream s, out T[] value) where T : IArrayReader, new()
        {
            value = null;
            short len = 0;
            Read(s, ref len);
            if(len > 0)
            {
                value = new T[len];
                for (int i = 0; i < len; ++i)
                {
                    value[i] = new T();
                    value[i].ReadFields(s);
                }
            }
            return value;
        }
        public static Stream Write(Stream s, String value, params int[] count)
        {
            byte[] length = null;
            byte[] buffer = null;
            if (null != count && count.Length > 0 && count[0] > 0)
            {
                length = BitConverter.GetBytes((ushort)count[0]);
                buffer = System.Text.Encoding.UTF8.GetBytes(value.ToCharArray(), 0, count[0]);
            }
            else
            {
                buffer = System.Text.Encoding.UTF8.GetBytes(value);
                length = BitConverter.GetBytes((ushort)buffer.Length);
            }
            // 高低位交换
            //Array.Reverse(length);
            s.Write(length, 0, length.Length);
            s.Write(buffer, 0, buffer.Length);
            return s;
        }
        public static Stream Write(Stream s, List<String> values, params int[] count)
        {
            if (null == values)
            {
                return s;
            }
            byte[] num = null;
            int valueNum = values.Count;
            if (valueNum <= 0)
            {
                num = BitConverter.GetBytes((ushort)0);
                s.Write(num, 0, num.Length);
                return s;
            }
            num = BitConverter.GetBytes((ushort)valueNum);
            // 高低位交换
            //Array.Reverse(num);
            s.Write(num, 0, num.Length);
            for (int i = 0; i < valueNum; ++i)
            {
                string value = values[i];
                Write(s, value, count);
            }
            return s;
        }
        public static Stream Write(Stream s, String[] values, params int[] count)
        {
            if (null == values)
            {
                return s;
            }
            byte[] num = null;
            int valueNum = values.Length;
            if (valueNum <= 0)
            {
                num = BitConverter.GetBytes((ushort)0);
                s.Write(num, 0, num.Length);
                return s;
            }
            num = BitConverter.GetBytes((ushort)valueNum);
            // 高低位交换
            //Array.Reverse(num);
            s.Write(num, 0, num.Length);
            for (int i = 0; i < valueNum; ++i)
            {
                string value = values[i];
                Write(s, value, count);
            }
            return s;
        }
        public static Stream Write(Stream s, Byte[] value, params int[] count)
        {
            byte[] length = null;
            byte[] buffer = null;
            if (null != count && count.Length > 0 && count[0] > 0)
            {
                length = BitConverter.GetBytes((ushort)count[0]);
                buffer = new byte[count[0]];
                Array.Copy(value, buffer, count[0]);
            }
            else
            {
                buffer = new byte[value.Length];
                Array.Copy(value, buffer, buffer.Length);
                length = BitConverter.GetBytes((ushort)buffer.Length);
            }
            // 高低位交换
            //Array.Reverse(length);
            s.Write(length, 0, length.Length);
            s.Write(buffer, 0, buffer.Length);
            return s;
        }
        public static Stream WriteByteArray4(Stream s, Byte[] value)
        {
            int len = 0;
            if (null == value)
            {
                return Write(s, len);
            }
            byte[] length = BitConverter.GetBytes(value.Length);
            s.Write(length, 0, length.Length);
            s.Write(value, 0, value.Length);
            return s;
        }
        //public static Stream Write<LenT, ValueT>(Stream s, ValueT[] values)
        ////    where LenT : IConvertible
        //    where ValueT : class, IFieldWriter
        //{
        //    if (null == values || values.Length <= 0)
        //    {
        //        LenT zero = default(LenT);
        //        Write(s, zero);
        //        return s;
        //    }
        //    Write(s, (LenT)(Object)values.Length);
        //    for (int i = 0; i < values.Length; ++i)
        //    {
        //        ValueT v = values[i] as ValueT;
        //        v.Write(s);
        //    }
        //    return s;
        //}
        public static Stream Write<T>(Stream s, T value)// where T : IConvertible
        {
            byte[] bin = null;
            if (value is Int64)
                bin = BitConverter.GetBytes((Int64)(Object)value);
            else if (value is UInt64)
                bin = BitConverter.GetBytes((UInt64)(Object)value);
            else if (value is Int32)
                bin = BitConverter.GetBytes((Int32)(Object)value);
            else if (value is UInt32)
                bin = BitConverter.GetBytes((UInt32)(Object)value);
            else if (value is Int16)
                bin = BitConverter.GetBytes((Int16)(Object)value);
            else if (value is UInt16)
                bin = BitConverter.GetBytes((UInt16)(Object)value);
            else if (value is Single)
                bin = BitConverter.GetBytes((Single)(Object)value);
            else if (value is Double)
                bin = BitConverter.GetBytes((Double)(Object)value);
            else if (value is Char)
                bin = BitConverter.GetBytes((Char)(Object)value);
            else if (value is Boolean)
                bin = BitConverter.GetBytes((Boolean)(Object)value);
            else if (value is Byte)
            {
                bin = new byte[1];
                bin[0] = (Byte)(Object)value;
            }
            else if (value is SByte)
            {
                bin = new byte[1];
                bin[0] = (byte)(Object)value;
            }
            else if (value is IFieldWriter)
            {
                (value as IFieldWriter).Write(s);
                return s;
            }
            else
                Console.WriteLine(string.Format("StreamUtils.Write<Number> failed, is {0} Type...", value.ToString()));
            // 高低位交换
            //Array.Reverse(bin);
            s.Write(bin, 0, bin.Length);
            return s;
        }
        public static Stream Write(Stream s, Type t, object value)// where T : IConvertible
        {
            byte[] bin = null;
            if (value is Int64)
                bin = BitConverter.GetBytes((Int64)value);
            else if (value is UInt64)
                bin = BitConverter.GetBytes((UInt64)value);
            else if (value is Int32)
                bin = BitConverter.GetBytes((Int32)value);
            else if (value is UInt32)
                bin = BitConverter.GetBytes((UInt32)value);
            else if (value is Int16)
                bin = BitConverter.GetBytes((Int16)value);
            else if (value is UInt16)
                bin = BitConverter.GetBytes((UInt16)value);
            else if (value is Single)
                bin = BitConverter.GetBytes((Single)value);
            else if (value is Double)
                bin = BitConverter.GetBytes((Double)value);
            else if (value is Char)
                bin = BitConverter.GetBytes((Char)value);
            else if (value is Boolean)
                bin = BitConverter.GetBytes((Boolean)value);
            else if (t.IsEnum)
                bin = BitConverter.GetBytes((Int32)Convert.ChangeType(value, typeof(Int32)));
            else if (value is Byte)
            {
                bin = new byte[1];
                bin[0] = (Byte)value;
            }
            else if (value is SByte)
            {
                bin = new byte[1];
                bin[0] = (byte)value;
            }
            else if(value is DateTime)
            {
                long ticks = ((DateTime)value).Ticks;
                bin = BitConverter.GetBytes((Int64)ticks);
            }
            else if (value is IFieldWriter)
            {
                (value as IFieldWriter).Write(s);
                return s;
            }
            else if (value is String)
            {
                byte[] length = null;
                byte[] buffer = null;
                buffer = System.Text.Encoding.UTF8.GetBytes(value as String);
                length = BitConverter.GetBytes((ushort)buffer.Length);
                // 高低位交换
                //Array.Reverse(length);
                s.Write(length, 0, length.Length);
                s.Write(buffer, 0, buffer.Length);
                return s;
            }
            else if(t.IsArray)
            {
                Array array = value as Array;
                ushort len = (ushort)array.Length;
                Write(s, len);
                foreach(var o in array)
                    Write(s, o.GetType(), o);
                return s;
            }
            else
            {
                Console.WriteLine(string.Format("StreamUtils.Write<Number> failed, is {0} Type...", value.ToString()));
                return s;
            }
            // 高低位交换
            //Array.Reverse(bin);
            s.Write(bin, 0, bin.Length);
            return s;
        }
        public static void Write1Array<T>(Stream s, T[] value) where T : IArrayWriter
        {
            byte len = 0;
            if (null == value)
            {
                Write(s, len);
                return;
            }
            len = (byte)value.Length;
            Write(s, len);
            for (int i = 0; i < len; ++i)
                value[i].WriteFields(s);
        }
        public static void Write2Array<T>(Stream s, T[] value) where T : IArrayWriter
        {
            short len = 0;
            if (null == value)
            {
                Write(s, len);
                return;
            }
            len = (short)value.Length;
            Write(s, len);
            for (int i = 0; i < len; ++i)
                value[i].WriteFields(s);
        }


        public static T BitConvert<T>(byte[] b, out T value)// where T : IConvertible
        {
            return BitConvert<T>(b, 0, out value);
        }
        public static T BitConvert<T>(byte[] b, int offset, out T value)// where T : IConvertible
        {
            value = default(T);
            int len = System.Runtime.InteropServices.Marshal.SizeOf(typeof(T));
            byte[] bin = new byte[len];
            for (int i = offset, j = 0; j < len; ++i, ++j)
                bin[j] = b[i];
            // 高低位交换
            //Array.Reverse(bin);
            if (value is Int64)
                value = (T)(Object)BitConverter.ToInt64(bin, 0);
            else if (value is UInt64)
                value = (T)(Object)BitConverter.ToUInt64(bin, 0);
            else if (value is Int32)
                value = (T)(Object)BitConverter.ToInt32(bin, 0);
            else if (value is UInt32)
                value = (T)(Object)BitConverter.ToUInt32(bin, 0);
            else if (value is Int16)
                value = (T)(Object)BitConverter.ToInt16(bin, 0);
            else if (value is UInt16)
                value = (T)(Object)BitConverter.ToUInt16(bin, 0);
            else if (value is Single)
                value = (T)(Object)BitConverter.ToSingle(bin, 0);
            else if (value is Double)
                value = (T)(Object)BitConverter.ToDouble(bin, 0);
            else if (value is Char)
                value = (T)(Object)BitConverter.ToChar(bin, 0);
            else if (value is Boolean)
                value = (T)(Object)BitConverter.ToBoolean(bin, 0);
            else if (value is Byte)
                value = (T)(Object)(bin[0]);
            else if (value is SByte)
                value = (T)(Object)(bin[0]);
            else
                Console.WriteLine(string.Format("StreamUtils.Read<Number> failed, is {0} Type...", value.ToString()));
            return value;
        }        
        //public static byte[] BitCompress(byte[] bin)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    GZipOutputStream gzip = new GZipOutputStream(ms);
        //    gzip.Write(bin, 0, bin.Length);
        //    gzip.Close();
        //    return ms.ToArray();
        //}
        //public static byte[] BitDecompress(byte[] bin)
        //{
        //    GZipInputStream gzi = new GZipInputStream(new MemoryStream(bin));
        //    MemoryStream re = new MemoryStream();
        //    int count = 0;
        //    byte[] data = new byte[4096];
        //    while ((count = gzi.Read(data, 0, data.Length)) != 0)
        //    {
        //        re.Write(data, 0, count);
        //    }
        //    return re.ToArray();
        //}
    }
}
