using System;
using System.Collections.Generic;
using UnityEngine;
using Framework.Tools;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
//using Threading_WP_8_1;
#else
using System.Threading;
#endif


namespace Networking.TcpIPNetworking
{
    public class TcpIPNetwork : AutoSingleT<TcpIPNetwork>
    {
        private ITcpNetworkingModel mNetworkingModel = null;
        private TcpIPClient mLocalClient = null;
        private TcpIPClient mMatchClient = null;
        private Dictionary<Int32, List<ITcpNetLogicModel>> mLogicModelRecvDelegates = new Dictionary<Int32, List<ITcpNetLogicModel>>();
        private List<Int32> mWaitUnlockMsgs = new List<Int32>();

        public bool CanWorking { get { return null != mNetworkingModel; } }

        public bool LoginConnected { get { return (mLocalClient != null && mLocalClient.Connected); } }
        public bool Connected
        {
            get
            {
                if (mLocalClient != null && mLocalClient.Connected)
                    return true;
                if (mMatchClient != null && mMatchClient.Connected)
                    return true;
                return false;
            }
        }

        public void Init(ITcpNetworkingModel model)
        {
            if(null == model)
            {
                throw new Exception("TcpIPNetwork.Init:ITcpNetworkingModel can not be null");
            }
            mNetworkingModel = model;
        }

