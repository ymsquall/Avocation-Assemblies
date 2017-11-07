using Framework.Logger;
using System;
using System.IO;
using System.Net;
using System.Threading;

namespace Framework.Tools
{
    public class HttpData : IQueueData
    {
        public string FN { set; get; }
        public Stream FS { set; get; }
    }
    public class HttpDownload : DataQueue<HttpDownload, HttpData>
    {
        public delegate void ProgHandler(HttpData data, float prog);
        public delegate void FileOverHandler(HttpData data);
        public delegate void AllOverHandler();
        public event ProgHandler OnProgress;
        public event FileOverHandler OnFileDownloadOvered;
        public event AllOverHandler OnDownloadOvered;

        const string Tag = "HttpDownload";
        string mRootWebURL = "";
        Thread mDownloadThread = null;
        //object mFileOverLocker = new object();
        //public object FileOverLocker { get { return mFileOverLocker; } }

        public HttpDownload(string url)
        {
            mRootWebURL = url;
        }
        public void StartDownload(string segment)
        {
            if (null != mDownloadThread && mDownloadThread.ThreadState == ThreadState.Running)
                return;
            mDownloadThread = new Thread(new ParameterizedThreadStart(Download));
            mDownloadThread.Start(segment);
        }
        void Download(object obj)
        {
            Stream myStream = null;
            HttpData data = null;
            try
            {
                string segment = obj as string;
                data = PopSend();
                while (null != data)
                {
                    //获取已经下载的长度
                    long pos = data.FS.Length;
                    data.FS.Seek(pos, SeekOrigin.Current);
                    //打开网络连接
                    HttpWebRequest myRequest = (HttpWebRequest)HttpWebRequest.Create(mRootWebURL + segment + data.FN);
                    if (pos > 0)
                        myRequest.AddRange((int)pos);             //设置Range值
                    //向服务器请求,获得服务器的回应数据流
                    myStream = myRequest.GetResponse().GetResponseStream();
                    long totalLen = myStream.Length;
                    //定义一个字节数据
                    byte[] btContent = new byte[512];
                    int intSize = 0;
                    intSize = myStream.Read(btContent, 0, 512);
                    while (intSize > 0)
                    {
                        data.FS.Write(btContent, 0, intSize);
                        long nowLen = data.FS.Length;
                        intSize = myStream.Read(btContent, 0, 512);
                        if(null != OnProgress)
                            OnProgress(data, (float)((double)nowLen / (double)totalLen) * 100f);
                        //Thread.Sleep(0);
                    }
                    //关闭流
                    myStream.Close();
                    // 下一个
                    data.FS.Seek(0, SeekOrigin.Begin);
                    //lock(FileOverLocker)
                    //{
                        if (null != OnFileDownloadOvered)
                            OnFileDownloadOvered(data);
                    //}
                    data = PopSend();
                }
                if (null != OnDownloadOvered)
                    OnDownloadOvered();
            }
            catch (Exception e)
            {
                LogSys.Error(Tag, "SimpleDownload Exception:{0}", e);
                if (null != myStream)
                    myStream.Close();
                if (null != data && null != data.FS)
                    data.FS.Close();
            }
        }
    }
}
