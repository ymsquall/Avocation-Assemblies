using System.IO;
using UnityEngine;
using Networking.ZYSocket;

namespace Networking.TcpIPNetworking
{
    class TcpIPClient : SocketClient
    {
        public TcpIPClient()
        {
            Connection += new ConnectionOk(OnTcpIPClientConnected);
            DataOn += new DataOn(OnTcpIPClientReceived);
            Disconnection += new ExceptionDisconnection(OnTcpIPClientDisconnected);
        }

        public delegate void DisConnected(string message);
        public event DisConnected OnDisConnected;

        bool mConnected = false;
        public bool Connected { get { return mConnected; } }
        void OnTcpIPClientConnected(string message, bool IsConn)
        {
            mConnected = IsConn;
            if (IsConn)
            {
                TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Intro, "[OnTcpIPClientConnected1]" + message));
                //mSendMessageThread = new Thread(new ParameterizedThreadStart(SendMessageThreading));
                //mSendMessageThread.Start(this);

            }
            else
            {
                TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Intro, "[ff0000][OnTcpIPClientConnected2]" + message));
                TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Error, message));
            }
        }
        void OnTcpIPClientReceived(byte[] data)
        {
            TcpIPPacketQueue.EnqueuePacket(data);
        }
        void OnTcpIPClientDisconnected(string message)
        {
            //if (null != mSendMessageThread)
            //{
            //    mSendMessageThread.Abort();
            //    mSendMessageThread = null;
            //    while (TcpIPMessageQueue.DequeueSend() != null) ;
            //}
            mConnected = false;
            //if (LocalPlayer.Inst.InLoginState)
            //    TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CPromptMode.Intro信息, "[OnTcpIPClientDisconnected]" + message));
            //else
                TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Error, message));
            if (null != OnDisConnected)
                OnDisConnected(message);
        }
        public override void Close()
        {
            //if (null != mSendMessageThread)
            //{
            //    mSendMessageThread.Abort();
            //    mSendMessageThread = null;
            //    while (TcpIPMessageQueue.DequeueSend() != null) ;
            //}
            TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Intro, "[NetworkClose]:Nothing"));
            base.Close();
            mConnected = false;
            Connection -= new ConnectionOk(OnTcpIPClientConnected);
            DataOn -= new DataOn(OnTcpIPClientReceived);
            Disconnection -= new ExceptionDisconnection(OnTcpIPClientDisconnected);
        }
        public void CloseNotCallback()
        {
            Connection -= new ConnectionOk(OnTcpIPClientConnected);
            DataOn -= new DataOn(OnTcpIPClientReceived);
            Disconnection -= new ExceptionDisconnection(OnTcpIPClientDisconnected);
            TcpIPMessageQueue.EnqueueRecv(S2C_Hint.Builder(S2CHintType.Intro, "[NetworkClose]:Nothing"));
            base.Close();
            mConnected = false;
        }
        public bool SendMsg(IC2S_Msg msg)
        {
            if(!mConnected)
            {
                //PopupDialogView.Popup(弹框类型.错误提示, Text.SharedShowStrings.连接服务器失败.ToString() + "，" + Text.SharedShowStrings.请重试.ToString() + "。");
                Debug.LogWarning(string.Format("发送[{0}]消息时，与服务器的连接是断开的!!!", ((RecvMsgType)msg.MessageID).ToString()));
                return false;
            }
            Stream s = msg.Serializer;
            s.Seek(0, SeekOrigin.Begin);
            byte[] buffer = new byte[s.Length];
            s.Read(buffer, 0, buffer.Length);
            this.SendTo(buffer);
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
            s.Dispose();
#else
            s.Close();
#endif
            //TcpIPMessageQueue.EnqueueSend(msg);
            return true;
        }
        //static void SendMessageThreading(object client)
        //{
        //    while(Thread.CurrentThread.ThreadState == ThreadState.Running)
        //    {
        //        IC2S_Msg msg = TcpIPMessageQueue.DequeueSend();
        //        if (null != msg)
        //        {
        //            //Thread.Sleep(SocketClient.SimulateSendLagSleepTime);
        //            SocketClient pClient = client as SocketClient;
        //            Stream s = msg.Serializer;
        //            s.Seek(0, SeekOrigin.Begin);
        //            byte[] buffer = new byte[s.Length];
        //            s.Read(buffer, 0, buffer.Length);
        //            pClient.SendTo(buffer);
        //        }
        //        Thread.Sleep(1);
        //    }
        //}
    }
}
