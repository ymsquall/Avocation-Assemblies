using UnityEngine;
using System.Collections.Generic;
using Scripting.UniLua;

namespace Scripting.LuaSupport.ClientLuaData
{

    public class ClientLuaDataProxy : LuaScriptDelegate
    {
        static ClientLuaDataProxy mInst = null;
        public static string LUA数据表脚本路径 = "ClientData";
        public static string LUA数据表脚本入口文件 = "main_data.lua";

        static Dictionary<string, ILuaTableReader> mDataReader = null;
        static Dictionary<string, ILuaTableReader> NewDataReader
        {
            get 
            {
                return new Dictionary<string, ILuaTableReader>()
                        {
                            //{ RoleLuaTable.TableName, new RoleLuaTable() },
                        };
            }
        }
        public static void Begining()
        {
            if (mInst == null)
            {
                mDataReader = NewDataReader;
                mInst = new ClientLuaDataProxy();
                mInst.Init();
            }
        }
        public static ClientLuaDataProxy Inst
        {
            get
            {
                if (mInst == null)
                {
                    mDataReader = NewDataReader;
                    mInst = new ClientLuaDataProxy();
                    mInst.Init();
                }
                return mInst;
            }
        }
        public static void Destroy()
        {
            if (null != mInst)
            {
                mDataReader.Clear();
                mDataReader = null;
                mInst = null;
            }
        }
        public ILuaTableReader this[string n]
        {
            get
            {
                if (mDataReader.ContainsKey(n))
                    return mDataReader[n];
                return null;
            }
        }
        protected override string LUAEntryFileName
        {
            get
            {
//#if UNITY_EDITOR
                return string.Format("{0}/{1}", LUA数据表脚本路径,
                                                LUA数据表脚本入口文件);
//#else
//                return LUA数据表脚本入口文件;
//#endif
            }
        }
        protected override string[] MethodNameList()
        {
            List<string> ret = new List<string>(0);
            return ret.ToArray();
        }
        void Init()
        {
            //PopUpDialogView.Popup(PopUpDialogView.DialogType.error01, "InitLuaLib1");
            base.InitLuaLib();
            foreach (string t in mDataReader.Keys)
            {
                mLuaState.GetGlobal(t);
                int top = mLuaState.GetTop();
                if (mLuaState.IsTable(top))
                {
                    if (!mDataReader[t].ReadTable(mLuaState.ToObject(top) as LuaTable))
                    {
                        //PopupDialogView.Popup(弹框类型.错误提示, "读取LUA数据表" + t + "失败");
                        Debug.LogWarning("读取LUA数据表" + t + "失败");
                    }
                    mLuaState.Pop(1);
                }
            }
        }
    }
}
