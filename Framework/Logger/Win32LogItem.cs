using System;

namespace Framework.Logger
{
    public interface IWin32LogItem : ILogItem
    {
        LogType LogType { get; }
    }
    public class Win32LogItem : LogItem, IWin32LogItem
    {
        public Win32LogItem(LogType t, string c, string s = ""):base(t, c, s)
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
        public virtual LogType LogType { get { return mLogType; } }
    }
}
