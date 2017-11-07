using Framework.Tools;
using System;
using System.IO;

namespace Networking.TcpIPNetworking
{
    public enum LocalAsyncToMainThreadType
    {
        BackToLogin = 0,
        Disconnection = 1,
        MatchDisconnection = 2,
        UnlockScreen = 3,
        max
    }
    public class LocalAsyncToMainThread : IS2C_Msg
    {
        public override Int32 MessageID { get { return (Int32)RecvMsgType.S2C_LocalAsyncToMainThread; } }
        public override bool CanDroped { get { return false; } }
        public LocalAsyncToMainThreadType type;		// int      模式。0系统信息 1错误信息 2滚动公告 3电视公告
        public override string ToString()
        {
            return string.Format("[{0}]", type.ToString());
        }
        public static IS2C_Msg Builder(Stream buffer)
        {
            var msg = new LocalAsyncToMainThread();
            Byte type = 0;
            StreamUtils.Read(buffer, ref type);
            msg.type = (LocalAsyncToMainThreadType)type;
            return msg;
        }
        public static IS2C_Msg Builder(LocalAsyncToMainThreadType type)
        {
            var msg = new LocalAsyncToMainThread();
            msg.type = type;
            return msg;
        }
    }
}
