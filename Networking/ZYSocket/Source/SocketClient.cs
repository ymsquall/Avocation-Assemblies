/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */
using System;
using System.Net;
using System.Net.Sockets;

namespace Networking.ZYSocket
{
    public delegate void ConnectionOk(string message,bool IsConn);
    public delegate void DataOn(byte[] Data);
    public delegate void ExceptionDisconnection(string message);

    /// <summary>
    /// ZYSOCKET 客户端
    /// （一个简单的异步SOCKET客户端，性能不错。支持.NET 3.0以上版本。适用于silverlight)
    /// </summary>
    public class SocketClient
    {
        public static int SimulateSendLagSleepTime = 0;
        public static int SimulateRecvLagSleepTime = 0;
        public static int SimulateConnectLagSleepTime = 0;
        /// <summary>
        /// SOCKET对象
        /// </summary>
        protected Socket sock;

        /// <summary>
        /// 连接成功事件
        /// </summary>
        public event ConnectionOk Connection;

        /// <summary>
        /// 数据包进入事件
        /// </summary>
        public event DataOn DataOn;
        //object mRecvLocked = new object();
        object mSendLocked = new object();
        /// <summary>
        /// 出错或断开触发事件
        /// </summary>
        public event ExceptionDisconnection Disconnection;

        private System.Threading.AutoResetEvent wait = new System.Threading.AutoResetEvent(false);
        
        public SocketClient()
        {           
            sock = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            //sock.ReceiveBufferSize = 100000;
            sock.NoDelay = true;
        }

        private bool IsConn;

        /// <summary>
        /// 异步连接到指定的服务器
        /// </summary>
        /// <param name="host"></param>
        /// <param name="port"></param>
        public void BeginConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
            try
            {
                myEnd = new IPEndPoint(IPAddress.Parse(host), port);
            }
            catch (FormatException)
            {
                IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress s in p.AddressList)
                {
                    if (!s.IsIPv6LinkLocal)
                        myEnd = new IPEndPoint(s, port);
                }
            }

            #endregion

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = myEnd;
            //byte[] dataLast = new byte[80000];
            //e.SetBuffer(dataLast, 0, dataLast.Length);
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }
        }

        public bool ConnectionTo(string host, int port)
        {
            IPEndPoint myEnd = null;

            #region ipformat
            try
            {
                myEnd = new IPEndPoint(IPAddress.Parse(host), port);
            }
            catch (FormatException)
            {
                IPHostEntry p = Dns.GetHostEntry(Dns.GetHostName());

                foreach (IPAddress s in p.AddressList)
                {
                    if (!s.IsIPv6LinkLocal)
                        myEnd = new IPEndPoint(s, port);
                }
            }

            #endregion

            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.RemoteEndPoint = myEnd;
            //byte[] dataLast = new byte[4098];
            //e.SetBuffer(dataLast, 0, dataLast.Length);
            e.Completed += new EventHandler<SocketAsyncEventArgs>(e_Completed);
            if (!sock.ConnectAsync(e))
            {
                eCompleted(e);
            }

            wait.WaitOne();
            wait.Reset();

            return IsConn;
        }



        void e_Completed(object sender, SocketAsyncEventArgs e)
        {
            eCompleted(e);
        }


        void eCompleted(SocketAsyncEventArgs e)
        {
            switch (e.LastOperation)
            {
                case SocketAsyncOperation.Connect:
                    {
                        //Thread.Sleep(SimulateConnectLagSleepTime);
                        if (e.SocketError == SocketError.Success)
                        {

                            IsConn = true;
                            wait.Set();

                            if (Connection != null)
                                Connection("连接成功", true);

                            byte[] data = new byte[4098];
                            e.SetBuffer(data, 0, data.Length);  //设置数据包

                            if (!sock.ReceiveAsync(e)) //开始读取数据包
                                eCompleted(e);
                        }
                        else
                        {
                            IsConn = false;
                            wait.Set();
                            if (Connection != null)
                                Connection("连接失败", false);
                        }
                    }
                    break;
                case SocketAsyncOperation.Receive:
                    {
                        //Thread.Sleep(SimulateRecvLagSleepTime);
                        //lock (mRecvLocked)
                        {
                            if (e.SocketError == SocketError.Success && e.BytesTransferred > 0)
                            {
                                byte[] data = new byte[e.BytesTransferred];
                                Array.Copy(e.Buffer, 0, data, 0, data.Length);

                                byte[] dataLast = new byte[4098];
                                e.SetBuffer(dataLast, 0, dataLast.Length);

                                if (!sock.ReceiveAsync(e))
                                    eCompleted(e);

                                if (DataOn != null)
                                    DataOn(data);
                            }
                            else if(e.BytesTransferred <= 0 || e.SocketError != SocketError.Success)
                            {
                                //TcpIPMessageQueue.EnqueueRecv(S2C_Prompt.Builder(S2CPromptMode.调试信息, "接收的消息包非法：size=" + e.BytesTransferred.ToString() + ",socketerr=" + e.SocketError.ToString()));
                                if (Disconnection != null)
                                    Disconnection("与服务器断开连接");
                            }
                        }
                    }
                    break;
                case SocketAsyncOperation.Disconnect:
                    {
                        //TcpIPMessageQueue.EnqueueRecv(S2C_Prompt.Builder(S2CPromptMode.调试信息, "收到Socket事件Disconnect。"));
                        if (Disconnection != null)
                            Disconnection("与服务器断开连接");
                    }
                    break;
            }
        }

        /// <summary>
        /// 发送数据包
        /// </summary>
        /// <param name="data"></param>
        public void SendTo(byte[] data)
        {
            SocketAsyncEventArgs e = new SocketAsyncEventArgs();
            e.SetBuffer(data, 0, data.Length);
            lock (mSendLocked)
            {
                sock.SendAsync(e);
            }
        }
        public virtual void Close()
        {
            //sock.Shutdown(SocketShutdown.Both);
            sock.Close();
        }
    }
}
