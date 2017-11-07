/*
 * 北风之神SOCKET框架(ZYSocket)
 *  Borey Socket Frame(ZYSocket)
 *  by luyikk@126.com
 *  Updated 2010-12-26 
 */
using System;
using System.Text;
using System.Threading;
using System.Runtime.Serialization.Formatters.Binary;


namespace Networking.ZYSocket
{
    /// <summary>
    /// 数据包格式化类
    /// （老版本了）
    /// </summary>
    public static class Buffers
    {

        static Buffers()
        {
            Encode = Encoding.Unicode;
        }


        public static Encoding Encode
        {
            get;
            set;
        }

        /// <summary>
        /// 将1个2维数据包整合成以个一维数据包
        /// </summary>
        /// <param name="args"></param>
        /// <returns></returns>
        static public Byte[] MergeBytes(params Byte[][] args)
        {
            Int32 length = 0;
            foreach (byte[] tempbyte in args)
            {
                length += tempbyte.Length;  //计算数据包总长度
            }

            Byte[] bytes = new Byte[length]; //建立新的数据包

            Int32 tempLength = 0;

            foreach (byte[] tempByte in args)
            {
                tempByte.CopyTo(bytes, tempLength);
                tempLength += tempByte.Length;  //复制数据包到新数据包
            }

            return bytes;

        }

        /// <summary>
        /// 将一个32位整形转换成一个BYTE[]4字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Int32 data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 将一个64位整型转换成以个BYTE[] 8字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(UInt64 data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 将一个 1位CHAR转换成1位的BYTE
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Char data)
        {
            Byte[] bytes = new Byte[] { (Byte)data };
            return bytes;
        }

        /// <summary>
        /// 将一个BYTE[]数据包添加首位长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(Byte[] data)
        {
            return MergeBytes(
                GetSocketBytes(data.Length),
                data
                );
        }

        /// <summary>
        /// 将一个字符串转换成BYTE[]，BYTE[]的首位是字符串的长度
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(String data)
        {
            Byte[] bytes = Encode.GetBytes(data);

            return MergeBytes(
                GetSocketBytes(bytes.Length),
                bytes
                );
        }

        /// <summary>
        /// 将一个DATATIME转换成为BYTE[]数组
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(DateTime data)
        {
            return GetSocketBytes(data.ToString());
        }


        /// <summary>
        /// 将一个对象转换为二进制数据
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(object obj)
        {
            byte[] data = SerializeObject(obj);

            return MergeBytes(
                GetSocketBytes(data.Length),
                data
                );
        }

        /// <summary>
        /// 将一个32位浮点数转换成一个BYTE[]4字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(float data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 将一个64位浮点数转换成一个BYTE[]8字节
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        static public Byte[] GetSocketBytes(double data)
        {
            return BitConverter.GetBytes(data);
        }

