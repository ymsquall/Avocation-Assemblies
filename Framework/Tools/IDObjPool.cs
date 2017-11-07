using System;
using System.Collections.Generic;

namespace Framework.Tools
{
    public interface IIDPoolObj : IDisposable
    {
        void New(params object[] ps);
    }
    public abstract class IDPoolObj<CLASS,BASET> : IIDPoolObj
        where CLASS : BaseType<BASET>
        where BASET : struct, IComparable, IFormattable, IConvertible, IComparable<BASET>, IEquatable<BASET>
    {
        protected CLASS mID = default(CLASS);
        public CLASS ID { set { mID = value; } get { return mID; } }
        public abstract void New(params object[] ps);
        public abstract void Dispose();
    }
    public interface IIDPool
    {
        object Min { get; }
        object Max { get; }
        object DoAdd();
    }
    public abstract class IDObjPool<CLASS, BASET, MgrT, ObjT> : IIDPool
        where CLASS : BaseType<BASET>
        where BASET : struct, IComparable, IFormattable, IConvertible, IComparable<BASET>, IEquatable<BASET>
        where MgrT : IDObjPool<CLASS, BASET, MgrT, ObjT>, new()
        where ObjT : IDPoolObj<CLASS, BASET>, new()
    {
        static MgrT mInst = null;
        protected CLASS mCurrent = default(CLASS);
        protected object mObjListLock = new object();
        protected Dictionary<CLASS, ObjT> mObjList = new Dictionary<CLASS, ObjT>();
        protected IDObjPool()
        {
            mCurrent = (CLASS)Min;
        }
        public static MgrT Inst
        {
            get
            {
                if(null == mInst)
                    mInst = new MgrT();
                return mInst;
            }
        }
        public abstract object Min { get; }
        public abstract object Max { get; }
        public ObjT this[CLASS id]
        {
            get
            {
                lock(Inst.mObjListLock)
                {
                    if (Inst.mObjList.ContainsKey(id))
                        return Inst.mObjList[id];
                }
                return null;
            }
        }
        public static ObjT NewObj(params object[] ps)
        {
            CLASS id = (CLASS)Inst.DoAdd();
            ObjT ret = null;
            lock (Inst.mObjListLock)
            {
                while (Inst.mObjList.ContainsKey(id))
                    id = (CLASS)Inst.DoAdd();
                ret = new ObjT();
                Inst.mObjList.Add(id, ret);
            }
            ret.ID = id;
            ret.New(ps);
            return ret;
        }
        public static ObjT NewWithID(CLASS id, params object[] ps)
        {
            ObjT ret = null;
            lock (Inst.mObjListLock)
            {
                while (Inst.mObjList.ContainsKey(id))
                    Inst.mObjList.Remove(id);
                ret = new ObjT();
                Inst.mObjList.Add(id, ret);
            }
            ret.ID = id;
            ret.New(ps);
            return ret;
        }
        public static ObjT DelObj(CLASS id)
        {
            ObjT t = null;
            lock (Inst.mObjListLock)
            {
                if (Inst.mObjList.ContainsKey(id))
                {
                    t = Inst.mObjList[id];
                    t.Dispose();
                    Inst.mObjList.Remove(id);
                }
            }
            return t;
        }
        public abstract object DoAdd();
    }
}