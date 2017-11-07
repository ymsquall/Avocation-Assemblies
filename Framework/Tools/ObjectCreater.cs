using System;
using System.IO;
using System.Reflection;

namespace Framework.Tools
{
    interface IObjectCreater
    {
        byte PC { get; }
        void Creater(params object[] ps);
    }
    public abstract class ObjectCreater<T> : IObjectCreater
        where T : ObjectCreater<T>, new() // new不支持非公共的无参构造函数
    {
        public static readonly char[] ArraySplitPrefix = ",".ToCharArray();
        //public const T Default = new T();
        protected ObjectCreater() { }
        public abstract byte PC { get; }
        public static T Create(params object[] ps)
        {
            T ret = new T();
            ret.Creater(ps);
            return ret;
        }
        public abstract void Creater(params object[] ps);
    }

    public interface IReflectionCreater
    {

    }
    [Serializable]
    public abstract class ReflectionCreater<T> : IReflectionCreater, IFieldReaderWriter
        where T : ReflectionCreater<T>, new() // new不支持非公共的无参构造函数
    {
        public static T Default = new T();
        protected string Create(params object[] ps)
        {
            Type thisType = GetType();
            FieldInfo[] thisFields = thisType.GetFields();
            if (ps.Length != thisFields.Length)
            {
                return string.Format("构造消息 {0} 失败，参数数量不同 {1} != {2}, 抛弃此消息！", GetType().ToString(), thisFields.Length, ps.Length);
            }
            for (int i = 0; i < ps.Length; ++i)
            {
                Type t1 = thisFields[i].FieldType;
                Type t2 = ps[i].GetType();
                if (!t1.Equals(t2))
                {
                    try
                    {
                        // 尝试转换类型
                        ps[i] = Convert.ChangeType(ps[i], t1);
                    }
                    catch (System.InvalidCastException e)
                    {
                        return string.Format("构造消息 {0} 失败，参数类型转换失败 InvalidCastException:{1}, 抛弃此消息！", GetType().ToString(), e.Message);
                    }
                    catch (System.FormatException e)
                    {
                        return string.Format("构造消息 {0} 失败，参数类型转换失败 FormatException:{1}, 抛弃此消息！", GetType().ToString(), e.Message);
                    }
                    catch (System.OverflowException e)
                    {
                        return string.Format("构造消息 {0} 失败，参数类型转换失败 OverflowException:{1}, 抛弃此消息！", GetType().ToString(), e.Message);
                    }
                    catch (System.ArgumentException e)
                    {
                        return string.Format("构造消息 {0} 失败，参数类型转换失败 ArgumentException:{1}, 抛弃此消息！", GetType().ToString(), e.Message);
                    }
                    catch (Exception e)
                    {
                        return string.Format("构造消息 {0} 失败，参数类型转换失败 Exception:{1}, 抛弃此消息！", GetType().ToString(), e.Message);
                    }
                }
                thisFields[i].SetValue(this, ps[i]);
            }
            return null;
        }
        public void Write(Stream s)
        {
            Type thisType = GetType();
            FieldInfo[] thisFields = thisType.GetFields();
            for (int i = 0; i < thisFields.Length; ++i)
            {
                Type t1 = thisFields[i].FieldType;
                object field = thisFields[i].GetValue(this);
                StreamUtils.Write(s, t1, field);
            }
        }
        public void Read(Stream s)
        {
            Type thisType = GetType();
            FieldInfo[] thisFields = thisType.GetFields();
            for (int i = 0; i < thisFields.Length; ++i)
            {
                Type t1 = thisFields[i].FieldType;
                object field = null;
                StreamUtils.Read(s, t1, out field);
                thisFields[i].SetValue(this, field);
            }
        }
        public static T New(ref string errMsg, params object[] ps)
        {
            T ret = new T();
            errMsg = ret.Create(ps);
            if (!string.IsNullOrEmpty(errMsg))
                return null;
            return ret;
        }

    }
}