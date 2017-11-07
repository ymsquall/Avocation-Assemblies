#if UNITY_WP_8_1

namespace IO_WP_8_1
{
    class File
    {
        System.IO.MemoryStream mStream = null;
        string mFileName = "";
        public File(string fn)
        {
            mFileName = fn;
            mStream = new System.IO.MemoryStream();
        }
        public void Write(byte[] buff)
        {
            if (null != mStream)
                mStream.Write(buff, 0, buff.Length);
        }
        public void Flush()
        {
            byte[] buff = mStream.ToArray();
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
            if (UnityEngine.Windows.File.Exists(mFileName))
                UnityEngine.Windows.File.Delete(mFileName);
            UnityEngine.Windows.File.WriteAllBytes(mFileName, buff);
#else
            if (System.IO.File.Exists(mFileName))
                System.IO.File.Delete(mFileName);
            System.IO.File.WriteAllBytes(mFileName, buff);
            mStream.Close();
#endif
            mStream.Dispose();
            mStream = null;
        }
    }
}
#endif