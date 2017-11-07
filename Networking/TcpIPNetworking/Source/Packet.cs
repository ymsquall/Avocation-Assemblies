using Framework.Tools;
using System;
using System.IO;

namespace Networking.TcpIPNetworking
{
    public enum SendMsgType
    {
        C2S_FastLogin               = 1000,
        C2S_Register                = 1001,
        C2S_Login                   = 1002,
        C2S_LoginToWorld            = 2000,
        C2S_PlayGame                = 2010,
        C2S_CancelPlayGame          = 2014, // 取消匹配
        C2S_CreateChara             = 2003,
        C2S_ModifyPassword          = 2005, // 修改密码
        C2S_ChangeHero              = 2201,
        C2S_Chat                    = 3001,
        C2S_TroopInvite             = 4001,
        C2S_TroopResponse           = 4003,
        C2S_KickoutTroopMember      = 4005,
        C2S_LeaveTropp              = 4006,
        C2S_CreateRoom              = 4200,
        C2S_JoinRoom                = 4201,
        C2S_LeaveRoom               = 4202,
        C2G_RoomReady               = 4203,
        C2S_RoomInvite              = 4204,
        C2S_RoomResponse            = 4205,
        C2S_UpdateRoomConfig        = 4206,
        C2S_HostStarted             = 4207,


        C2S_FriendInvite			= 5001,
		C2S_FriendResponse			= 5003,
		C2S_RemoveFriend			= 5005,
		C2S_ShieldPlayer			= 5010,
        // mail
        C2S_GetMailInfo             = 5032,     // 玩家提取邮件
        C2S_ClickMailLink           = 5033,     // 玩家点击邮件链接
        C2S_GetMotionInfo           = 5040,     // 获取统计数据
        C2S_GetMotionList           = 5041,
        // to match
        C2S_EnterGame               = 6000,
        C2S_ReadyGo                 = 6003,
        C2S_StartRuning             = 6004,
        C2S_SyncUserPowers          = 6010,
        C2S_SyncUserPosition        = 6011,
        C2S_UserLeaveRoom           = 6016,
        C2S_UsingSkill              = 7002,
        C2S_RemoveBuff              = 8002,
        C2S_EnterGameRoom           = 9200,
        C2S_GameRoomPlayerReady     = 9201,
        // common
        C2S_Heart                   = 100000,
    }
    public enum RecvMsgType
    {
        S2C_Register                = 1101,
        S2C_Login                   = 1102,
        S2C_LoginToWorld            = 2100,
        S2C_EnterWorld              = 2101,
        S2C_LeaveWorld              = 2102,
        S2C_CharaList               = 2103,
        S2C_LocalUserInfo           = 2104,
        S2C_ModifyPassword          = 2105, // 修改密码结果
        S2C_PlayGame                = 2110,
        S2C_BaseInfoChanged         = 2111, // 基本信息变化
        S2C_HeroInfoChanged         = 2112, // 英雄信息变化
        S2C_CancelPlayGame          = 2114, // 取消结果
        S2C_InMatchQueuePlayerCount = 2115,
        S2C_ChangeHero              = 2301,
        S2C_Chat                    = 3101,
        S2C_TroopMemberList         = 4000,
        S2C_TroopInvite             = 4101,
        S2C_TroopInviteResult       = 4102,
        S2C_TroopResponse           = 4103,
        S2C_TroopResponseResult     = 4104,
        S2C_KickoutTroopMember      = 4105,
        S2C_LeaveTroop              = 4106,
        S2C_ChangeTroopOwner        = 4107,
        S2C_RoomList                = 4300,
        S2C_RoomListUpdate          = 4301,
        S2C_JoinRoomFailed          = 4302,
        S2C_RoomMemberInfo          = 4303,
        S2C_RoomInvite              = 4304,
        S2C_RoomResponse            = 4305,
        S2C_EnterGameRoom           = 4306,
        S2C_LeaveGameRoom           = 4307,
        S2C_RoomInviteFailed        = 4308,     // 邀请玩家失败
        S2C_StartRoomGame           = 4309,     // 房间内开始游戏
        S2C_UpdateRoomConfig        = 4310,
        S2C_HostStarted             = 4311,

        S2C_FriendList = 5000,
		S2C_FriendInvite			= 5101,
		S2C_FriendInviteResult		= 5102,
		S2C_FriendResponse			= 5103,
		S2C_FriendResponseResult	= 5104,
		S2C_RemoveFriendResult		= 5106,
		S2C_FriendInfoChanged		= 5109,
        S2C_FriendInviteList        = 5120,
        // mail
        S2C_MailList                = 5130,     // 同步邮件列表
        S2C_UpdateMail              = 5131,     // 邮件信息发生改变
        S2C_GetMotionInfo           = 5140,     // 返回统计数据
        S2C_GetMotionList           = 5141,
        // from match
        S2C_EnterPlayRoomResult     = 6100,
        S2C_EnterPlayRoom           = 6101,
        S2C_LeavePlayRoom           = 6102,
        S2C_ReadyGo                 = 6103,
        S2C_StartRuning             = 6104,
        S2C_SyncUserPowers          = 6110,
        S2C_SyncUserPosition        = 6111,
        S2C_UserArrivedInGoal       = 6112,
        S2C_UpdateUserScore         = 6116,
        S2C_LoginSign               = 6120,
        S2C_BeginGenSkill           = 7100,
        S2C_OveredGenSkill          = 7101,
        S2C_UsedSkill               = 7102,
        S2C_OnSkillHit              = 7103,
        S2C_OnAddBuff               = 8101,
        S2C_OnRemoveBuff            = 8102,
        S2C_OnBuffInfoChange        = 8103,
        S2C_PropValueChanged        = 9100,
        S2C_OnAoeObjectAdded        = 9101,
        S2C_OnAoeHitingUser         = 9102,
        S2C_OnAoeObjectRemoved      = 9103,
        S2C_EnterGameRoom_Game      = 9300,
        S2C_GameRoomPlayerReady     = 9301,
        // common
        S2C_Heart                   = 100100,
        S2C_LocalAsyncToMainThread  = 200100,
        S2C_Hint                    = 300100,
        S2C_ChangeScene             = 400100,
    }
   
