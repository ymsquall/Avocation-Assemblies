/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */
using System;
using System.Collections.Generic;
using System.Threading;

namespace Networking.ZYSocket
{
    /// <summary>
    /// 数据包组合类
    /// 功能描述:保持数据包完整性。
    /// （通过互联网发送数据包，实际上是将一个较大的包拆分成诺干小包，此类的功能就是讲诺干小包重新组合成完整的数据包）
    /// 此类是线程安全的
    /// </summary>
    public class BuffList
    {
        public object locklist = new object();

        /// <summary>
        /// 数据包列表
        /// </summary>
        public List<byte> ByteList { get; set; }

        private int current;

        private int lengt;

        private int Vlent;

        /// <summary>
        /// 数据包有可能出现的最大长度。如果不想服务器被人攻击到内存崩溃请按实际情况设置
        /// </summary>
        public int MaxSize { get; set; }

        /// <summary>
        /// 数据包组合类
        /// </summary>
        /// <param name="maxSize">数据包有可能出现的最大长度。如果不想服务器被人攻击到内存崩溃请按实际情况设置</param>
        public BuffList(int maxSize)
        {
            MaxSize=maxSize;
            lengt = -1;
            Vlent = 0;
            ByteList = new List<byte>();
           
        }

        public void Reset()
        {
            Interlocked.Exchange(ref lengt, -1);
            Interlocked.Exchange(ref Vlent, 0);
            Interlocked.Exchange(ref current,0);
            ByteList.Clear();
            
        }

       

        public bool InsertByteArray(byte[] Data,int ml,out byte[] datax)
        {
            lock(locklist)
            {
                datax = null;
                              
                ByteList.AddRange(Data);

                Interlocked.Add(ref Vlent, Data.Length);

                
                if (lengt == -1 && Vlent > ml)
                {
                    int res = 0;

                    for (int i = 0; i < ml; i++)
                    {
                        int temp = ((int)ByteList[current+i]) & 0xff;
                        temp <<= i * 8;
                        res = temp + res;
                    }

                    if (res > MaxSize)
                    {
                        Reset();
                        throw new Exception("数据包大于预设长度，如果你传入的数据比较大，请设置重新 maxSize 值");
                    }

                    if (res <= 0)
                    {
                        Reset();
                       
                        return false;
                    }
                    
                    Interlocked.Exchange(ref lengt,res);
                }


                if ((Vlent - current) >= lengt)
                {

                    int lengx = lengt;
                    Interlocked.Exchange(ref lengt, -1);
                    datax = new byte[lengx];

                    ByteList.CopyTo(current, datax, 0, lengx);

                    Interlocked.Add(ref current, lengx);

                    if (current == ByteList.Count)
                    {
                        Reset();
                    }

                    return true;
                }
                else
                {
                    return false;
                }
            }
           
            
        }

 

    }
}