        public void RegisterRecvDelegate(ITcpNetLogicModel model)
        {
            for (int i = 0; i < model.CanRecvMessages.Length; ++ i)
            {
                var id = (Int32)model.CanRecvMessages[i];
                List<ITcpNetLogicModel> models = null;
                if (!mLogicModelRecvDelegates.ContainsKey(id))
                {
                    models = new List<ITcpNetLogicModel>();
                    models.Add(model);
                    mLogicModelRecvDelegates.Add(id, models);
                    //Debug.Log(string.Format("创建模块消息接收列表[{0}]，注册接收消息[{1}]", model.ToString(), ((接收消息类型)id).ToString()));
                }
                else
                {
                    models = mLogicModelRecvDelegates[id];
                    if (!models.Contains(model))
                        models.Add(model);
                    //Debug.Log(string.Format("为模块[{0}]注册接收消息[{1}]", model.ToString(), ((接收消息类型)id).ToString()));
                }
            }
        }
        public void UnRegisterRecvDelegate(ITcpNetLogicModel model)
        {
            for (int i = 0; i < model.CanRecvMessages.Length; ++i)
            {
                var id = (Int32)model.CanRecvMessages[i];
                List<ITcpNetLogicModel> models = null;
                if (mLogicModelRecvDelegates.ContainsKey(id))
                {
                    models = mLogicModelRecvDelegates[id];
                    models.Remove(model);
                    //Debug.Log(string.Format("移除模块[{0}]接收消息[{1}]", model.ToString(), ((接收消息类型)id).ToString()));
                }
            }
        }
        public void UnLockNetMsgScreenLocked()
        {
            mWaitUnlockMsgs.Clear();
            mNetworkingModel.TcpNetUnlockScreen();
        }
        public void AsyncUnLockNetMsgScreenLocked()
        {
            lock(mWaitUnlockMsgs)
            {
                mWaitUnlockMsgs.Clear();
            }
            TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.UnlockScreen));
        }
        public void CheckRecvedMessage(ITcpNetLogicModel l)
        {
            if (!CanWorking)
            {
                throw new Exception("CheckRecvedMessage:this tcp networking cannot working, because mNetworkingModel is null");
            }
            IS2C_Msg msg = TcpIPMessageQueue.PeekRecv();
            while(null != msg)
            {
                bool handled = true;
                bool breaked = false;
                if (msg.MessageID == (int)RecvMsgType.S2C_ChangeScene)
                    UnLockNetMsgScreenLocked();
                else
                {
                    if (mWaitUnlockMsgs.Contains(msg.MessageID))
                        mWaitUnlockMsgs.Remove(msg.MessageID);
                    if (mWaitUnlockMsgs.Count == 0)
                        UnLockNetMsgScreenLocked();
                }
                if (!l.HandleMessage(msg, ref handled, ref breaked))
                {
                    Debug.LogError("处理消息[" + (RecvMsgType)msg.MessageID + "]失败!!!");
                    TcpIPMessageQueue.DequeueRecv();
                }
                if (handled)
                    TcpIPMessageQueue.DequeueRecv();
                else
                {
                    Debug.LogError("标记为未处理的消息[" + (RecvMsgType)msg.MessageID + "]");
                    if(msg.CanDroped)
                        TcpIPMessageQueue.DequeueRecv();
                    break;
                }
                if (breaked)
                {
                    Debug.LogWarning("跳出本次消息处理[" + (RecvMsgType)msg.MessageID + "]");
                    break;
                }
                msg = TcpIPMessageQueue.PeekRecv();
            }
        }
        
        public bool ConnectServer(string address, int port)
        {
            if(!CanWorking)
            {
                throw new Exception("ConnectServer:this tcp networking cannot working, because mNetworkingModel is null");
            }
            if (null != mLocalClient)
            {
                Debug.LogWarning(string.Format("关闭当前连接后才能与服务器[{0}:{1}]建立连接！", address, port));
                Close(false);
            }
            mLocalClient = new TcpIPClient();
            mLocalClient.OnDisConnected += new TcpIPClient.DisConnected((string message) =>
            {
                mLocalClient.Close();
                mLocalClient = null;
                TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.Disconnection));
            });
            mLocalClient.ConnectionTo(address, port);
            return true;
        }
        public bool ConnectMatchServer(string address, int port)
        {
            if (!CanWorking)
            {
                throw new Exception("ConnectMatchServer:this tcp networking cannot working, because mNetworkingModel is null");
            }
            if (null != mMatchClient)
            {
                Debug.LogWarning(string.Format("关闭当前连接后才能与服务器[{0}:{1}]建立连接！", address, port));
                CloseMatch(false);
            }
            mMatchClient = new TcpIPClient();
            mMatchClient.OnDisConnected += new TcpIPClient.DisConnected((string message) =>
            {
                mMatchClient.Close();
                mMatchClient = null;
                TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.MatchDisconnection));
            });
            mMatchClient.ConnectionTo(address, port);
            return true;
        }
        public bool SendMessage(IC2S_Msg msg)
        {
            if (!CanWorking)
            {
                throw new Exception("SendMessage:this tcp networking cannot working, because mNetworkingModel is null");
            }
            List<TcpIPClient> clientList = new List<TcpIPClient>();
            if (msg.IsCommonMsg)
            {
                if(null != mLocalClient)
                    clientList.Add(mLocalClient);
                if(null != mMatchClient)
                    clientList.Add(mMatchClient);
            }
            else if (msg.IsMatchMsg)
                clientList.Add(mMatchClient);
            else
                clientList.Add(mLocalClient);
            for(int i = 0; i < clientList.Count; ++ i)
            {
                if (null == clientList[i])
                {
                    Debug.LogWarning(string.Format("发送[{0}]消息时还未与服务器建立连接！", ((SendMsgType)msg.MessageID).ToString()));
                    UnLockNetMsgScreenLocked();
                    return false;
                }
                if (msg.NeedLockScreen)
                {
                    if (!mWaitUnlockMsgs.Contains(msg.MessageID))
                        mWaitUnlockMsgs.Add(msg.UnLockedID);
                    mNetworkingModel.TcpNetLockScreen();
                }
                if (!clientList[i].SendMsg(msg))
                {
                    UnLockNetMsgScreenLocked();
                    return false;
                }
            }
            return true;
        }
        public bool Close(bool hint = true)
        {
            if (null != mLocalClient)
            {
                if (!CanWorking)
                {
                    throw new Exception("Close:this tcp networking cannot working, because mNetworkingModel is null");
                }
                if (hint)
                    mLocalClient.Close();
                else
                    mLocalClient.CloseNotCallback();
                while (null != mLocalClient && mLocalClient.Connected)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    continue;
#else
                    Thread.Sleep(1);
#endif
                mLocalClient = null;
                if(hint)
                {
                    TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.Disconnection));
                }
                AsyncUnLockNetMsgScreenLocked();
            }
            return true;
        }
        public bool CloseMatch(bool hint = true)
        {
            if (null != mMatchClient)
            {
                if (!CanWorking)
                {
                    throw new Exception("CloseMatch:this tcp networking cannot working, because mNetworkingModel is null");
                }
                if (hint)
                    mMatchClient.Close();
                else
                    mMatchClient.CloseNotCallback();
                while (null != mMatchClient && mMatchClient.Connected)
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                    continue;
#else
                    Thread.Sleep(1);
#endif
                mMatchClient = null;
                if (hint)
                {
                    TcpIPMessageQueue.EnqueueRecv(LocalAsyncToMainThread.Builder(LocalAsyncToMainThreadType.MatchDisconnection));
                }
                AsyncUnLockNetMsgScreenLocked();
            }
            return true;
        }
    }
}
