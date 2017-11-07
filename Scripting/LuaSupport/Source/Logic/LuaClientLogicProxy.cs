using System.Collections.Generic;

namespace Scripting.LuaSupport.Logic
{
    public class LuaClientLogicProxy<T> : LuaScriptDelegate
        where T : LuaClientLogicProxy<T>, new()
    {
        static T mInst = null;
        public static string LUA数据表脚本路径 = "LuaLogic/client";
        public static string LUA数据表脚本入口文件 = "main.lua";
        public static void Begining()
        {
            if (mInst == null)
            {
                mInst = new T();
                mInst.Init();
            }
        }
        public static T Inst
        {
            get
            {
                if (mInst == null)
                {
                    mInst = new T();
                    mInst.Init();
                }
                return mInst;
            }
        }
        public static void Destroy()
        {
            if (null != mInst)
            {
                mInst = null;
            }
        }
        protected override string LUAEntryFileName
        {
            get
            {
                return string.Format("{0}/{1}", LUA数据表脚本路径,
                                                LUA数据表脚本入口文件);
            }
        }
        protected override string[] MethodNameList()
        {
            List<string> ret = new List<string>();
            return ret.ToArray();
        }
        void Init()
        {
            State.L_RequireF(LuaCommonLib.LIBName   // 库的名字
                            , LuaCommonLib.OpenLib  // 库的初始化函数
                            , false                 // 不默认放到全局命名空间 (在需要的地方用require获取)
            );
            base.InitLuaLib();
        }
    }
}
