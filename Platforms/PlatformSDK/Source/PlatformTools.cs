using System;
#if UNITY_WP_8_1 && !UNITY_EDITOR
public class PlatformTools
{
    public static string BS2String_UTF8(byte[] bs)
    {
        return "";
    }
    public static string BS2String_UTF8(byte[] bs, int idx, int cnt)
    {
        return "";
    }
    public static string DownloadWebString(string url)
    {
        string html = "";
        return html;
    }
    public static int CompareStr(string tarTex, string srcTex, bool ignoreCase = false)
    {
        int ret = 0;
        if (ignoreCase)
        {
            tarTex = tarTex.ToLower();
            srcTex = srcTex.ToLower();
        }
        ret = tarTex.CompareTo(srcTex);
        return ret;
    }
    public static int CompareStr(string strA, int indexA, string strB, int indexB, int length)
    {
        int ret = 0;
        ret = String.Compare(strA, indexA, strB, indexB, length);
        return ret;
    }
}
#else
public class PlatformTools
{
    public static string BS2String_UTF8(byte[] bs)
    {
        return System.Text.Encoding.UTF8.GetString(bs, 0, bs.Length);
    }
    public static string BS2String_UTF8(byte[] bs, int idx, int cnt)
    {
        return System.Text.Encoding.UTF8.GetString(bs, idx, cnt);
    }
    public static string DownloadWebString(string url)
    {
        string html = "";
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
        Windows.Web.Http.HttpClient client = new Windows.Web.Http.HttpClient();
        Windows.Foundation.IAsyncOperationWithProgress<string, Windows.Web.Http.HttpProgress> prog =
            client.GetStringAsync(new Uri(url));
        bool completed = false;
        prog.Completed = (Windows.Foundation.IAsyncOperationWithProgress<string, Windows.Web.Http.HttpProgress> asyncInfo, Windows.Foundation.AsyncStatus asyncStatus) =>
        {
            if(asyncStatus == Windows.Foundation.AsyncStatus.Completed)
            {
                html = asyncInfo.GetResults();
                completed = true;
            }
        };
        client.Dispose();
        client = null;
        while (!completed) continue;
#else
        System.Net.WebClient aWebClient = new System.Net.WebClient();
        aWebClient.Encoding = System.Text.Encoding.UTF8;
        try
        {
            html = aWebClient.DownloadString(url);
        }
        catch (System.Net.WebException e)
        {
            html = "";
            UnityEngine.Debug.LogError(e.Message);
        }
        aWebClient.Dispose();
        aWebClient = null;
#endif
        return html;
    }
    public static int CompareStr(string tarTex, string srcTex, bool ignoreCase = false)
    {
        int ret = 0;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
        if (ignoreCase)
        {
            tarTex = tarTex.ToLower();
            srcTex = srcTex.ToLower();
        }
        ret = tarTex.CompareTo(srcTex);
#else
        ret = String.Compare(tarTex, srcTex, ignoreCase);
#endif
        return ret;
    }
    public static int CompareStr(string strA, int indexA, string strB, int indexB, int length)
    {
        int ret = String.Compare(strA, indexA, strB, indexB, length);
        return ret;
    }
}
#endif