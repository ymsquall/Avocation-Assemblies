using System;
using System.IO;

namespace Framework.Tools
{
    public interface IBaseType : IFieldReaderWriter, IArrayReaderWrite
    {
        object ValueObj { get; }
    }
    public class BaseType<T> : IBaseType, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
    {
        public T Value { set; get; }
        public object ValueObj
        {
            get
            {
                return Value;
            }
        }
        public override string ToString()
        {
            return Value.ToString();
        }
        // IComparable
        public int CompareTo(object obj)
        {
            return Value.CompareTo(obj);
        }
        // IFormattable
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return Value.ToString();
        }
        // IConvertible
        public TypeCode GetTypeCode()
        {
            return Value.GetTypeCode();
        }
        public bool ToBoolean(IFormatProvider provider)
        {
            return Value.ToBoolean(provider);
        }
        public byte ToByte(IFormatProvider provider)
        {
            return Value.ToByte(provider);
        }
        public char ToChar(IFormatProvider provider)
        {
            return Value.ToChar(provider);
        }
        public DateTime ToDateTime(IFormatProvider provider)
        {
            return Value.ToDateTime(provider);
        }
        public decimal ToDecimal(IFormatProvider provider)
        {
            return Value.ToDecimal(provider);
        }
        public double ToDouble(IFormatProvider provider)
        {
            return Value.ToDouble(provider);
        }
        public short ToInt16(IFormatProvider provider)
        {
            return Value.ToInt16(provider);
        }
        public int ToInt32(IFormatProvider provider)
        {
            return Value.ToInt32(provider);
        }
        public long ToInt64(IFormatProvider provider)
        {
            return Value.ToInt64(provider);
        }
        public sbyte ToSByte(IFormatProvider provider)
        {
            return Value.ToSByte(provider);
        }
        public float ToSingle(IFormatProvider provider)
        {
            return Value.ToSingle(provider);
        }
        public string ToString(IFormatProvider provider)
        {
            return Value.ToString(provider);
        }
        public object ToType(Type conversionType, IFormatProvider provider)
        {
            return Value.ToType(conversionType, provider);
        }
        public ushort ToUInt16(IFormatProvider provider)
        {
            return Value.ToUInt16(provider);
        }
        public uint ToUInt32(IFormatProvider provider)
        {
            return Value.ToUInt32(provider);
        }
        public ulong ToUInt64(IFormatProvider provider)
        {
            return Value.ToUInt64(provider);
        }
        // IComparable<T>
        public int CompareTo(T other)
        {
            return Value.CompareTo(other);
        }
        // IEquatable<T>
        public bool Equals(T other)
        {
            return Value.Equals(other);
        }
        // 
        public void ReadFields(Stream s)
        {
            Read(s);
        }
        public void WriteFields(Stream s)
        {
            Write(s);
        }
        public void Read(Stream s)
        {
            T val = default(T);
            StreamUtils.Read(s, ref val);
            Value = val;
        }
        public void Write(Stream s)
        {
            StreamUtils.Write(s, Value);
        }
    }
    public class NumberBaseType<T, CLASS> : BaseType<T>
        where T : struct, IComparable, IFormattable, IConvertible, IComparable<T>, IEquatable<T>
        where CLASS : NumberBaseType<T, CLASS>, new()
    {
        public static readonly char[] ArraySplitPrefix = ",".ToCharArray();
        public NumberBaseType() { }
        // 隐式转换,右值
        public static CLASS ConvertFrom(object v)
        {
            CLASS ret = new CLASS();
            try
            {
                ret.Value = (T)System.Convert.ChangeType(v, typeof(T));
            }
            catch(System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ret;
        }
        // 隐式转换,左值
        public static TR ConvertTo<TR>(CLASS c)
        {
            TR ret = default(TR);
            try
            {
                if(typeof(TR) == typeof(System.UInt16))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.UInt16));
                if (typeof(TR) == typeof(System.UInt32))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.UInt32));
                if (typeof(TR) == typeof(System.UInt64))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.UInt64));
                if (typeof(TR) == typeof(System.Int16))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.Int16));
                if (typeof(TR) == typeof(System.Int32))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.Int32));
                if (typeof(TR) == typeof(System.Int64))
                    ret = (TR)System.Convert.ChangeType(c.Value, typeof(System.Int64));
            }
            catch (System.Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return ret;
        }
    }
    public class TypeQuery
    {
        public static Type GetTypeByString(string type)
        {
            switch (type.ToLower())
            {
                case "bool":
                    return Type.GetType("System.Boolean", true);
                case "byte":
                    return Type.GetType("System.Byte", true);
                case "sbyte":
                    return Type.GetType("System.SByte", true);
                case "char":
                    return Type.GetType("System.Char", true);
                case "decimal":
                    return Type.GetType("System.Decimal", true);
                case "double":
                    return Type.GetType("System.Double", true);
                case "float":
                    return Type.GetType("System.Single", true);
                case "int":
                    return Type.GetType("System.Int32", true);
                case "uint":
                    return Type.GetType("System.UInt32", true);
                case "long":
                    return Type.GetType("System.Int64", true);
                case "ulong":
                    return Type.GetType("System.UInt64", true);
                case "object":
                    return Type.GetType("System.Object", true);
                case "short":
                    return Type.GetType("System.Int16", true);
                case "ushort":
                    return Type.GetType("System.UInt16", true);
                case "string":
                    return Type.GetType("System.String", true);
                case "date":
                case "datetime":
                    return Type.GetType("System.DateTime", true);
                case "guid":
                    return Type.GetType("System.Guid", true);
                default:
                    return Type.GetType(type, true);
            }
        }
        public static object Str2Value(string str, Type t)
        {
            switch (Type.GetTypeCode(t))
            {
                case TypeCode.Boolean:
                    return bool.Parse(str);
                case TypeCode.Byte:
                    return byte.Parse(str);
                case TypeCode.SByte:
                    return sbyte.Parse(str);
                case TypeCode.Char:
                    return char.Parse(str);
                case TypeCode.Decimal:
                    return decimal.Parse(str);
                case TypeCode.Double:
                    return double.Parse(str);
                case TypeCode.Single:
                    return float.Parse(str);
                case TypeCode.Int32:
                    return int.Parse(str);
                case TypeCode.UInt32:
                    return uint.Parse(str);
                case TypeCode.Int64:
                    return long.Parse(str);
                case TypeCode.UInt64:
                    return ulong.Parse(str);
                case TypeCode.Object:
                    return Convert.ChangeType(str, typeof(object));
                case TypeCode.Int16:
                    return short.Parse(str);
                case TypeCode.UInt16:
                    return ushort.Parse(str);
                case TypeCode.String:
                    return str;
                case TypeCode.DateTime:
                    return DateTime.Parse(str);
                case TypeCode.DBNull:
                    return DBNull.Value;
                case TypeCode.Empty:
                    return null;
            }
            return null;
        }
    }
}