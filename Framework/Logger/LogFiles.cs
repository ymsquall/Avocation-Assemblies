using System;
using System.Collections.Generic;
#if UNITY3D
using UnityEngine;
#endif

namespace Framework.Logger
{
    public static class LogFiles
    {
        public delegate void FileHandler(ILogItem[] items, int count);
        public static event FileHandler FileWriter;

        public static int ResizeCount = 32;
        public static float FlushTime = 60f;
        private static float FlushTimer = 0;

        private static List<int> DebugList = new List<int>();
        private static List<int> WarningList = new List<int>();
        private static List<int> ErrorList = new List<int>();
        private static List<int> ExceptionList = new List<int>();
        private static List<int> AssertList = new List<int>();
        private static ILogItem[] LogList = new ILogItem[ResizeCount];
        private static int LogIndex = 0;
#if UNITY3D
        public static void AddUnityLog(LogType t, string condition, string stackTrace)
        {
            if(LogIndex >= LogList.Length)
            {
                Array.Resize(ref LogList, LogList.Length + ResizeCount);
            }
            switch (t)
            {
                case LogType.Log: DebugList.Add(LogIndex); break;
                case LogType.Warning: WarningList.Add(LogIndex); break;
                case LogType.Error: ErrorList.Add(LogIndex); break;
                case LogType.Exception: ExceptionList.Add(LogIndex); break;
                case LogType.Assert: AssertList.Add(LogIndex); break;
            }
            LogList[LogIndex++] = new UnityLogItem(t, condition, stackTrace);
        }
#endif
        public static void ClearAllLog()
        {
            DebugList.Clear();
            WarningList.Clear();
            ErrorList.Clear();
            ExceptionList.Clear();
            AssertList.Clear();
            if(LogList.Length >= ResizeCount * 10)
            {
                LogList = new ILogItem[ResizeCount];
            }
            LogIndex = 0;
        }
        public static void Update(float dt)
        {
            if(null == FileWriter)
            {
                return;
            }
            if (FlushTimer <= 0)
            {
                FileWriter(LogList, LogIndex + 1);
                ClearAllLog();
                FlushTimer = FlushTime;
            }
            else
            {
                FlushTimer -= dt;
            }
        }
    }
}
