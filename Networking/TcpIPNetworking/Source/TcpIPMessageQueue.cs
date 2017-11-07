using System;
using System.Collections.Generic;
using System.Threading;

namespace Networking.TcpIPNetworking
{
    public class TcpIPMessageQueue
    {
        static object mRecvLocker = new object();        // 添加一个对象作为锁
        static object mSendLocker = new object();        // 添加一个对象作为锁
        static Queue<IS2C_Msg> mReceivedMessageQueue = new Queue<IS2C_Msg>();
        static Queue<IC2S_Msg> mSendMessageQueue = new Queue<IC2S_Msg>();
        public static void EnqueueRecv(IS2C_Msg msg)
        {
            lock (mRecvLocker)
            {
                mReceivedMessageQueue.Enqueue(msg);
            }
        }
        public static IS2C_Msg PeekRecv()
        {
            lock (mRecvLocker)
            {
                if (mReceivedMessageQueue.Count > 0)
                    return mReceivedMessageQueue.Peek();
            }
            return null;
        }
        public static IS2C_Msg DequeueRecv()
        {
            lock (mRecvLocker)
            {
                if (mReceivedMessageQueue.Count > 0)
                    return mReceivedMessageQueue.Dequeue();
            }
            return null;
        }
        ///
        public static void EnqueueSend(IC2S_Msg msg)
        {
            lock (mSendLocker)
            {
                mSendMessageQueue.Enqueue(msg);
            }
        }
        public static IC2S_Msg DequeueSend()
        {
            lock (mSendLocker)
            {
                if (mSendMessageQueue.Count > 0)
                    return mSendMessageQueue.Dequeue();
            }
            return null;
        }
        public static void Clear()
        {
            lock (mSendLocker)
            {
                mSendMessageQueue.Clear();
            }
        }
    }
}
