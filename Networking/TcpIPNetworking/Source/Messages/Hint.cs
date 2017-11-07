using Framework.Tools;
using System;
using System.IO;

namespace Networking.TcpIPNetworking
{
    public enum S2CHintType
    {
        System = 0,
        Error = 1,
        Scroll = 2,
        TV = 3,
        BackToLogin = 101,
        BackToMatch = 102,
        BackToMatchHint = 103,
        Debug = 104,
        Intro = 105,
        max
    }
    public class S2C_Hint : IS2C_Msg
    {
        public override Int32 MessageID { get { return (Int32)RecvMsgType.S2C_Hint; } }
        public override bool CanDroped { get { return true; } }
        public S2CHintType mode;		// int      模式。0系统信息 1错误信息 2滚动公告 3电视公告
        String msg;		        // String	信息
        public string Hints { get { return msg; } }
        public bool IsError { get { return mode == S2CHintType.Error; } }
        public override string ToString()
        {
            return string.Format("[{0}]:{1}", mode.ToString(), msg);
        }
        public static IS2C_Msg Builder(Stream buffer)
        {
            var msg = new S2C_Hint();
            Byte mode = 0;
            StreamUtils.Read(buffer, ref mode);
            msg.mode = (S2CHintType)mode;
            StreamUtils.Read(buffer, ref msg.msg);
            //if(msg.mode == S2CPromptMode.滚动公告)
            //    UnityEngine.Debug.LogWarning("[滚动公告]" + msg.msg);
            return msg;
        }
        public static IS2C_Msg Builder(S2CHintType mode, string hints)
        {
            var msg = new S2C_Hint();
            msg.mode = mode;
            msg.msg = hints;
            return msg;
        }
    }
}
