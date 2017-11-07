using System;
using System.Collections.Generic;
using Scripting.UniLua;
#if !UNITY_3 && !UNITY_4 && !UNITY_5
    using Logging.FileLogger;
#else
    using UnityEngine;
#endif

namespace Scripting.LuaSupport
{
    public class LuaScriptDelegate
    {
        public static ILuaState State { get { return mLuaState; } }
        protected virtual string LUAEntryFileName { get { return null; } }
        protected virtual string[] MethodNameList() { return null; }

        protected void InitLuaLib()
        {
            if (mLuaState == null)
            {
                mLuaState = LuaAPI.NewState();
                mLuaState.L_OpenLibs();
                LuaEncLib.OpenLib(mLuaState);
            }
            string entryFileName = LUAEntryFileName;
            if (null != entryFileName && entryFileName.Length > 0)
            {
                var status = mLuaState.L_DoFile(entryFileName);
                if (status != ThreadStatus.LUA_OK)
                    throw new Exception(mLuaState.ToString(-1));
                if (!mLuaState.IsTable(-1))
                    throw new Exception("entry main's return value is not a table");
                OnEntryFileOvered(mLuaState.ToObject(-1) as LuaTable);
                string[] funcList = MethodNameList();
                if (null != funcList)
                {
                    for (int i = 0; i < funcList.Length; ++i)
                    {
                        var s = funcList[i];
                        if (mLuaFunctionRefs.ContainsKey(s))
                        {
#if !UNITY_3 && !UNITY_4 && !UNITY_5
                            FileLogMgr.OutToFileW("注册方法[{0}时发现同名的方法，删掉老的注册新的！", s);
#else
                            Debug.LogWarning("注册方法" + s + "时发现同名的方法，删掉老的注册新的！");
#endif
                            //UnStoreMethod(s, mLuaFunctionRefs[s]);
                            mLuaFunctionRefs.Remove(s);
                        }
                        int funcRef = StoreMethod(s);
                        mLuaFunctionRefs.Add(s, funcRef);
                    }
                }
                mLuaState.Pop(1);
            }
        }
        protected virtual void OnEntryFileOvered(LuaTable entryTbl) { }
        private int StoreMethod(string name)
        {
            mLuaState.GetField(-1, name);
            if (!mLuaState.IsFunction(-1))
                throw new Exception(string.Format(
                    "method {0} not found!", name));
            return mLuaState.L_Ref(LuaDef.LUA_REGISTRYINDEX);
        }
        //private void UnStoreMethod(string name, int l_ref)
        //{
        //    mLuaState.GetField(-1, name);
        //    if (!mLuaState.IsFunction(-1))
        //        throw new Exception(string.Format(
        //            "method {0} not found!", name));
        //    mLuaState.L_Unref(LuaDef.LUA_REGISTRYINDEX, l_ref);
        //}
        protected void CallMethod<T>(bool traceBack, params object[] plist)
        {
            string key = typeof(T).ToString();
            if (!mLuaFunctionRefs.ContainsKey(key))
            {
                return;
            }
            CallMethod(mLuaFunctionRefs[key], traceBack, plist);
        }
        public void CallMethod(string key, bool traceBack, params object[] plist)
        {
            if (!mLuaFunctionRefs.ContainsKey(key))
            {
                return;
            }
            CallMethod(mLuaFunctionRefs[key], traceBack, plist);
        }
        private void CallMethod(int funcRef, bool traceBack, params object[] plist)
        {
            mLuaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);
            // insert `traceback' function  
            var b = 0;
            if (traceBack)
            {
                b = mLuaState.GetTop();
                mLuaState.PushCSharpFunction(Traceback);
                mLuaState.Insert(b);
            }
            int pCount = 0;
            if (null != plist)
            {
                for (int i = 0; i < plist.Length; ++i)
                {
                    var s = plist[i];
                    if (s is byte)
                        mLuaState.PushNumber((byte)s);
                    else if (s is sbyte)
                        mLuaState.PushNumber((sbyte)s);
                    else if (s is short)
                        mLuaState.PushNumber((short)s);
                    else if (s is ushort)
                        mLuaState.PushNumber((ushort)s);
                    else if (s is int)
                        mLuaState.PushNumber((int)s);
                    else if (s is uint)
                        mLuaState.PushNumber((uint)s);
                    else if (s is long)
                        mLuaState.PushNumber((long)s);
                    else if (s is ulong)
                        mLuaState.PushUInt64((ulong)s);
                    else if (s is float)
                        mLuaState.PushNumber((float)s);
                    else if (s is double)
                        mLuaState.PushNumber((double)s);
                    else if (s is bool)
                        mLuaState.PushBoolean((bool)s);
                    else if (s is string)
                        mLuaState.PushString(s as string);
                    pCount++;
                }
            }
            var status = mLuaState.PCall(pCount, 0, b);
            if (status != ThreadStatus.LUA_OK)
#if !UNITY_3 && !UNITY_4 && !UNITY_5
                FileLogMgr.OutToFileE(mLuaState.ToString(-1));
#else
                Debug.LogError(mLuaState.ToString(-1));
#endif
            if (traceBack)
            {
                // remove `traceback' function
                mLuaState.Remove(b);
            }
        }
        public T CallMethod<T>(string key, bool traceBack, params object[] plist)
        {
            if (!mLuaFunctionRefs.ContainsKey(key))
            {
                return default(T);
            }
            return CallMethod<T>(mLuaFunctionRefs[key], traceBack, plist);
        }
        private T CallMethod<T>(int funcRef, bool traceBack, params object[] plist)
        {
            mLuaState.RawGetI(LuaDef.LUA_REGISTRYINDEX, funcRef);
            var b = 0;
            if (traceBack)
            {
                // insert `traceback' function  
                b = mLuaState.GetTop();
                mLuaState.PushCSharpFunction(Traceback);
                mLuaState.Insert(b);
            }
            int pCount = 0;
            if (null != plist)
            {
                for (int i = 0; i < plist.Length; ++i)
                {
                    var s = plist[i];
                    //FileLogMgr.OutToFileD("CallMethod:PushString=" + s);
                    if (s is byte)
                        mLuaState.PushNumber((byte)s);
                    else if (s is sbyte)
                        mLuaState.PushNumber((sbyte)s);
                    else if (s is short)
                        mLuaState.PushNumber((short)s);
                    else if (s is ushort)
                        mLuaState.PushNumber((ushort)s);
                    else if (s is int)
                        mLuaState.PushNumber((int)s);
                    else if (s is uint)
                        mLuaState.PushNumber((uint)s);
                    else if (s is long)
                        mLuaState.PushNumber((long)s);
                    else if (s is ulong)
                        mLuaState.PushNumber((ulong)s);
                    else if (s is float)
                        mLuaState.PushNumber((float)s);
                    else if (s is double)
                        mLuaState.PushNumber((double)s);
                    else if (s is bool)
                        mLuaState.PushBoolean((bool)s);
                    else if (s is string)
                        mLuaState.PushString(s as string);
                    pCount++;
                }
            }
            var status = mLuaState.PCall(pCount, 1, b);
            if (status != ThreadStatus.LUA_OK)
#if !UNITY_3 && !UNITY_4 && !UNITY_5
                FileLogMgr.OutToFileE(mLuaState.ToString(-1));
#else
                Debug.LogError(mLuaState.ToString(-1));
#endif
            if (traceBack)
            {
                // remove `traceback' function
                mLuaState.Remove(b);
            }
            T ret = default(T);
            int retValue = mLuaState.GetTop();
            if (ret is string)
                ret = (T)(object)mLuaState.ToString(retValue);
            else if (ret is bool)
                ret = (T)(object)mLuaState.ToBoolean(retValue);
            else if (ret is float || ret is double || ret is byte || ret is sbyte || ret is short || ret is ushort ||
                ret is int || ret is uint)
            {
                if (ret is long || ret is ulong)
                {
                    ret = (T)Convert.ChangeType(mLuaState.ToUInt64(retValue), typeof(T));
                }
                else
                {
                    ret = (T)Convert.ChangeType(mLuaState.ToNumber(retValue), typeof(T));
                }
            }
            mLuaState.Pop(1);
            return ret;
        }
        private static int Traceback(ILuaState lua)
        {
            var msg = lua.ToString(1);
            if (msg != null)
                lua.L_Traceback(lua, msg, 1);
            else if (!lua.IsNoneOrNil(1))
            {
                // is there an error object?
                // try its `tostring' metamethod
                if (!lua.L_CallMeta(1, "__tostring"))
                    lua.PushString("(no error message)");
            }
            return 1;
        }

        protected static ILuaState mLuaState;
        static Dictionary<string, int> mLuaFunctionRefs = new Dictionary<string, int>();
    }
}