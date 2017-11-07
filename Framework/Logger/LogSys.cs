using System.Threading;
using System.Collections.Generic;
using System;

#if UNITY3D
    using PObject = UnityEngine.Object;
#else
    using PObject = System.Object;
#endif

namespace Framework.Logger
{
    public static class LogSys
    {
        public struct Log
        {
            public string tag;
            public string msg;

            static Log Now = new Log();
            
            public static Log Make(string t, string m)
            {
                Now.tag = t;
                Now.msg = m;
                return Now;
            }
        }
        public struct LogP
        {
            public string tag;
            public string msg;
            public PObject context;

            static LogP Now = new LogP();

            public static LogP Make(string t, string m, PObject c)
            {
                Now.tag = t;
                Now.msg = m;
                Now.context = c;
                return Now;
            }
        }
        public struct LogExp
        {
            public string tag;
            public string msg;
            public Exception exception;
            public PObject context;

            static LogExp Now = new LogExp();
            public static LogExp Make(string t, string m, Exception e, PObject c)
            {
                Now.tag = t;
                Now.msg = m;
                Now.exception = e;
                Now.context = c;
                return Now;
            }
        }
        public struct LogWLine
        {
            public int lv;
            public string tag;
            public string msg;

            public static LogWLine Now = new LogWLine();
            public static LogWLine Make(int l, string t, string m)
            {
                Now.lv = l;
                Now.tag = t;
                Now.msg = m;
                return Now;
            }
        }
        public struct LogHint
        {
            public object ptr;
            public string msg;

            public static LogHint Now = new LogHint();
            public static LogHint Make(object p, string m)
            {
                Now.ptr = p;
                Now.msg = m;
                return Now;
            }
        }
        public struct LogPopup
        {
            public string title;
            public string msg;

            public static LogPopup Now = new LogPopup();
            public static LogPopup Make(string t, string m)
            {
                Now.title = t;
                Now.msg = m;
                return Now;
            }
        }

        public delegate void LogEventHandler(string tag, string msg);
        public delegate void LogPEventHandler(string tag, string msg, PObject context);
        public delegate void ExpEventHandler(string tag, string msg, Exception exp, PObject context);
        public delegate void WLEventHandler(int lv, string tag, string msg);
        public delegate void HintEventHandler(object ptr, string format);
        public delegate void PopupEventHandler(string title, string format);

        public static event LogPEventHandler DebugImpl;
        public static event LogPEventHandler WarnImpl;
        public static event LogPEventHandler ErrorImpl;
        public static event LogPEventHandler AssertImpl;
        public static event ExpEventHandler ExpImpl;

        public static event LogEventHandler InfoImpl;
        public static event LogEventHandler VerboseImpl;
        public static event LogEventHandler WtfImpl;
        public static event LogEventHandler FatalImpl;
        public static event WLEventHandler WriteLineImpl;
        public static event HintEventHandler ToastShortImpl;
        public static event HintEventHandler ToastLongImpl;
        public static event PopupEventHandler PopupImpl;

        public static Queue<LogP> DebugThreadQueue = new Queue<LogP>();
        public static Queue<LogP> WarnThreadQueue = new Queue<LogP>();
        public static Queue<LogP> ErrorThreadQueue = new Queue<LogP>();
        public static Queue<LogP> AssertThreadQueue = new Queue<LogP>();
        public static Queue<LogExp> ExpThreadQueue = new Queue<LogExp>();

        public static Queue<Log> InfoThreadQueue = new Queue<Log>();
        public static Queue<Log> VerboseThreadQueue = new Queue<Log>();
        public static Queue<Log> WtfThreadQueue = new Queue<Log>();
        public static Queue<Log> FatalThreadQueue = new Queue<Log>();
        public static Queue<LogWLine> WriteLineThreadQueue = new Queue<LogWLine>();
        public static Queue<LogHint> ToastShortThreadQueue = new Queue<LogHint>();
        public static Queue<LogHint> ToastLongThreadQueue = new Queue<LogHint>();
        public static Queue<LogPopup> PopupThreadQueue = new Queue<LogPopup>();

