
/********************************************************************
created:    2012-12-27
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using UnityEngine;
using System.Text;
using System.IO;
using Unique.Reflection;

namespace Unique
{
    public static partial class os
    {
		static os ()
		{
			isEditor = Application.isEditor;

			var platform = Application.platform;
			if (platform == RuntimePlatform.IPhonePlayer)
			{
				isIPhonePlayer = true;
			}
			else if (platform == RuntimePlatform.Android)
			{
				isAndroid = true;
			}

			isBigMemory	= SystemInfo.systemMemorySize > 1024 + 512;
			isWindows   = platform == RuntimePlatform.WindowsEditor
						|| platform == RuntimePlatform.WindowsPlayer;

			frameCount = Time.frameCount;
			time = Time.realtimeSinceStartup;

			// init editor mode properties.
			_InitModeTypes();
		}

        public static void startfile (string filename, string arguments= null, bool shell= false)
        {
            var process = new System.Diagnostics.Process();
			var si = process.StartInfo;
            si.FileName = filename;
			si.Arguments= arguments;
			si.UseShellExecute = shell;
            process.Start ();
        }

        public static void mkdir (string path)
        {
            if (!Directory.Exists(path))
            {
                Directory.CreateDirectory(path);
            }
        }

		public static void makedirs (string name)
		{
			if (!string.IsNullOrEmpty(name) && !Directory.Exists(name))
			{
				var head = Path.GetDirectoryName(name);
				makedirs(head);
				Directory.CreateDirectory(name);
			}
		}

        public static string[] walk (string path, string searchPattern)
        {
            if (Directory.Exists(path))
            {
                var paths = Directory.GetFiles(path, searchPattern, SearchOption.AllDirectories);
                return paths;
            }

            return _emptyPaths;
        }

		public static void dispose<T> (ref T obj) where T : class, System.IDisposable
		{
			if (null != obj)
			{
				obj.Dispose();
				obj = null;
			}
		}

//		public static void recycle (System.IDisposable obj)
//		{
//			DisposableRecycler.Recycle(obj);
//		}

		public static bool isEqual (float a, float b)
		{
			const float eps= 0.000001f;

			var delta = a - b;
			return delta < eps && delta > -eps;
		}

		public static void swap<T> (ref T lhs, ref T rhs)
		{
			var temp = lhs;
			lhs = rhs;
			rhs = temp;
		}

		public static void collectgarbage ()
		{
//			WeakCacheManager.Instance.Clear();
//			PrefabPool.ClearPools();

//			if (!isBigMemoryMode)
//			{
//				var prefabCache = Unique.Web.WebPrefab.GetLruCache();
//				prefabCache.TrimExcess();

//				var webCache = Unique.Web.WebItem.GetLruCache();
//				webCache.TrimExcess();
//			}

//			// called in game client, for flexibility.
////			Resources.UnloadUnusedAssets();
////			GC.Collect();
		}

        private static bool _isTargetPlatformChecked;
        private static TargetPlatform _targetPlatform;

        public static TargetPlatform targetPlatform
        {
            get 
            {
                if (!_isTargetPlatformChecked)
                {
                    _isTargetPlatformChecked = true;

                    if (Application.platform == RuntimePlatform.Android)
                    {
                        _targetPlatform = TargetPlatform.Android;
                    }
                    else if(Application.platform == RuntimePlatform.IPhonePlayer)
                    {
                        _targetPlatform = TargetPlatform.iPhone;
                    }
                    else if (Application.isWebPlayer)
                    {
                        _targetPlatform = TargetPlatform.WebPlayer;
                    }
                    else if (Application.isEditor)
                    {
                        _targetPlatform = EditorUserBuildSettings.activeBuildTarget;
                    }
                }
                
                return _targetPlatform;
            }
        }

		public static bool	isEditor		{ get; private set; }
		public static bool	isIPhonePlayer	{ get; private set; }
		public static bool	isAndroid		{ get; private set; }
		public static bool	isBigMemory		{ get; private set; }
		public static bool  isWindows		{ get; private set; }

		public static int	frameCount		{ get; internal set; }
		public static float	time			{ get; internal set; }

        public const string linesep  = "\n";

        public static readonly StringIntern intern = new StringIntern();
		public static readonly Encoding UTF8 = new UTF8Encoding(false, false);

        private static readonly string[] _emptyPaths = new string[0];
    }
}