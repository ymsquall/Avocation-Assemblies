using System.Collections.Generic;

namespace Framework.Tools
{
    public interface IQueueData
    {
    }
    public interface IDataQueue<MT>
        where MT : IQueueData
    {
        object RecvQueueLocker { get; }
        object SendQueueLocker { get; }
        bool PushRecv(MT msg);
        bool PushSend(MT msg);
        MT PeekRecv();
        MT PeekSend();
        MT PopRecv();
        MT PopSend();
        void ClearRecv();
        void ClearSend();
        void Clear();
    }
    public class DataQueue<T, MT> : IDataQueue<MT>
        where T : DataQueue<T, MT>
        where MT : IQueueData
    {
        object mRecvQueueLocker = new object();
        Queue<MT> mRecvQueue = new Queue<MT>();
        object mSendQueueLocker = new object();
        Queue<MT> mSendQueue = new Queue<MT>();
        object IDataQueue<MT>.RecvQueueLocker { get { return mRecvQueueLocker; } }
        object IDataQueue<MT>.SendQueueLocker { get { return mRecvQueueLocker; } }
        public bool PushRecv(MT msg)
        {
            lock (mRecvQueueLocker)
            {
                mRecvQueue.Enqueue(msg);
            }
            return true;
        }
        public bool PushSend(MT msg)
        {
            lock (mSendQueueLocker)
            {
                mSendQueue.Enqueue(msg);
            }
            return true;
        }
        public MT PeekRecv()
        {
            MT ret = default(MT);
            lock (mRecvQueueLocker)
            {
                if (mRecvQueue.Count > 0)
                    ret = mRecvQueue.Peek();
            }
            return ret;
        }
        public MT PeekSend()
        {
            MT ret = default(MT);
            lock (mSendQueueLocker)
            {
                if (mSendQueue.Count > 0)
                    ret = mSendQueue.Peek();
            }
            return ret;
        }
        public MT PopRecv()
        {
            MT ret = default(MT);
            lock (mRecvQueueLocker)
            {
                if (mRecvQueue.Count > 0)
                    ret = mRecvQueue.Dequeue();
            }
            return ret;
        }
        public MT PopSend()
        {
            MT ret = default(MT);
            lock (mSendQueueLocker)
            {
                if (mSendQueue.Count > 0)
                    ret = mSendQueue.Dequeue();
            }
            return ret;
        }
        public void ClearRecv()
        {
            lock (mRecvQueueLocker)
            {
                mRecvQueue.Clear();
            }
        }
        public void ClearSend()
        {
            lock (mSendQueueLocker)
            {
                mSendQueue.Clear();
            }
        }
        public void Clear()
        {
            ClearRecv();
            ClearSend();
        }
    }
}