        public static int MainThreadID = Thread.CurrentThread.ManagedThreadId;
        static string Format(string format, params object[] ps)
        {
            string str = "";
            if (ps.Length > 0)
                str = string.Format(format, ps);
            else
                str = format;
            return str;
        }
        public static void Debug(string tag, string format, params object[] ps)
        {
            Debug(tag, format, null, ps);
        }
        public static void Debug(string tag, string format, PObject context, params object[] ps)
        {
            string msg = Format(format, ps);
            if(Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                DebugImpl?.Invoke(tag, msg, context);
            }
            else
            {
                lock(DebugThreadQueue)
                {
                    DebugThreadQueue.Enqueue(LogP.Make(tag, msg, context));
                }
            }
        }
        public static void Warn(string tag, string format, params object[] ps)
        {
            Warn(tag, format, null, ps);
        }
        public static void Warn(string tag, string format, PObject context, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                WarnImpl?.Invoke(tag, msg, context);
            }
            else
            {
                lock (WarnThreadQueue)
                {
                    WarnThreadQueue.Enqueue(LogP.Make(tag, msg, context));
                }
            }
        }
        public static void Error(string tag, string format, params object[] ps)
        {
            Error(tag, format, null, ps);
        }
        public static void Error(string tag, string format, PObject context, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                ErrorImpl?.Invoke(tag, msg, context);
            }
            else
            {
                lock (ErrorThreadQueue)
                {
                    ErrorThreadQueue.Enqueue(LogP.Make(tag, msg, context));
                }
            }
        }
        public static void Assert(string tag, string format, params object[] ps)
        {
            Assert(tag, format, null, ps);
        }
        public static void Assert(string tag, string format, PObject context, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                AssertImpl?.Invoke(tag, msg, context);
            }
            else
            {
                lock (AssertThreadQueue)
                {
                    AssertThreadQueue.Enqueue(LogP.Make(tag, msg, context));
                }
            }
        }
        public static void Exception(string tag, string format, Exception exp, PObject context, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                ExpImpl?.Invoke(tag, msg, exp, context);
            }
            else
            {
                lock (ExpThreadQueue)
                {
                    ExpThreadQueue.Enqueue(LogExp.Make(tag, msg, exp, context));
                }
            }
        }

        public static void Info(string tag, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                InfoImpl?.Invoke(tag, msg);
            }
            else
            {
                lock (InfoThreadQueue)
                {
                    InfoThreadQueue.Enqueue(Log.Make(tag, msg));
                }
            }
        }
        public static void Verbose(string tag, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                VerboseImpl?.Invoke(tag, msg);
            }
            else
            {
                lock (VerboseThreadQueue)
                {
                    VerboseThreadQueue.Enqueue(Log.Make(tag, msg));
                }
            }
        }
        public static void Wtf(string tag, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                WtfImpl?.Invoke(tag, msg);
            }
            else
            {
                lock (WtfThreadQueue)
                {
                    WtfThreadQueue.Enqueue(Log.Make(tag, msg));
                }
            }
        }
        public static void WriteLine(int lv, string tag, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                WriteLineImpl?.Invoke(lv, tag, msg);
            }
            else
            {
                lock (WriteLineThreadQueue)
                {
                    WriteLineThreadQueue.Enqueue(LogWLine.Make(lv, tag, msg));
                }
            }
        }
        public static void Fatal(string tag, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                FatalImpl?.Invoke(tag, msg);
            }
            else
            {
                lock (FatalThreadQueue)
                {
                    FatalThreadQueue.Enqueue(Log.Make(tag, msg));
                }
            }
        }
        public static void ToastShort(object ptr, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                ToastShortImpl?.Invoke(ptr, msg);
            }
            else
            {
                lock (ToastShortThreadQueue)
                {
                    ToastShortThreadQueue.Enqueue(LogHint.Make(ptr, msg));
                }
            }
        }
        public static void ToastLong(object ptr, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                ToastLongImpl?.Invoke(ptr, msg);
            }
            else
            {
                lock (ToastLongThreadQueue)
                {
                    ToastLongThreadQueue.Enqueue(LogHint.Make(ptr, msg));
                }
            }
        }
        public static void Popup(string title, string format, params object[] ps)
        {
            string msg = Format(format, ps);
            if (Thread.CurrentThread.ManagedThreadId == MainThreadID)
            {
                PopupImpl?.Invoke(title, msg);
            }
            else
            {
                lock (PopupThreadQueue)
                {
                    PopupThreadQueue.Enqueue(LogPopup.Make(title, msg));
                }
            }
        }
        
        public static void Update()
        {
            while (DebugThreadQueue.Count > 0)
            {
                lock (DebugThreadQueue)
                {
                    var log = DebugThreadQueue.Dequeue();
                    DebugImpl?.Invoke(log.tag, log.msg, log.context);
                }
                return;
            }

            while (WarnThreadQueue.Count > 0)
            {
                lock (WarnThreadQueue)
                {
                    var log = WarnThreadQueue.Dequeue();
                    WarnImpl?.Invoke(log.tag, log.msg, log.context);
                }
                return;
            }

            while (ErrorThreadQueue.Count > 0)
            {
                lock (ErrorThreadQueue)
                {
                    var log = ErrorThreadQueue.Dequeue();
                    ErrorImpl?.Invoke(log.tag, log.msg, log.context);
                }
                return;
            }

            while (AssertThreadQueue.Count > 0)
            {
                lock (AssertThreadQueue)
                {
                    var log = AssertThreadQueue.Dequeue();
                    AssertImpl?.Invoke(log.tag, log.msg, log.context);
                }
                return;
            }

            while (ExpThreadQueue.Count > 0)
            {
                lock (ExpThreadQueue)
                {
                    var log = ExpThreadQueue.Dequeue();
                    ExpImpl?.Invoke(log.tag, log.msg, log.exception, log.context);
                }
                return;
            }

            while (InfoThreadQueue.Count > 0)
            {
                lock (InfoThreadQueue)
                {
                    var log = InfoThreadQueue.Dequeue();
                    InfoImpl?.Invoke(log.tag, log.msg);
                }
                return;
            }

            while (VerboseThreadQueue.Count > 0)
            {
                lock (VerboseThreadQueue)
                {
                    var log = VerboseThreadQueue.Dequeue();
                    VerboseImpl?.Invoke(log.tag, log.msg);
                }
                return;
            }

            while (WtfThreadQueue.Count > 0)
            {
                lock (WtfThreadQueue)
                {
                    var log = WtfThreadQueue.Dequeue();
                    WtfImpl?.Invoke(log.tag, log.msg);
                }
                return;
            }

            while (FatalThreadQueue.Count > 0)
            {
                lock (FatalThreadQueue)
                {
                    var log = FatalThreadQueue.Dequeue();
                    FatalImpl?.Invoke(log.tag, log.msg);
                }
                return;
            }

            while (WriteLineThreadQueue.Count > 0)
            {
                lock (WriteLineThreadQueue)
                {
                    var log = WriteLineThreadQueue.Dequeue();
                    WriteLineImpl?.Invoke(log.lv, log.tag, log.msg);
                }
                return;
            }

            while (ToastShortThreadQueue.Count > 0)
            {
                lock (DebugThreadQueue)
                {
                    var log = ToastShortThreadQueue.Dequeue();
                    ToastShortImpl?.Invoke(log.ptr, log.msg);
                }
                return;
            }

            while (ToastLongThreadQueue.Count > 0)
            {
                lock (ToastLongThreadQueue)
                {
                    var log = ToastLongThreadQueue.Dequeue();
                    ToastLongImpl?.Invoke(log.ptr, log.msg);
                }
            }
        }
    }
}
