
/********************************************************************
created:    2014-12-20
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
//#define USE_UNLOAD_UNUSED_RESOURCES_ROUTINE

using UnityEngine;
using System;
using System.IO;
using System.Collections;
using System.Globalization;

namespace Unique
{
    public static class BundleTools
    {
		internal static void UnloadAsset<T> (ref T asset) where T: UnityEngine.Object
		{
			if (null != asset)
			{
				// Resources.UnloadAsset() will not really unload the asset from memory,
				// a Resources.UnloadUnusedAssets() is required, but the second method is
				// very time consuming, it can cost 100ms on my macos.
				Resources.UnloadAsset(asset);
				asset = null;

				#if USE_UNLOAD_UNUSED_RESOURCES_ROUTINE
				if (null == _unloadAssetRoutine)
				{
					CoroutineManager.StartCoroutine(_CoUnloadAsset(), out _unloadAssetRoutine);
				}
				#endif
			}
		}

		#if USE_UNLOAD_UNUSED_RESOURCES_ROUTINE
		private static IEnumerator _CoUnloadAsset ()
		{
			var endTime = Time.time + 10.0f;
			while (Time.time < endTime)
			{
				yield return null;
			}

			Resources.UnloadUnusedAssets();
			_unloadAssetRoutine = null;
		}
		#endif

		public static BundleFormat GetBundleFormat (string fullpath)
        {
            if (!string.IsNullOrEmpty(fullpath) && File.Exists(fullpath))
            {
                using (var stream = new FileStream(fullpath, FileMode.Open, FileAccess.Read))
                {
                    stream.Read(_compressedFlags, 0, _flagsCount);
                    return GetBundleFormat(_compressedFlags);
                }
            }

            return BundleFormat.None;
        }

		public static BundleFormat GetBundleFormat (byte[] bytes)
        {
            if (null != bytes && bytes.Length >= _flagsCount)
            {
                if (   bytes[0] == 'U'
                    && bytes[1] == 'n'
                    && bytes[2] == 'i'
                    && bytes[3] == 't'
                    && bytes[4] == 'y'
                    )
                {
                    if (bytes[5] == 'R' && bytes[6] == 'a' && bytes[7] == 'w')
                    {
                        return BundleFormat.UnityRaw;
                    }
                    else if (bytes[5] == 'W' && bytes[6] == 'e' && bytes[7] == 'b')
                    {
                        return BundleFormat.UnityWeb;
                    }
                }
            }

            return BundleFormat.None;
        }

		internal static long GetTotalSize (string path)
		{
			if (string.IsNullOrEmpty(path))
			{
				return 0;
			}
			
			var bundleSize  = os.path.getsize(path);
			if (bundleSize == 0)
			{
				return 0;
			}
			
			var manifestPath= Path.ChangeExtension(path, ".didi");
			var abExt       = Constants.BundleExtension;
			var abExtLength = abExt.Length;
			
			if (File.Exists(manifestPath))
			{
				var lines = File.ReadAllLines(manifestPath);
				
				foreach (var line in lines)
				{
					var c = line[0];
					// the first is flags, so it is a digit.
					if (!char.IsDigit(c))
					{
						continue;
					}
					
					var splits = line.Split();
					if (splits.Length != 2)
					{
						Console.Error.WriteLine("splits.Length={0}, line={1}", splits.Length, line);
						continue;
					}

					var partSharedPath = splits[1];
					if (!partSharedPath.EndsWithEx(abExt, CompareOptions.Ordinal))
					{
						var abIndex = partSharedPath.IndexOf(abExt, StringComparison.Ordinal);
						AssertTools.Greater(abIndex, 0);
						partSharedPath = partSharedPath.Substring(0, abIndex + abExtLength);
						AssertTools.IsTrue(partSharedPath.EndsWithEx(abExt, CompareOptions.Ordinal));
					}
					
					var partFullPath = PathTools.DefaultBasePath + "/" + Constants.LocalSharedDirectory + "/" + partSharedPath;
					bundleSize += os.path.getsize(partFullPath);
				}
			}
			
			return bundleSize;
		}

        private const int _flagsCount = 8;
        private static readonly byte[] _compressedFlags = new byte[_flagsCount];

		#if USE_UNLOAD_UNUSED_RESOURCES_ROUTINE
		private static CoroutineItem _unloadAssetRoutine;
		#endif
    }
}