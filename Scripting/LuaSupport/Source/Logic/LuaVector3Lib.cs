using Scripting.UniLua;
using UnityEngine;

namespace Scripting.LuaSupport.Logic
{
    using DataTable;
    public class LuaVector3Lib : LuaCallLib<LuaVector3Lib>
    {
        //public delegate bool EventHandler(params object[] ps);
        //public static event EventHandler OnLuaCallback;
        public static int LuaVector3Lib_Test(ILuaState lua)
        {
            var a = lua.L_CheckNumber(1); // 第一个参数
            var b = lua.L_CheckNumber(2); // 第二个参数
            var c = a - b; // 执行减法操作
            lua.PushNumber(c); // 将返回值入栈
            return 1; // 有一个返回值
        }
        public static int Length(ILuaState lua)
        {
            if (!lua.IsTable(-1))
            {
                lua.PushNumber(0);
                return 1;
            }
            Vector3 srcPos = LuaDataReadHelper.GetLuaVector3(lua);
            Vector3 dstPos = LuaDataReadHelper.GetLuaVector3(lua);
            double len = (srcPos - dstPos).magnitude;
            lua.PushNumber(len);
            return 1;
        }
        public static int Angle(ILuaState lua)
        {
            if (!lua.IsTable(-1))
            {
                lua.PushNumber(0);
                return 1;
            }
            Vector3 srcPos = LuaDataReadHelper.GetLuaVector3(lua);
            Vector3 dstPos = LuaDataReadHelper.GetLuaVector3(lua);
            Vector3 oriPos = LuaDataReadHelper.GetLuaVector3(lua);
            Vector3 oriDir = dstPos - oriPos;
            Vector3 nowDir = dstPos - srcPos;
            oriDir.Normalize(); nowDir.Normalize();
            double angle = Vector3.Angle(oriDir, nowDir);
            lua.PushNumber(angle);
            return 1;
        }
        public static int Translate(ILuaState lua)
        {
            if (!lua.IsTable(-1))
            {
                lua.PushNumber(0);
                lua.PushNumber(0);
                lua.PushNumber(0);
                return 3;
            }
            Vector3 srcPos = LuaDataReadHelper.GetLuaVector3(lua);
            Vector3 dir = LuaDataReadHelper.GetLuaVector3(lua);
            dir.Normalize();
            float delta = (float)lua.L_CheckNumber(7);
            srcPos = srcPos + dir * delta;
            lua.PushNumber(srcPos.x);
            lua.PushNumber(srcPos.y);
            lua.PushNumber(srcPos.z);
            return 3;
        }
    }
}
