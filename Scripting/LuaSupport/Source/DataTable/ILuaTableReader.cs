using UnityEngine;
using System.Collections.Generic;
using Scripting.UniLua;

namespace Scripting.LuaSupport.DataTable
{
    public interface ILuaTableElement
    {
        int Count { get; }
        bool Init(StkId[] ps);
        bool Init(StkId[] ps, ILuaTableElement oth);
        ILuaTableReader Holder { set; get; }
    }
    public abstract class LuaTableKeyValue : ILuaTableElement
    {
        ILuaTableReader mHolder = null;
        public ILuaTableReader Holder { set { mHolder = value; } get { return mHolder; } }
        public LuaTableKeyValue() { }
        protected int mHashCode = 0;
        public abstract int Count { get; }
        public virtual bool Init(StkId[] ps) { return true; }
        public virtual bool Init(StkId[] ps, ILuaTableElement oth) { return true; }
        public override bool Equals(object obj)
        {
            if (obj == null)
                return false;
            LuaTableKeyValue oth = obj as LuaTableKeyValue;
            if (oth.mHashCode == this.mHashCode)
            {
                int i = 0;
                i++;
            }
            return oth.mHashCode == mHashCode;
        }
        public override int GetHashCode()
        {
            //if (null == mDatas)
            //    return 0;
            //int hashCode = 0;
            //for (int i = 0; i < mDatas.Length; ++ i)
            //{
            //    object o = mDatas[i];
            //    hashCode ^= o.GetHashCode();
            //}
            return mHashCode;
        }
    }
    public class LuaTableReader<T, TKey, TVal> : ILuaTableReader
        where T : LuaTableReader<T, TKey, TVal>
        where TKey : ILuaTableElement, new()
        where TVal : ILuaTableElement, new()
    {
        //protected TVal NoValueVal = new TVal();
        protected Dictionary<TKey, TVal> mTableDatas = new Dictionary<TKey, TVal>();
        protected virtual void OnKeyLoaded(TKey k) { }
        protected virtual void OnDataLoaded(TKey k, TVal d) { }
        protected virtual void OnDataLoaded(params object[] ps) { }
        protected virtual void OnTableReadOvered() { }
        public bool ReadTable(LuaTable tbl)
        {
            // KEY Index
            Dictionary<string, int> headerKeyDict = LuaReadHelper.ReadHeaderKey(tbl, "HK");
            Dictionary<string, int> headerDataDict = LuaReadHelper.ReadHeaderKey(tbl, "HD");
            if (null == headerKeyDict || null == headerDataDict)
                return false;
            TKey tKey = new TKey();
            TVal tVal = new TVal();
            if (headerKeyDict.Count != tKey.Count || headerDataDict.Count != tVal.Count)
                return false;
            tKey = default(TKey);
            tVal = default(TVal);
            // DATA
            LuaTable hTbl = LuaReadHelper.GetSubTable(tbl, "DATA");
            if (null == hTbl)
                return false;
            for (int i = 0; i < hTbl.ArrayPart.Length; ++i)
            {
                StkId stkID = hTbl.ArrayPart[i];
                if ((null == stkID) || (default(TValue) == stkID.V))
                    continue;
                if (!stkID.V.TtIsTable())
                    continue;
                if (!ReadTableData(stkID))
                    return false;
            }
            for (int i = 0; i < hTbl.HashPart.Length; ++i)
            {
                StkId stkID = hTbl.HashPart[i].Val;
                if ((null == stkID) || (default(TValue) == stkID.V))
                    continue;
                if (!stkID.V.TtIsTable())
                    continue;
                if (!ReadTableData(stkID))
                    return false;
            }
            OnTableReadOvered();
            return true;
        }
        public bool ReadTableData(StkId stkID)
        {
            LuaTable subTbl = stkID.V.OValue as LuaTable;
            LuaTable subKeyTbl = LuaReadHelper.GetSubTable(subTbl, "K");
            if (null == subKeyTbl)
                return false;
            TKey tKey = new TKey();
            TVal tVal = new TVal();
            tKey.Holder = this;
            tVal.Holder = this;
            int keyCount = 0;
            for (int c = 0; c < subKeyTbl.ArrayPart.Length; ++c)
            {
                if ((null == subKeyTbl.ArrayPart[c]) || (default(TValue) == subKeyTbl.ArrayPart[c].V))
                    continue;
                keyCount++;
            }
            if (keyCount != tKey.Count)
                return false;
            tKey.Init(subKeyTbl.ArrayPart);
            OnKeyLoaded(tKey);
            LuaTable subDataTbl = LuaReadHelper.GetSubTable(subTbl, "D");
            if (null == subDataTbl)
                return false;
            int valCount = 0;
            for (int c = 0; c < subDataTbl.ArrayPart.Length; ++c)
            {
                if ((null == subDataTbl.ArrayPart[c]) || (default(TValue) == subDataTbl.ArrayPart[c].V))
                    continue;
                valCount++;
            }
            if (valCount != tVal.Count)
                return false;
            if (tVal is ConstLuaTable.ValDesc)
                tVal.Init(subDataTbl.ArrayPart, tKey);
            else
                tVal.Init(subDataTbl.ArrayPart);
            OnDataLoaded(tKey, tVal);
            if (mTableDatas.ContainsKey(tKey))
            {
                Debug.LogError(string.Format("读取数据表时出现相同的索引[{0}]", tKey.ToString()));
                return false;
            }
            mTableDatas.Add(tKey, tVal);
            return true;
        }
//#if UNITY_EDITOR
        public Dictionary<TKey, TVal> Datas { get { return mTableDatas; } }
//#endif
        public virtual void OnDestroy() { }
        public TVal this[TKey k]
        {
            get
            {
                if (!mTableDatas.ContainsKey(k))
                    return default(TVal);
                return mTableDatas[k];
            }
        }
    }
}
