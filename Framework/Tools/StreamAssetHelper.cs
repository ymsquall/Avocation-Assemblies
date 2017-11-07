using UnityEngine;
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
using UnityEngine.Windows;
#else
using System.IO;
#endif

namespace Framework.Tools
{
    public enum StreamAssetRoot
    {
        LUA_ROOT,
        HTML_ROOT,
        MAP2D_ROOT,
        TEXTURE_ROOT,
        FONT_ROOT,
        SHADER_ROOT,
        UIATLASTEX_ROOT,
        UIVIEW_ROOT,
        SOUND_ROOT,
        MUSIC_ROOT,
        EFFECT_ROOT,
        max
    }
    public class StreamAssetHelper
    {
        public static string WWWNewAssetPath = "";
        public static string WWWAssetPath = "";
        public static string NewAssetPath = "";
        public static string AssetPath = "";
        static string NewAssetsExisted(string n, StreamAssetRoot t = StreamAssetRoot.max)
        {
            string path1 = "", path2 = "";
            switch (t)
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                case StreamAssetRoot.LUA_ROOT: path1 = path2 = NewAssetPath + "LuaRoot\\"; break;
                case StreamAssetRoot.HTML_ROOT: path1 = path2 = NewAssetPath + "Html\\"; break;
#else
                case StreamAssetRoot.LUA_ROOT: path1 = path2 = NewAssetPath + "LuaRoot/"; break;
                case StreamAssetRoot.HTML_ROOT: path1 = path2 = NewAssetPath + "Html/"; break;
#endif
                case StreamAssetRoot.MAP2D_ROOT: path1 = NewAssetPath + "Map2D/"; path2 = WWWNewAssetPath + "Map2D/"; break;
                case StreamAssetRoot.TEXTURE_ROOT: path1 = NewAssetPath + "Texture/"; path2 = WWWNewAssetPath + "Texture/"; break;
                case StreamAssetRoot.FONT_ROOT: path1 = NewAssetPath + "Font/"; path2 = WWWNewAssetPath + "Font/"; break;
                case StreamAssetRoot.SHADER_ROOT: path1 = NewAssetPath + "Shader/"; path2 = WWWNewAssetPath + "Shader/"; break;
                case StreamAssetRoot.UIATLASTEX_ROOT: path1 = NewAssetPath + "Texture/Atlas/"; path2 = WWWNewAssetPath + "Texture/Atlas/"; break;
                case StreamAssetRoot.UIVIEW_ROOT: path1 = NewAssetPath + "UIView/"; path2 = WWWNewAssetPath + "UIView/"; break;
                case StreamAssetRoot.SOUND_ROOT: path1 = NewAssetPath + "Sound/"; path2 = WWWNewAssetPath + "Sound/"; break;
                case StreamAssetRoot.MUSIC_ROOT: path1 = NewAssetPath + "Music/"; path2 = WWWNewAssetPath + "Music/"; break;
                case StreamAssetRoot.EFFECT_ROOT: path1 = NewAssetPath + "Effect/"; path2 = WWWNewAssetPath + "Effect/"; break;
                default: path1 = NewAssetPath;  path2 = WWWNewAssetPath; break;
            }
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
            string filePath = path1 + n.Replace("/", "\\");
#else
            string filePath = path1 + n;
#endif
            if (File.Exists(filePath))
            {
                if (StreamAssetRoot.LUA_ROOT == t)
                    return filePath;
                else
                    return path2 + n;
            }
            return "";
        }
        public static string AssetsPath(StreamAssetRoot t, string n)
        {
            string newAsset = NewAssetsExisted(n, t);
            if (!string.IsNullOrEmpty(newAsset))
                return newAsset;
            switch(t)
            {
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
                case StreamAssetRoot.LUA_ROOT: return AssetPath + "LuaRoot\\" + n.Replace("/", "\\");
                case StreamAssetRoot.HTML_ROOT: return AssetPath + "Html\\" + n.Replace("/", "\\");
#else
                case StreamAssetRoot.LUA_ROOT: return AssetPath + "LuaRoot/" + n;
                case StreamAssetRoot.HTML_ROOT: return AssetPath + "Html/" + n;
#endif
                case StreamAssetRoot.MAP2D_ROOT: return WWWAssetPath + "Map2D/" + n;
                case StreamAssetRoot.TEXTURE_ROOT: return WWWAssetPath + "Texture/" + n;
                case StreamAssetRoot.FONT_ROOT: return WWWAssetPath + "Font/" + n;
                case StreamAssetRoot.SHADER_ROOT: return WWWAssetPath + "Shader/" + n;
                case StreamAssetRoot.UIATLASTEX_ROOT: return WWWAssetPath + "Texture/Atlas/" + n;
                case StreamAssetRoot.UIVIEW_ROOT: return WWWAssetPath + "UIView/" + n;
                case StreamAssetRoot.SOUND_ROOT: return WWWAssetPath + "Sound/" + n;
                case StreamAssetRoot.MUSIC_ROOT: return WWWAssetPath + "Music/" + n;
                case StreamAssetRoot.EFFECT_ROOT: return WWWAssetPath + "Effect/" + n;
            }
            return WWWAssetPath + n;
        }
        public static WWW LoadAsset(string f, int v = -1)
        {
            string path = NewAssetsExisted(f);
            if (string.IsNullOrEmpty(path))
                path = WWWAssetPath + f;
            if (v != -1)
                return WWW.LoadFromCacheOrDownload(path, v);
            return new WWW(path);
        }
        public static WWW LoadAsset(StreamAssetRoot t, string f, int v = -1)
        {
            string path = AssetsPath(t, f);
            if (v != -1)
                return WWW.LoadFromCacheOrDownload(path, v);
            return new WWW(path);
        }
#if NETFX_CORE && UNITY_METRO && !UNITY_EDITOR
        public static System.IO.Stream LoadFile(StreamAssetRoot t, string f)
        {
            string path = AssetsPath(t, f);
            Debug.Log(path);
            if (!File.Exists(path))
                return null;
            byte[] bytes = File.ReadAllBytes(path);
            System.IO.MemoryStream stream = new System.IO.MemoryStream(bytes);
            return stream;// WinRTLegacy.UnityFileStreams.OpenFileForReading(new System.Uri(path, System.UriKind.Absolute));
        }
#else
        public static Stream LoadFile(StreamAssetRoot t, string f)
        {
            string path = AssetsPath(t, f);
            return File.Open(path, FileMode.Open, FileAccess.Read, FileShare.ReadWrite);
        }
#endif
    }
}