    public class Packet
    {
        public const int HeaderSize = sizeof(UInt32);
        public const int MsgIDSize = sizeof(UInt32);
        static byte[] UnCompressOutBuff = new byte[1024 * 32];

        //static Stream CompressMsg(MemoryStream s)
        //{
        //    MemoryStream ms = new MemoryStream();
        //    GZipOutputStream gzip = new GZipOutputStream(ms);
        //    byte[] binary = s.ToArray();
        //    gzip.Write(binary, 0, binary.Length);
        //    gzip.Close();
        //    //byte[] press = ms.ToArray();
        //    //Debug.Log(Convert.ToBase64String(press) + "  " + press.Length);
        //    return ms;
        //}
        public static byte[] UnCompressMsg(byte[] compBuff, int offset, ref int outLen)
        {
            using(MemoryStream outStream = new MemoryStream())
            {
                using (ComponentAce.Compression.Libs.zlib.ZOutputStream zStream = new ComponentAce.Compression.Libs.zlib.ZOutputStream(outStream))
                {
                    zStream.Write(compBuff, offset, compBuff.Length - 1);
                    zStream.Flush();
                    outStream.Seek(0, SeekOrigin.Begin);
                    //byte[] outBuff = new byte[outStream.Length];
                    outLen = (int)outStream.Length;
                    if (outLen > UnCompressOutBuff.Length)
                        UnCompressOutBuff = new byte[outLen];
                    outStream.Read(UnCompressOutBuff, 0, outLen);
                    //zStream.Close();
                }
                //outStream.Close();
            }
            return UnCompressOutBuff;
        }
        public static IS2C_Msg CreateS2CPacket(Stream s)
        {
            IS2C_Msg msg = null;
            s.Seek(0, SeekOrigin.Begin);
            byte packed = 0;
            StreamUtils.Read(s, ref packed);
            if (packed != 0)
            {
                s.Seek(0, SeekOrigin.Begin);
                byte[] compBuff = new byte[s.Length];
                s.Read(compBuff, 0, compBuff.Length);
                int outLen = 0;
                byte[] outData = UnCompressMsg(compBuff, 1, ref outLen);
                s = new MemoryStream(outData, 0, outLen);
            }
            Int32 id = 0;
            StreamUtils.Read(s, ref id);
            RecvMsgType msgType = (RecvMsgType)id;

#if DEBUG_OUTPUT
            if (msgType != RecvMsgType.生物移动 &&
                msgType != RecvMsgType.生物跳跃 &&
                msgType != RecvMsgType.生物攻击 &&
                msgType != RecvMsgType.战斗事件)
            {
                TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CPromptMode.调试信息, string.Format("收到消息[{0}:{1}] size={2}",
                        id, msgType.ToString(), s.Length)));
            }
#endif
            if (msgType != RecvMsgType.S2C_Heart &&
                msgType != RecvMsgType.S2C_SyncUserPosition &&
                msgType != RecvMsgType.S2C_SyncUserPowers)
            {
                UnityEngine.Debug.Log("[收到消息]" + msgType.ToString());
            }
#if SERVER_HOST
            switch(id)
            {
                case (int)ServerHost.RecvMsgType.S2H_LoginToCenter:         msg = ServerHost.S2H_LoginToCenter.Builder(s); break;
                case (int)ServerHost.RecvMsgType.S2H_OnCreateServerRoom:    msg = ServerHost.S2H_OnCreateServerRoom.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnCreateRoom:          msg = ServerHost.S2H_OnCreateRoom.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnUpdateRoomInfo:      msg = ServerHost.S2H_OnUpdateRoomInfo.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnAddRoomMember:       msg = ServerHost.S2H_OnAddRoomMember.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnUpdateRoomMember:    msg = ServerHost.S2H_OnUpdateRoomMember.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnRemoveRoomMember:    msg = ServerHost.S2H_OnRemoveRoomMember.Builder(s); break;
                //case (int)ServerHost.RecvMsgType.S2H_OnRemoveRoom:          msg = ServerHost.S2H_OnRemoveRoom.Builder(s); break;
                case (int)RecvMsgType.S2C_Heart:                            TcpIPPacketQueue.RecvHeart(); break;
            }
#endif
            if (null == msg)
            {
                switch (msgType)
                {
                    case RecvMsgType.S2C_Heart: TcpIPPacketQueue.RecvHeart(); break;
                    case RecvMsgType.S2C_LocalAsyncToMainThread: LocalAsyncToMainThread.Builder(s); break;
                    case RecvMsgType.S2C_Hint: msg = S2C_Hint.Builder(s); break;
                }
            }
            if (null != msg)
            {
                msg.packed = packed;
                TcpIPMessageQueue.EnqueueRecv(msg);
            }
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
            s.Dispose();
#else
            s.Close();
#endif
            return msg;
        }
    }
}
