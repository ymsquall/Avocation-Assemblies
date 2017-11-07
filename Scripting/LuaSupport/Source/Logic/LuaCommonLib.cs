using UnityEngine;
using Scripting.UniLua;
using System;
using System.Reflection;
using Framework.Tools;

namespace Scripting.LuaSupport.Logic
{
    public class LuaCommonLib : LuaCallLib<LuaCommonLib>
    {
        public static int LoadMod(ILuaState lua)
        {
            var cn = lua.L_CheckString(1); // 第一个参数
            try
            {
                Type c = Type.GetType(cn, true, true);
                MethodInfo mi = c.GetMethod("OpenLib", BindingFlags.Public | BindingFlags.Static | BindingFlags.FlattenHierarchy);
                CSharpFunctionDelegate openLibFun = Delegate.CreateDelegate(typeof(CSharpFunctionDelegate), mi.GetBaseDefinition()) as CSharpFunctionDelegate;
                //CSharpFunctionDelegate openLibFun = mi.CreateDelegate(typeof(CSharpFunctionDelegate)) as CSharpFunctionDelegate;
                LuaScriptDelegate.State.L_RequireF(c.Name + ".cs", openLibFun, false);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message);
            }
            return 1;
        }
        public static int Print(ILuaState lua)
        {
            var s = lua.L_CheckString(1); // 第一个参数
            Debug.Log(s);
            return 1; // 有一个返回值
        }
        public static int OSTime(ILuaState lua)
        {
            lua.PushNumber((double)DateUtils.NowMillisecond * 0.001);
            return 1; // 有一个返回值
        }
        public static int OSTimeMilliStr(ILuaState lua)
        {
            lua.PushString(DateUtils.NowMillisecond.ToString());
            return 1; // 有一个返回值
        }
    }
}
