using System;
using System.Collections.Generic;
using System.Linq;

namespace Library.CsvDataTable
{
    public interface ICsvTableElement
    {
        int Count { get; }
    }
    public abstract class CsvTableKeyValue : ICsvTableElement, IConvertible
    {
        public CsvTableKeyValue() { }
        protected int mHashCode = 0;
        public abstract int Count { get; }
        //public virtual bool Init(StkId[] ps) { return true; }
        //public virtual bool Init(StkId[] ps, ILuaTableElement oth) { return true; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            CsvTableKeyValue oth = obj as CsvTableKeyValue;
            return oth.mHashCode == mHashCode;
        }
        public override int GetHashCode()
        {
            return mHashCode;
        }
        public TypeCode GetTypeCode()
        {
            throw new NotImplementedException();
        }

        public bool ToBoolean(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public char ToChar(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public sbyte ToSByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public byte ToByte(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public short ToInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ushort ToUInt16(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public int ToInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public uint ToUInt32(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public long ToInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public ulong ToUInt64(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public float ToSingle(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public double ToDouble(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public decimal ToDecimal(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public DateTime ToDateTime(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public string ToString(IFormatProvider provider)
        {
            throw new NotImplementedException();
        }

        public object ToType(Type conversionType, IFormatProvider provider)
        {
            throw new NotImplementedException();
        }
    }
    public interface ICsvDataTableReader
    {
        int Count { get; }
        void PutValues(IConvertible key, IConvertible[] vals);
    }
    public abstract class CsvDataTableReader<T, TK> : ICsvDataTableReader
        where T : CsvDataTableReader<T, TK>
        where TK : IConvertible
    {
        public abstract int Count { get; }
        public void PutValues(IConvertible key, IConvertible[] vals)
        {
            PutValues((TK)key, vals);
        }
        public abstract void PutValues(TK key, IConvertible[] vals);
    }
    public class CsvDataTable<T, TK, TV> : CsvDataTableReader<T, TK>
        where T : CsvDataTable<T, TK, TV>
        where TK : CsvTableKeyValue
        where TV : CsvTableKeyValue
    {
        Dictionary<TK, TV> mDataTable = new Dictionary<TK, TV>();

        public override int Count { get { return mDataTable.Count; } }

        public override void PutValues(TK key, IConvertible[] val) { }
    }
    public class CsvNumber2StringDataTable<T, TK> : CsvDataTableReader<T, TK>
        where T : CsvNumber2StringDataTable<T, TK>
        where TK : IConvertible
    {
        Dictionary<TK, string> mDataTable = new Dictionary<TK, string>();

        public override int Count { get { return mDataTable.Count; } }
        public string[] Values { get { return mDataTable.Values.ToArray(); } }
        public Dictionary<TK, string>.Enumerator Pairs { get { return mDataTable.GetEnumerator(); } }
        public string this[TK k]
        {
            get
            {
                if (mDataTable.ContainsKey(k))
                    return mDataTable[k];
                return null;
            }
        }
        public override void PutValues(TK key, IConvertible[] vals)
        {
            if (!mDataTable.ContainsKey(key))
                mDataTable.Add(key, vals[0] as string);
            else
                mDataTable[key] = vals[0] as string;
        }
    }
    public class CsvNumber2StringStringDataTable<T, TK> : CsvDataTableReader<T, TK>
        where T : CsvNumber2StringStringDataTable<T, TK>
        where TK : IConvertible
    {
        protected Dictionary<TK, string> mStr1DataTable = new Dictionary<TK, string>();
        protected Dictionary<TK, string> mStr2DataTable = new Dictionary<TK, string>();

        public override int Count { get { return mStr1DataTable.Count; } }
        public string[] Str1Values { get { return mStr1DataTable.Values.ToArray(); } }
        public string[] Str2Values { get { return mStr2DataTable.Values.ToArray(); } }
        public Dictionary<TK, string>.Enumerator Str1Pairs { get { return mStr1DataTable.GetEnumerator(); } }
        public Dictionary<TK, string>.Enumerator Str2Pairs { get { return mStr2DataTable.GetEnumerator(); } }
        public override void PutValues(TK key, IConvertible[] vals)
        {
            if (!mStr1DataTable.ContainsKey(key))
                mStr1DataTable.Add(key, vals[0] as string);
            else
                mStr1DataTable[key] = vals[0] as string;
            if (!mStr2DataTable.ContainsKey(key))
                mStr2DataTable.Add(key, vals[1] as string);
            else
                mStr2DataTable[key] = vals[1] as string;
        }
    }
    public class CsvNumber2StringNumberDataTable<T, TK, TN> : CsvDataTableReader<T, TK>
        where T : CsvNumber2StringNumberDataTable<T, TK, TN>
        where TK : IConvertible
    {
        protected Dictionary<TK, string> mStrDataTable = new Dictionary<TK, string>();
        protected Dictionary<TK, TN> mNumDataTable = new Dictionary<TK, TN>();

        public override int Count { get { return mStrDataTable.Count; } }
        public string[] StrValues { get { return mStrDataTable.Values.ToArray(); } }
        public TN[] NumValues { get { return mNumDataTable.Values.ToArray(); } }
        public Dictionary<TK, string>.Enumerator StrPairs { get { return mStrDataTable.GetEnumerator(); } }
        public Dictionary<TK, TN>.Enumerator NumPairs { get { return mNumDataTable.GetEnumerator(); } }
        public override void PutValues(TK key, IConvertible[] vals)
        {
            if (!mStrDataTable.ContainsKey(key))
                mStrDataTable.Add(key, vals[0] as string);
            else
                mStrDataTable[key] = vals[0] as string;
            if (!mNumDataTable.ContainsKey(key))
                mNumDataTable.Add(key, (TN)vals[1]);
            else
                mNumDataTable[key] = (TN)vals[1];
        }
    }
    public class CsvUShort2StringDataTable : CsvNumber2StringDataTable<CsvUShort2StringDataTable, ushort>
    {
    }
    public class CsvUShort2StringUShortDataTable : CsvNumber2StringNumberDataTable<CsvUShort2StringUShortDataTable, ushort, ushort>
    {
        public string GetStrVal(ushort k)
        {
            if (mStrDataTable.ContainsKey(k))
                return mStrDataTable[k];
            return null;
        }
        public ushort GetNumVal(ushort k)
        {
            if (mNumDataTable.ContainsKey(k))
                return mNumDataTable[k];
            return ushort.MinValue;
        }
    }
}
