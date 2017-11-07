using System;
using UnityEngine;

namespace Framework.Logger
{
    public interface IUnityLogItem : ILogItem
    {
        LogType UnityLogType { get; }
    }
    public class UnityLogItem : LogItem, IUnityLogItem
    {
        public UnityLogItem(LogType t, string c, string s = ""):base(t, c, s)
        {
            mLogType = t;
            var lines = mStackTrace.Split("\n".ToCharArray());
            mStackTrace = "";
            for(int i = 2; i < lines.Length; ++ i)
            {
                mStackTrace += lines[i];
                if (i < lines.Length - 1)
                {
                    mStackTrace += "\n";
                }
            }
        }

        private LogType mLogType = LogType.Log;
        public virtual LogType UnityLogType { get { return mLogType; } }
    }
}
