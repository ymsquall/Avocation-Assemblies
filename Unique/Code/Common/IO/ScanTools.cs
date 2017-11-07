
/********************************************************************
created:    2015-01-09
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using Unique.Reflection;

namespace Unique
{
    public static class ScanTools
    {
        public static bool ScanAll (string title, string[] paths, Action<string> handler)
        {
            if (paths.IsNullOrEmptyEx() || null == handler)
            {
                return false;
            }

            title = title ?? string.Empty;
            var length = paths.Length;

            var invLength = 1.0f / length;
			var startTime = System.DateTime.Now;
			var lastProgress = 0.0f;
            
            try
            {
                for (int i= 0; i< length; ++i)
                {
                    var path = paths[i];

					// do need change DisplayCancelableProgressBar() rapidly, it will slow down
					// the scanning time greatly.
                    // var info = _GetEditorResourcePath(path);
                    var progress = i * invLength;
					if (progress - lastProgress < 0.05f)
					{
						progress = lastProgress;
					}
					else 
					{
						lastProgress = progress;
					}

					var isCanceled = EditorUtility.DisplayCancelableProgressBar(title, string.Empty, progress);
					if (isCanceled)
					{
						return false;
					}

                    handler(path);

//                    // do not call UnloadUnusedAssets() every time.
//                    if ((i % 50) == 0)
//                    {
//                        // EditorUtility.UnloadUnusedAssets();
//                    }
                }
            }
            finally
            {
                EditorUtility.ClearProgressBar();
            }

			var timeSpan = System.DateTime.Now - startTime;
			Console.WriteLine("[ScanTools.ScanAll()] {0}, costTime={1}s", title, timeSpan.TotalSeconds.ToString("F2"));
            return true;
        }

//        private static string _GetEditorResourcePath (string path)
//        {
//            var head = PathTools.EditorResourceRoot;
//            
//            if (path.StartsWith(head))
//            {
//                return path.Substring(head.Length + 1);
//            }
//            
//            return string.Empty;
//        }
    }
}