
/********************************************************************
created:    2013-12-16
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using UnityEngine;
using System;
using System.IO;
using System.Collections.Generic;
using System.Globalization;
using Unique.Reflection;

namespace Unique
{
    public static class PathTools
    {
        public static string GetExportPath (string localPath)
        {
            return os.path.join(ExportResourceRoot, localPath);
        }

        public static string GetLocalPath (string exportPath)
        {
            if (null != exportPath)
            {
                var head = EditorResourceRoot;
                
                if (exportPath.StartsWithEx(head, CompareOptions.Ordinal))
                {
					var totalLength = exportPath.Length;
					for (int i = head.Length + 1; i< totalLength; ++i)
					{
						var c = exportPath[i];

						if (c == '/' || c == '\\')
						{
							var startIndex = i + 1;
							if (startIndex < totalLength)
							{
								var localPath = exportPath.Substring(i + 1);
								if (os.isWindows)
								{
									localPath = os.path.normpath(localPath);
								}

								return localPath;
							}
							else
							{
								break;
							}
						}
					}
                }
            }
            
            return exportPath;
        }

		internal static string TranslateToLotPath (string path)
		{
			if (null != path && path.EndsWithEx(Constants.BundleExtension, CompareOptions.Ordinal))
			{
				var dirname  = Path.GetDirectoryName(path);
				var basename = path.Substring(dirname.Length + 1);

				// 3 = ".ab".Length
				basename = basename.Substring(0, basename.Length - 3);
				path = dirname + ".ab/" + basename;
			}

			return path;
		}

		public static string GetExportResourceRoot (TargetPlatform targetPlatform)
		{
			switch (targetPlatform)
			{
			case TargetPlatform.iPhone:
				return EditorResourceRoot + "/ios";
				
			case TargetPlatform.Android:
				return EditorResourceRoot + "/android";
				
			default:
				{
					var message = "Unsupported buildTarget found: " + targetPlatform.ToString()
						+ ", please change the 'Platform' in 'Build Settings'";
					throw new NotImplementedException(message);
				}
			}
		}

		internal static int LastIndexOfExtensionDot (string path)
		{
			if (null == path)
			{
				return -1;
			}
			
			var length = path.Length;
			if (length == 0)
			{
				return -1;
			}
			
			for (int i= length - 1; i>= 0; i--)
			{
				var c = path[i];
				if (c == '.')
				{
					return i;
				}
				else if (c == '/' || c =='\\')
				{
					return -1;
				}
			}

			return -1;
		}

		internal static string GetLocalPathWithDigest (string localPath, string digest)
		{
			var lastDotIndex = LastIndexOfExtensionDot(localPath);
			if (lastDotIndex > 0)
			{
				var localPathWithoutExtension = localPath.Substring(0, lastDotIndex);
				var extension = localPath.Substring(lastDotIndex);
				
				var localPathWithDigest = localPathWithoutExtension + "." + digest + extension;
				return localPathWithDigest;
			}
			else
			{
				var localPathWithDigest = localPath + "." + digest;
				return localPathWithDigest;
			}
		}

		public static string GetRawBundlePath (string increamentPath)
		{
			if (null == increamentPath)
			{
				return string.Empty;
			}

			var targetLength = increamentPath.Length - Constants.LotAsset.IncreamentBundleTail.Length;
			if (targetLength <= 0)
			{
				return string.Empty;
			}

			var rawBundlePath = increamentPath.Substring(0, targetLength) + Constants.BundleExtension;
			return rawBundlePath;
		}

		public static string ExtractLocalPath (string localPathWithDigest)
		{
			var endDotIndex = LastIndexOfExtensionDot(localPathWithDigest);
			if (endDotIndex == -1)
			{
				return localPathWithDigest;
			}
			
			var startDotIndex = localPathWithDigest.LastIndexOf('.', endDotIndex - 1);
			var digestLength = endDotIndex - startDotIndex -1 ;
			
			if (digestLength != Md5sum.AssetDigestLength)
			{
				return localPathWithDigest;
			}
			
			var localPath = localPathWithDigest.Substring(0, startDotIndex) + localPathWithDigest.Substring(endDotIndex);
			return localPath;
		}

		internal static string ExtractAssetDigest (string localPathWithDigest)
		{
			var endDotIndex = LastIndexOfExtensionDot(localPathWithDigest);
			if (endDotIndex == -1)
			{
				return string.Empty;
			}

			var startDigestIndex = localPathWithDigest.LastIndexOf('.', endDotIndex - 1) + 1;
			var digestLength = endDotIndex - startDigestIndex;

			if (digestLength != Md5sum.AssetDigestLength)
			{
				return string.Empty;
			}

			var digest = localPathWithDigest.Substring(startDigestIndex, digestLength);
			return digest;
		}

		internal static bool IsLotBundle (string path)
		{
			if (string.IsNullOrEmpty(path) || path.EndsWithEx(Constants.BundleExtension, CompareOptions.Ordinal))
			{
				return false;
			}
			
			// 6 = "1.ab/2".Length;
			if (path.Length >= 6 && path.LastIndexOf(".ab/", StringComparison.Ordinal) > 0)
			{
				return true;
			}
			
			return false;
		}

		public static bool IsIncrementBundle (string path)
		{
			return null != path && path.EndsWithEx(Constants.LotAsset.IncreamentBundleTail, CompareOptions.Ordinal);
		}

		internal static bool IsDigestEquals (string path1, string path2)
		{
			if (null == path1 || null == path2)
			{
				return false;
			}

			var length1 = path1.Length;
			var length2 = path2.Length;
			var digestLength = Md5sum.AssetDigestLength;
			if (length1 <= digestLength || length2 <= digestLength)
			{
				return false;
			}

			var lastDotIndex = PathTools.LastIndexOfExtensionDot(path1);
			if (lastDotIndex < 0)
			{
				return false;
			}

			var delta = digestLength + length1 - lastDotIndex; 
			var sign = string.CompareOrdinal(path1, length1 - delta, path2, length2 - delta, digestLength);
			return 0 == sign;
		}

		internal static void TestAssetDigest ()
		{
			var localPath = "android/movie.ab";
			var digest = "123";
			
			var localPathWithDigest = PathTools.GetLocalPathWithDigest(localPath, digest);
			var localPathExtracted = PathTools.ExtractLocalPath(localPathWithDigest);
			var digestExtracted = PathTools.ExtractAssetDigest(localPathWithDigest);
			Console.WriteLine("localPathWithDigest={0}, localPathExtracted={1}, digestExtracted={2}"
			                  , localPathWithDigest, localPathExtracted, digestExtracted);

			localPath = "android/mapping";
			localPathWithDigest = PathTools.GetLocalPathWithDigest(localPath, digest);
			localPathExtracted = PathTools.ExtractLocalPath(localPathWithDigest);
			digestExtracted = PathTools.ExtractAssetDigest(localPathWithDigest);
			Console.WriteLine("localPathWithDigest={0}, localPathExtracted={1}, digestExtracted={2}"
			                  , localPathWithDigest, localPathExtracted, digestExtracted);
		}

		/// <summary>
		/// Resource root, this is platform specific, so may be: "resource/android"
		/// Used by System.IO codes, so will not startswith "file:///"
		/// </summary>
		/// <value>The default base path.</value>
        public static string DefaultBasePath
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.OSXEditor:
                        {
                            return ExportResourceRoot;
                        }

					case RuntimePlatform.WindowsPlayer:
					case RuntimePlatform.OSXPlayer:
						{
							return Application.dataPath + "/res";
						}

					// we need to use a subdirectory, because sometimes the operating system
					// may write files in parent directory.
                    case RuntimePlatform.IPhonePlayer:
                        return Application.temporaryCachePath + "/res";

                    case RuntimePlatform.Android:
                        return Application.persistentDataPath + "/res";

                    default:
                        throw new NotImplementedException("Invalid platform type");
                }
            }
        }

        // Used by Editors, it means file in local system, so not StartsWith("file:///")
        private static string _editorResourceRoot;
        public static string EditorResourceRoot
        {
            get
            {
                if (null == _editorResourceRoot)
                {
					//_editorResourceRoot = UniqueManifest._GetEditorResourceRoot();
                }

                return _editorResourceRoot;
            }

			set 
			{
				// editors may need to set the output editor resource root.
				_editorResourceRoot = value;
			}
        }
        
        // Used by Editors, for exporting purpose, platform depended, under EditorResourceRoot, so not StartsWith("file:///")
        public static string ExportResourceRoot
        {
            get
            {
                var activeBuildTarget = EditorUserBuildSettings.activeBuildTarget;
				return GetExportResourceRoot(activeBuildTarget);
            }
        }
        
        private static string _exportPrefabsRoot;
        public static string ExportPrefabsRoot
        {
            get
            {
                if (null == _exportPrefabsRoot)
                {
                    _exportPrefabsRoot = ExportResourceRoot + "/prefabs";
                }
                
                return _exportPrefabsRoot;
            }
        }

        public static string FileProtocolHead
        {
            get
            {
                switch (Application.platform)
                {
                    case RuntimePlatform.WindowsEditor:
                    case RuntimePlatform.WindowsPlayer:
                        return "file:///";

                    default:
                        return "file://";
                }
            }
        }

        public static string DefaultBaseUrl
        {
            get
            {
                return FileProtocolHead + DefaultBasePath;
            }
        }

        private static string _projectPath;
        public static string ProjectPath
        {
            get
            {
                if (null == _projectPath)
                {
                    var dataPath = Application.dataPath;
                    _projectPath = dataPath.Substring(0, dataPath.Length - 7);
                }

                return _projectPath;
            }
        }

		private static string _apkPath;
		public static string ApkPath
		{
			get 
			{
				if (null == _apkPath)
				{
					var streamingPath = Application.streamingAssetsPath;
					// jar:file:///mnt/asec/com.perfectworld.torchlight-2/pkg.apk!/assets
					//            /mnt/asec/com.perfectworld.torchlight-2/pkg.apk
					
					var removeHeadLength = 11;
					var removeTailLength = 8;
					var removeLength = removeHeadLength + removeTailLength;
					var apkPath = streamingPath.Substring(removeHeadLength, streamingPath.Length - removeLength);

					_apkPath = apkPath;
				}

				return _apkPath;
			}
		}
    }
}