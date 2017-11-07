using System;
using System.Collections.Generic;

namespace Framework.Tools
{
    public interface INotify
    {
        void Notify(int n, params object[] ps);
        void Notify(Enum n, params object[] ps);
        void Notify(string n, params object[] ps);
    }
    public interface INotifications
    {
        void AddNotify(INotify n);
        void RemoveNotify(INotify n);
        void Clear();
        //void Notify(string e, params object[] ps);
        //void Notify(int e, params object[] ps);
        //void Notify(Enum e, params object[] ps);
    }
    public class NotificationsSingleT<T> : AutoSingleT<T>, INotifications
        where T : NotificationsSingleT<T>, new()
    {
        List<INotify> mList = new List<INotify>();

        public void AddNotify(INotify n)
        {
            if (!mList.Contains(n))
                mList.Add(n);
        }
        public void RemoveNotify(INotify n)
        {
            if (mList.Contains(n))
                mList.Remove(n);
        }
        public void Clear()
        {
            mList.Clear();
        }
        protected void Notify(string e, params object[] ps)
        {
            foreach(var n in mList)
            {
                n.Notify(e, ps);
            }
        }
        protected void Notify(int e, params object[] ps)
        {
            foreach (var n in mList)
            {
                n.Notify(e, ps);
            }
        }
        protected void Notify(Enum e, params object[] ps)
        {
            foreach (var n in mList)
            {
                n.Notify(e, ps);
            }
        }
    }
}
