using System;
using System.Collections.Generic;
using System.Reflection;
using Scripting.UniLua;

namespace Scripting.LuaSupport.Logic
{
    public interface ILuaCallLib
    {
        string LibName { get; }
    }
    public class LuaCallLib<T> : ILuaCallLib
        where T : LuaCallLib<T>
    {
        public static readonly string LIBName = typeof(T).Name + ".cs";
        public string LibName { get { return LIBName; } }
        public static NameFuncPair[] Funcs
        {
            get
            {
                Type t = typeof(T);
                MethodInfo[] ms = t.GetMethods(BindingFlags.Public | BindingFlags.Static);
                List<NameFuncPair> funcList = new List<NameFuncPair>();
                for(int i = 0; i < ms.Length; ++ i)
                {
                    if (ms[i].Name == "OpenLib" || ms[i].Name.Contains("add_") || ms[i].Name.Contains("remove_"))
                        continue;
                    Delegate delg = Delegate.CreateDelegate(typeof(CSharpFunctionDelegate), ms[i].GetBaseDefinition());
                    funcList.Add(new NameFuncPair(ms[i].Name, delg as CSharpFunctionDelegate));
                }
                return funcList.ToArray();
            }
        }

        public static int OpenLib(ILuaState lua) // 库的初始化函数
        {
            lua.L_NewLib(Funcs);
            return 1;
        }
    }
}
