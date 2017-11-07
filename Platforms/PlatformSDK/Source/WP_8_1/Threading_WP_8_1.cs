//#if UNITY_WP_8_1
//#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
//using Windows.Networking.Sockets;
//#else
////using System.Net.Sockets;
//#endif
//namespace Threading_WP_8_1
//{
//    public delegate void ParameterizedThreadStart(object obj);

//    public class Thread
//    {
//        public Thread(ParameterizedThreadStart start)
//        {
//        }
//        public void Abort() { }
//        public void Start(object parameter) { }
//        public static void Sleep(int millisecondsTimeout) { }
//        public static void Sleep(System.TimeSpan timeout) { }
//    }
//    public class ThreadInterruptedException : System.Exception
//    {
//    }
//}
//#endif