#if UNITY3D
using UnityEngine;
#endif

namespace Framework.Logger
{
    public interface ILogItem
    {
        string Text { get; }
    }
    public class LogItem : ILogItem
    {
        public LogItem(LogType t, string c, string s = "")
        {
            mCondition = c;
            mStackTrace = s;
        }
        protected string mCondition = "";
        protected string mStackTrace = "";
        public virtual string Text
        {
            get
            {
                if (!string.IsNullOrEmpty(mCondition) && !string.IsNullOrEmpty(mStackTrace))
                {
                    return string.Format("{0}\n{1}", mCondition, mStackTrace);
                }
                if (!string.IsNullOrEmpty(mCondition))
                {
                    return mCondition;
                }
                if (!string.IsNullOrEmpty(mStackTrace))
                {
                    return mStackTrace;
                }
                return "<null>";
            }
        }
    }
}
