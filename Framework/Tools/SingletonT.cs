using Framework.Logger;
using System;
#if UNITY3D
using UnityEngine;
#endif

namespace Framework.Tools
{
    public interface ISingletonT
    {
        void DestoryT();
    }

#region 自动单件实例
    public class AutoSingleT<T> : ISingletonT where T : new()//，new不支持非公共的无参构造函数 
    {
        /*
         * 单线程测试通过！
         * 多线程测试通过！
         * 根据需要在调用的时候才实例化单例类！
        */
        private static T mInstance;
        public static readonly object SyncObject = new object();
        public static T Inst
        {
            set
            {
                mInstance = value;
            }
            get
            {
                //没有第一重 singleton == null 的话，每一次有线程进入 GetInstance()时，均会执行锁定操作来实现线程同步，
                //非常耗费性能 增加第一重singleton ==null 成立时的情况下执行一次锁定以实现线程同步
                if (mInstance == null)
                {
                    lock (SyncObject)
                    {
                        if (mInstance == null)//Double-Check Locking 双重检查锁定
                        {
                            //_instance = new T();
                            //需要非公共的无参构造函数，不能使用new T() ,new不支持非公共的无参构造函数 
                            try
                            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                                mInstance = Activator.CreateInstance<T>();
#else
                                mInstance = (T)Activator.CreateInstance(typeof(T), true); //第二个参数防止异常：“没有为该对象定义无参数的构造函数。”
#endif
                            }
                            catch (System.MissingMemberException e)
                            {
                                LogSys.Exception("SingleonT", e.Message, e, e);
                            }
                            finally
                            {
                                mInstance = new T();
                            }
                        }
                    }
                }
                return mInstance;
            }
        }
        public void DestoryT()
        {
            Inst = default(T);
        }
    }
#endregion

//#region 预创建单件1
//    public class SingletonT<T> : ISingletonT where T : class//new()，new不支持非公共的无参构造函数 
//    {
//        /*
//         * 单线程测试通过！
//         * 多线程测试通过！
//         * 主动实例化单例类！
//         * 注：使用静态初始化的话，无需显示地编写线程安全代码，C# 与 CLR 会自动解决多线程同步问题
//        */ 
//        private static T mInstance = null;
//        public static T Inst{ get{ return mInstance; } }
//        public static T Create(params object[] ps)
//        {
//            if (null == ps) ps = new object[0];
//            if (null == mInstance)
//                mInstance = (T)Activator.CreateInstance(typeof(T), true);
//            SingletonT<T> inst = mInstance as SingletonT<T>;
//            if (null != inst && inst.InitWithParams(ps))
//            {
//                return inst as T;
//            }
//            UnityEngine.Debug.Log(string.Format("创建单件对象[{0}]失败，可能是参数列表与预期不匹配,参数数量[{1}] : ", typeof(T).ToString(), ps.Length));
//            foreach(object p in ps)
//                UnityEngine.Debug.Log(string.Format("  --type[{0}] : value[{1}]", p.GetType().ToString(), p.ToString()));
//            return null;
//        }
//        protected virtual bool InitWithParams(params object[] ps)
//        {
//            return true;
//        }
//        public void DestoryT()
//        {
//            mInstance = null;
//        }
//    }
//#endregion

#region 预创建单件2
    public class SingletonT<T> : ISingletonT where T : class//new()，new不支持非公共的无参构造函数 
    {
        /*         
         * 单线程测试通过！
         * 多线程测试通过！
         *主动实例化单例类！
         * 注：使用静态初始化的话，无需显示地编写线程安全代码，C# 与 CLR 会自动解决多线程同步问题
        */
        protected SingletonT() { }
        /*内部类
         * 创建内部类的一个目的是为了抽象外部类的某一状态下的行为，
         * 或者C#内部类仅在外部类的某一特定上下文存在。或是隐藏实现，
         * 通过将内部类设为private，可以设置仅有外部类可以访问该类。
         * 内部类的另外一个重要的用途是当外部类需要作为某个特定的类工作，
         * 而外部类已经继承与另外一个类的时候，因为C#不支持多继承，
         * 所以创建一个对应的内部类作为外部类的一个facade来使用。 
        */
        protected class SingletonCreator
        {
            internal static readonly T Instance = (T)Activator.CreateInstance(typeof(T), true);// new T();
            internal static bool Created = false;
        }
        public static T Inst { get { if (!SingletonCreator.Created) return null; return SingletonCreator.Instance; } }
        public static T Create(params object[] ps)
        {
            if (null == ps) ps = new object[0];
            SingletonT<T> inst = SingletonCreator.Instance as SingletonT<T>;
            if (null != inst && inst.InitWithParams(ps))
            {
                SingletonCreator.Created = true;
                return inst as T;
            }
            Console.WriteLine(string.Format("创建单件对象[{0}]失败，可能是参数列表与预期不匹配,参数数量[{1}] : ", typeof(T).ToString(), ps.Length));
            for (int i = 0; i < ps.Length; ++i)
            {
                var p = ps[i];
                Console.WriteLine(string.Format("  --type[{0}] : value[{1}]", p.GetType().ToString(), p.ToString()));
            }
            SingletonCreator.Created = false;
            return null;
        }
        protected virtual bool InitWithParams(params object[] ps)
        {
            return true;
        }
        public void DestoryT()
        {
            Console.WriteLine("不能销毁SingleCreatorT的实例");
            SingletonCreator.Created = false;
        }
    }
#endregion

#if UNITY3D
    public class SingletonMBT<T> : MonoBehaviour, ISingletonT
        where T : SingletonMBT<T>
    {
        static T mInstance = null;
        public static T Inst { get { return mInstance; } }
        public void DestoryT()
        {
            MonoBehaviour _this = this as MonoBehaviour;
            GameObject.Destroy(_this.gameObject);
        }
        protected virtual void Awake()
        {
            mInstance = this as T;
        }
        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }
    public class AutoSingleMBT<T> : MonoBehaviour, ISingletonT
        where T : AutoSingleMBT<T>
    {
        static T mInstance = null;
        public static T Inst
        {
            get
            {
                if(null == mInstance)
                {
                    string goName = typeof(T).Name;
                    GameObject go = GameObject.Find(goName);
                    if(null == go)
                    {
                        go = new GameObject(goName);
                        mInstance = go.AddComponent<T>();
                    }
                    else
                    {
                        mInstance = go.GetComponent<T>();
                        if(null == mInstance)
                        {
                            mInstance = go.AddComponent<T>();
                        }
                    }
                }
                return mInstance;
            }
        }
        protected virtual void Awake()
        {
            mInstance = this as T;
        }
        public void DestoryT()
        {
            MonoBehaviour _this = this as MonoBehaviour;
            GameObject.Destroy(_this.gameObject);
        }
        protected virtual void OnDestroy()
        {
            mInstance = null;
        }
    }
#endif
}
