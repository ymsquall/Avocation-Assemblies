﻿/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */

using System;
using System.Collections.Generic;
using System.Net.Sockets;

namespace Networking.ZYSocket
{
    /// <summary>
    /// Based on example from http://msdn2.microsoft.com/en-us/library/system.net.sockets.socketasynceventargs.socketasynceventargs.aspx
    /// Represents a collection of reusable SocketAsyncEventArgs objects.  
    /// </summary>
    internal sealed class SocketAsyncEventArgsPool
    {
        /// <summary>
        /// SocketAsyncEventArgs栈
        /// </summary>
        Stack<SocketAsyncEventArgs> pool;

        /// <summary>
        /// 初始化SocketAsyncEventArgs池
        /// </summary>
        /// <param name="capacity">最大可能使用的SocketAsyncEventArgs对象.</param>
        internal SocketAsyncEventArgsPool(Int32 capacity)
        {
            this.pool = new Stack<SocketAsyncEventArgs>(capacity);
        }

        /// <summary>
        /// 返回SocketAsyncEventArgs池中的 数量
        /// </summary>
        internal Int32 Count
        {
            get { return this.pool.Count; }
        }

        /// <summary>
        /// 弹出一个SocketAsyncEventArgs
        /// </summary>
        /// <returns>SocketAsyncEventArgs removed from the pool.</returns>
        internal SocketAsyncEventArgs Pop()
        {
            lock (this.pool)
            {
                return this.pool.Pop();
            }
        }

        /// <summary>
        /// 添加一个 SocketAsyncEventArgs
        /// </summary>
        /// <param name="item">SocketAsyncEventArgs instance to add to the pool.</param>
        internal void Push(SocketAsyncEventArgs item)
        {
            if (item == null)
            {
                throw new ArgumentNullException("Items added to a SocketAsyncEventArgsPool cannot be null");
            }
            lock (this.pool)
            {
                this.pool.Push(item);
            }
        }
    }
}
