using System;
using System.IO;
using Framework.Tools;

namespace Networking.TcpIPNetworking
{
    public interface IMessage
    {
        Int32 MessageID { get; }    // 消息号
        bool IsMatchMsg { get; }
        bool IsCommonMsg { get; }
    }
    public interface IC2S_Msg : IMessage
    {
        Stream Serializer { get; }
        bool NeedLockScreen { get; }
        Int32 UnLockedID { get; }
    }
    public abstract class C2S_Msg_NoLock<T> : IC2S_Msg
        where T : C2S_Msg_NoLock<T>, new()
    {
        public abstract Int32 MessageID { get; }    // 消息号
        public virtual bool IsMatchMsg { get { return false; } }
        public virtual bool IsCommonMsg { get { return false; } }
        public virtual Stream Serializer
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                StreamUtils.Write(stream, (Byte)1);
                StreamUtils.Write(stream, (Int32)0);
                StreamUtils.Write(stream, (Byte)0);
                StreamUtils.Write(stream, MessageID);
                WriteFields(stream);
                stream.Seek(1, SeekOrigin.Begin);
                StreamUtils.Write(stream, (Int32)stream.Length - sizeof(Byte) - sizeof(Int32));
                return stream;
            }
        }
        protected virtual void WriteFields(Stream stream) { }
        protected virtual void Builder(params object[] ps) { }
        public bool NeedLockScreen { get { return false; } }
        public Int32 UnLockedID { get { return -1; } }
        public static T Create(params object[] ps)
        {
            T msg = new T();
            msg.Builder(ps);
            return msg;
        }
    }
    public abstract class C2S_Msg_LockScreen<T> : IC2S_Msg
        where T : C2S_Msg_LockScreen<T>, new()
    {
        public abstract Int32 MessageID { get; }    // 消息号
        public virtual bool IsMatchMsg { get { return false; } }
        public virtual bool IsCommonMsg { get { return false; } }
        public virtual Stream Serializer
        {
            get
            {
                MemoryStream stream = new MemoryStream();
                StreamUtils.Write(stream, (Byte)1);
                StreamUtils.Write(stream, (Int32)0);
                StreamUtils.Write(stream, (Byte)0);
                StreamUtils.Write(stream, MessageID);
                WriteFields(stream);
                stream.Seek(1, SeekOrigin.Begin);
                StreamUtils.Write(stream, (Int32)stream.Length - sizeof(Byte) - sizeof(Int32));
                return stream;
            }
        }
        protected virtual void WriteFields(Stream stream) { }
        protected virtual void Builder(params object[] ps) { }
        public bool NeedLockScreen { get { return true; } }
        public abstract Int32 UnLockedID { get; }
        public static T Create(params object[] ps)
        {
            T msg = new T();
            msg.Builder(ps);
            return msg;
        }
    }
    public abstract class IS2C_Msg : IMessage
    {
        public abstract Int32 MessageID { get; }
        public virtual bool CanDroped { get { return false; } }
        public virtual bool IsMatchMsg { get { return false; } }
        public virtual bool IsCommonMsg { get { return false; } }
        public Byte packed;
    }
    class C2S_HeartTicker : C2S_Msg_NoLock<C2S_HeartTicker>
    {
        public override Int32 MessageID { get { return (Int32)SendMsgType.C2S_Heart; } }
        public override bool IsCommonMsg { get { return true; } }
    }
}
