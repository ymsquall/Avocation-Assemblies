using Framework.Tools;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace Networking.TcpIPNetworking
{
    public class TcpIPPacketQueue : MonoBehaviour
    {
        public static Queue<byte[]> RecvPacketQueue = new Queue<byte[]>();
        public float mHeartPacketRate = 5f;
        MemoryStream mLastPacketStream = null;
        Int32 mLastPacketLength = 0;
        byte[] mLastNotEnoughSizeBuffer = null;
        static float mLastSendHeartTime = 0;
        static float mLaseRecvHeartTime = 0;
        static int mHeartTick = 0;
        public static TcpIPPacketQueue Inst
        {
            get
            {
                TcpIPPacketQueue inst = FindObjectOfType<TcpIPPacketQueue>();
                if (null != inst)
                    Destroy(inst.gameObject);
                GameObject obj = new GameObject("TcpIPPacketQueue");
                GameObject.DontDestroyOnLoad(obj);
                inst = obj.AddComponent<TcpIPPacketQueue>();
                return inst;
            }
        }
        void Start()
        {
            StartCoroutine("DoParsePacket");
            StartCoroutine("DoHeartEvent");
        }
        public static void SendHeart()
        {
            //LocalPlayer lp = LocalPlayer.Inst;
            //if (null != lp && lp.InSceneState)
            {
                if (mHeartTick >= 4)
                {
                    TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.Disconnection));
                    mHeartTick = 0;
                    return;
                }
            }
            TcpIPNetwork.Inst.SendMessage(C2S_HeartTicker.Create());
            mLastSendHeartTime = Time.realtimeSinceStartup;
            //if (null != lp && lp.InSceneState)
                mHeartTick++;
            Debug.Log("发送心跳包：" + mLastSendHeartTime.ToString() + ", cnt=" + mHeartTick.ToString());
        }
        public static void RecvHeart()
        {
            mHeartTick = 0;
            mLaseRecvHeartTime = Time.realtimeSinceStartup;
            //if (null != NoFightMainView.Inst)
            //    NoFightMainView.Inst.OnUpdatePing(mLaseRecvHeartTime - mLastSendHeartTime);
            //if (null != FightingMainView.Inst)
            //    FightingMainView.Inst.OnUpdatePing(mLaseRecvHeartTime - mLastSendHeartTime);
            Debug.Log("接收心跳包：" + mLaseRecvHeartTime.ToString() + ", ping=" + ((int)((mLaseRecvHeartTime - mLastSendHeartTime) * 1000f)).ToString());
        }
        IEnumerator DoHeartEvent()
        {
            while (true)
            {
				//while(null == LocalPlayer.Inst || LocalPlayer.Inst.InLoginState)
                {
                    //mHeartTick = 0;
                    //yield return 0;
                }
				yield return new WaitForSeconds(1);
				if(TcpIPNetwork.Inst.Connected)
				{
	                if (Time.realtimeSinceStartup - mLastSendHeartTime >= mHeartPacketRate)
	                    SendHeart();
				}
                //else if(LocalPlayer.Inst.InSceneState)
                //{
                //    PopupDialogView view = PopupDialogView.Popup(弹框类型.错误提示, (弹框按钮 t, object[] ps)=>
                //    {
                //        if(null != InSceneLogicModel.Inst)
                //            InSceneLogicModel.Inst.BackToLoginScene(弹框按钮.确认, ps);
                //        return true;
                //    }, Text.SharedShowStrings.返回登陆界面.ToString(), Text.SharedShowStrings.与服务器断开连接.ToString() + "，" + 
                //        Text.SharedShowStrings.返回登陆界面.ToString());
                //    view.唯一 = true;
                //    view.唯一等待 = false;
                //}
            }
        }
        public static void EnqueuePacket(byte[] buff)
        {
            lock (RecvPacketQueue)
            {
                RecvPacketQueue.Enqueue(buff);
            }
        }
        static byte[] DequeuePacket()
        {
            lock (RecvPacketQueue)
            {
                if (RecvPacketQueue.Count > 0)
                    return RecvPacketQueue.Dequeue();
            }
            return null;
        }
        IEnumerator DoParsePacket()
        {
            while (true)
            {
                byte[] data = DequeuePacket();
                while (null != data)
                {
                    ParseRecvBuff(data);
                    data = DequeuePacket();
                }
                //yield return new WaitForFixedUpdate();
                //byte[] data = null;
                //if (RecvPacketQueue.Count > 0)
                //    data = RecvPacketQueue.Dequeue();
                //if (null != data)
                //    ParseRecvBuff(data);
                yield return 0;
                if (!TcpIPNetwork.Inst.Connected)
                    Clear();
            }
        }
        void ParseRecvBuff(byte[] data)
        {
            int readLength = 0;
            if (null == mLastPacketStream)
            {
                if(null != mLastNotEnoughSizeBuffer)
                {
                    List<byte> tmpBuff = new List<byte>();
                    tmpBuff.AddRange(mLastNotEnoughSizeBuffer);
                    tmpBuff.AddRange(data);
                    data = tmpBuff.ToArray();
                    mLastNotEnoughSizeBuffer = null;
                }
                if (data.Length < sizeof(Int32) + sizeof(Byte))
                {
#if DEBUG_OUTPUT
                    NGUI2DRootPanel.Inst.AddDebugText("收到的网络数据包长度小于4字节：" + data.Length.ToString());
#else
                    Debug.Log("收到的网络数据包长度小于4字节, 等待后续处理！");
#endif
                    mLastNotEnoughSizeBuffer = data;
                    return;
                }
                int headerLen = sizeof(Int32) + sizeof(Byte);
                int dataLength = data.Length - headerLen;
                Byte version = 0;
                StreamUtils.BitConvert(data, out version);
                StreamUtils.BitConvert(data, 1, out mLastPacketLength);
                mLastPacketStream = new MemoryStream();
                readLength = Math.Min(dataLength, mLastPacketLength);
                mLastPacketStream.Write(data, headerLen, readLength);
                mLastPacketLength -= readLength;
                readLength += headerLen;
#if DEBUG_OUTPUT
                string hits = "";
                Int32 msgID = 0;
                if (data.Length > 8)
                {
                    if(data[4] == 0)
                    {
                        List<byte> buff1 = new List<byte>();
                        buff1.AddRange(new byte[] { data[5], data[6], data[7], data[8] });
                        msgID = StreamUtils.BitConvert(buff1.ToArray(), out msgID);
                    }
                }
                string msgHints = "";
                if (msgID <= 0)
                    msgHints = string.Format("接收网络包[无法解析ID]剩余拆分长度[{0}]]", data.Length);
                else
                    msgHints = string.Format("接收网络包[{0}:{1}]", msgID, ((接收消息类型)msgID).ToString());
                if (mLastPacketLength == 0)
                {
                    hits = string.Format("<font size=\"18\" color=\"0000ff\" effect=\"o,1,1,00ff00\">{0}完成，大小[{1}]</font>",
                       msgHints, mLastPacketStream.Length);
                }
                else
                {
                    hits = string.Format("<font size=\"18\" color=\"00ff00\" effect=\"o,1,1,ff0000\">{0}，已接[{1}]，剩余[{2}]</font>",
                        msgHints, mLastPacketStream.Length, mLastPacketLength);
                }
                NGUI2DRootPanel.Inst.AddDebugText(hits);
#endif
            }
            else
            {
                readLength = Math.Min(data.Length, mLastPacketLength);
                mLastPacketStream.Write(data, 0, readLength);
                mLastPacketLength -= readLength;
#if DEBUG_OUTPUT
                string hits = "";
                Int32 msgID = 0;
                if (data[0] == 0)
                {
                    byte[] tmpBuff = mLastPacketStream.GetBuffer();
                    List<byte> buff1 = new List<byte>();
                    buff1.AddRange(new byte[] { tmpBuff[1], tmpBuff[2], tmpBuff[3], tmpBuff[4] });
                    msgID = StreamUtils.BitConvert(buff1.ToArray(), out msgID);
                }
                if (mLastPacketLength == 0)
                {
                    hits = string.Format("<font size=\"18\" color=\"0000ff\" effect=\"o,1,1,00ff00\">接收网络包[{0}:{1}]完成，大小[{2}]</font>",
                        msgID, ((接收消息类型)msgID).ToString(), mLastPacketStream.Length);
                }
                else
                {
                    hits = string.Format("<font size=\"18\" color=\"00ff00\" effect=\"o,1,1,ff0000\">接收网络包[{0}:{1}]，已接[{2}]，剩余[{3}]</font>",
                        msgID, ((接收消息类型)msgID).ToString(), mLastPacketStream.Length, mLastPacketLength);
                }
                NGUI2DRootPanel.Inst.AddDebugText(hits);
#endif
            }
            if (mLastPacketLength < 0)
            {
#if DEBUG_OUTPUT
                    NGUI2DRootPanel.Inst.AddDebugText("OnTcpIPClientReceived哪里出问题了：" + mLastPacketLength.ToString());
#else
                    Debug.LogWarning("OnTcpIPClientReceived哪里出问题了！！！");
#endif
                return;
            }
            if (mLastPacketLength == 0)
            {
                Packet.CreateS2CPacket(mLastPacketStream);
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                mLastPacketStream.Dispose();
#else
                mLastPacketStream.Close();
#endif
                mLastPacketStream = null;
                mLastPacketLength = 0;
            }
            if (data.Length > readLength)
            {
                // data没读完，继续处理
                byte[] buff = new byte[data.Length - readLength];
                //data.CopyTo(buff, readLength);
                Array.Copy(data, readLength, buff, 0, data.Length - readLength);
#if DEBUG_OUTPUT
                string hits = "";
                Int32 id = 0;
                if(buff.Length > 8)
                {
                    List<byte> buff1 = new List<byte>();
                    buff1.AddRange(new byte[] { buff[5], buff[6], buff[7], buff[8] });
                    id = StreamUtils.BitConvert(buff1.ToArray(), out id);
                }
                if (id > 0)
                {
                    hits = string.Format("<font size=\"18\" color=\"0099ff\" effect=\"o,1,1,ff5500\">拆包消息[{0}:{1}]剩余拆分长度[{2}]</font>",
                        id, ((接收消息类型)id).ToString(), data.Length - readLength - sizeof(Int32));
                }
                else
                {
                    hits = string.Format("<font size=\"18\" color=\"00ffff\" effect=\"o,1,1,ff0000\">拆包消息[无法解析ID]剩余拆分长度[{0}]]</font>",
                        data.Length - readLength - sizeof(Int32));
                }
                NGUI2DRootPanel.Inst.AddDebugText(hits);
#endif
                ParseRecvBuff(buff);
            }
            //if (mLastPacketLength > 0)
            //{
            //    // data读完了，不是一个完整的包，等下次继续
            //    return;
            //}
        }
        public void Clear()
        {
            lock (RecvPacketQueue)
            {
                if (RecvPacketQueue.Count <= 0)
                {
                    mLastPacketStream = null;
                    mLastPacketLength = 0;
                }
            }
        }
    }
}