        /// <summary>
        /// 把对象序列化并返回相应的字节
        /// </summary>
        /// <param name="pObj">需要序列化的对象</param>
        /// <returns>byte[]</returns>
        public static byte[] SerializeObject(object pObj)
        {         
            System.IO.MemoryStream _memory = new System.IO.MemoryStream();
            BinaryFormatter formatter = new BinaryFormatter();
           // formatter.TypeFormat=System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString;
            formatter.Serialize(_memory, pObj);
            _memory.Position = 0;
            byte[] read = new byte[_memory.Length];
            _memory.Read(read, 0, read.Length);
            _memory.Close();
            return read;
        }

    }
    
    /// <summary>
    /// 数据包读取类
    /// （此类的功能是讲通讯数据包重新转换成.NET 数据类型）
    /// </summary>
    public class ReadBytes
    {

        private int current;

        private byte[] Data;

        /// <summary>
        /// 数据包长度
        /// </summary>
        public int Length { get; set; }

        /// <summary>
        /// 当前其位置
        /// </summary>
        public int Postion
        {
            get
            {
                return current;
            }

            set
            {               
                Interlocked.Exchange(ref current, value);
            }
        }

        public void Reset()
        {
            current = 0;
        }


        public ReadBytes(Byte[] data)
        {
            Data = data;
            this.Length = Data.Length;
            current = 0;
        }


        #region 整数
        /// <summary>
        /// 读取内存流中的头2位并转换成整型
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadInt16(out short values)
        {

            try
            {
                values = BitConverter.ToInt16(Data, current);
                current = Interlocked.Add(ref current, 2);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头4位并转换成整型
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <returns></returns>
        public bool ReadInt32(out int values)
        {
            try
            {
                values = BitConverter.ToInt32(Data, current);
                current = Interlocked.Add(ref current, 4);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头8位并转换成长整型
        /// </summary>
        /// <param name="ms">内存流</param>
        /// <returns></returns>
        public bool ReadInt64(out long values)
        {
            try
            {
                values = BitConverter.ToInt64(Data, current);
                current = Interlocked.Add(ref current, 8);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }

        /// <summary>
        /// 读取内存流中的首位
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadByte(out byte values)
        {
            try
            {
                values = (byte)Data[current];
                current = Interlocked.Increment(ref current);
                return true;
            }
            catch
            {
                values = 0;
                return false;
            }
        }

        #endregion

        #region 浮点数


        /// <summary>
        /// 读取内存流中的头4位并转换成单精度浮点数
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadFloat(out float values)
        {

            try
            {
                values = BitConverter.ToSingle(Data, current);
                current = Interlocked.Add(ref current, 4);
                return true;
            }
            catch
            {
                values = 0.0f;
                return false;
            }
        }


        /// <summary>
        /// 读取内存流中的头8位并转换成浮点数
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadDouble(out double values)
        {

            try
            {
                values = BitConverter.ToDouble(Data, current);
                current = Interlocked.Add(ref current, 8);
                return true;
            }
            catch
            {
                values = 0.0;
                return false;
            }
        }


        #endregion

        #region 布尔值
        /// <summary>
        /// 读取内存流中的头1位并转换成布尔值
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadBoolean(out bool values)
        {

            try
            {
                values = BitConverter.ToBoolean(Data, current);
                current = Interlocked.Add(ref current, 1);
                return true;
            }
            catch
            {
                values = false;
                return false;
            }
        }

        #endregion

        #region 字符串
        /// <summary>
        /// 读取内存流中一段字符串
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadString(out string values)
        {
            int lengt;
            try
            {
                if (ReadInt32(out lengt))
                {

                    Byte[] buf = new Byte[lengt];

                    Array.Copy(Data, current, buf, 0, buf.Length);

                    values = Encoding.Unicode.GetString(buf, 0, buf.Length);

                    current = Interlocked.Add(ref current, lengt);

                    return true;

                }
                else
                {
                    values = "";
                    return false;
                }
            }
            catch
            {
                values = "";
                return false;
            }

        }
        #endregion

        #region 数据
        /// <summary>
        /// 读取内存流中一段数据
        /// </summary>
        /// <param name="ms"></param>
        /// <returns></returns>
        public bool ReadByteArray(out byte[] values)
        {
            int lengt;
            try
            {
                if (ReadInt32(out lengt))
                {
                    values = new Byte[lengt];
                    Array.Copy(Data, current, values, 0, values.Length);
                    current = Interlocked.Add(ref current, lengt);
                    return true;

                }
                else
                {
                    values = null;
                    return false;
                }
            }
            catch
            {
                values = null;
                return false;
            }

        }
        #endregion

        #region 对象

        /// <summary>
        /// 把字节反序列化成相应的对象
        /// </summary>
        /// <param name="pBytes">字节流</param>
        /// <returns>object</returns>
        private object DeserializeObject(byte[] pBytes)
        {
            object _newOjb = null;
            if (pBytes == null)
                return _newOjb;
            System.IO.MemoryStream _memory = new System.IO.MemoryStream(pBytes);
            _memory.Position = 0;
            BinaryFormatter formatter = new BinaryFormatter();
          //  formatter.TypeFormat = System.Runtime.Serialization.Formatters.FormatterTypeStyle.XsdString;
            _newOjb = formatter.Deserialize(_memory);
            _memory.Close();
            return _newOjb;
        }

        /// <summary>
        /// 读取一个对象
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public bool ReadObject(out object obj)
        {
            byte[] data;
            if (this.ReadByteArray(out data))
            {
                obj = DeserializeObject(data);
                return true;
            }
            else
            {
                obj = null;
                return false;
            }

        }

        #endregion

    }


}
