using UnityEngine;
using Scripting.LuaSupport.DataTable;
using Scripting.UniLua;

namespace Scripting.LuaSupport
{
    public class LuaLogic : Logic.LuaClientLogicProxy<LuaLogic>
    {
        public string ServerHostCenterHost = "";
        public ushort ServerHostCenterPort = 0;
        public string ServerHostLoginAccount = "";
        public string ServerHostLoginPassword = "";
        protected override void OnEntryFileOvered(LuaTable entryTbl)
        {
            // load params
            LuaTable tbl = LuaReadHelper.GetSubTable(entryTbl, "__ClientParams");
            if(null == tbl)
            {
                Debug.LogError("找不到参数表__ClientParams！！！");
                return;
            }
            ServerHostCenterHost = LuaReadHelper.ReadUtf8String(tbl, "ServerHostCenterHost");
            if (!LuaReadHelper.ReadNumber(tbl, "ServerHostCenterPort", out ServerHostCenterPort))
            {
                Debug.LogError("读取参数 ServerHostCenterPort 失败！！！");
                return;
            }
            ServerHostLoginAccount = LuaReadHelper.ReadUtf8String(tbl, "ServerHostLoginAccount");
            ServerHostLoginPassword = LuaReadHelper.ReadUtf8String(tbl, "ServerHostLoginPassword");
        }
    }
}
