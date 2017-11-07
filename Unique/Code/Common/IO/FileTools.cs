
/********************************************************************
created:    2015-03-04
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using System.IO;
//using System.Threading;

namespace Unique
{
    public static class FileTools
    {
		public static void WriteAllTextSafely (string path, string contents)
		{
			_WriteAllSafely(path, contents, File.WriteAllText);
		}

		public static void WriteAllLinesSafely (string path, string[] contents)
		{
			_WriteAllSafely(path, contents, File.WriteAllLines);
		}

		public static void WriteAllBytesSafely (string path, byte[] bytes)
		{
			_WriteAllSafely(path, bytes, File.WriteAllBytes);
		}

		private static void _WriteAllSafely<T> (string path, T contents, Action<string, T> writeFunction)
		{
			if (string.IsNullOrEmpty(path))
			{
				return;
			}
			
			var existence = File.Exists(path);
			if (!existence)
			{
				var dirpath = Path.GetDirectoryName(path);
				os.makedirs(dirpath);
			}
			
			var tempFileName = path + ".tmp.0506";
			writeFunction(tempFileName, contents);

			if (existence)
			{
				File.Delete(path);
			}
			
			File.Move(tempFileName, path);
		}

		public static void Overwrite (string sourceFileName, string destFileName)
		{
			if (string.IsNullOrEmpty(sourceFileName) || string.IsNullOrEmpty(destFileName))
			{
				return;
			}

			FileTools.DeleteSafely(destFileName);
			File.Move(sourceFileName, destFileName);
		}

		public static string ShowTempFile (string fname, string contents)
		{
			fname = fname ?? "tempfilename";
			contents = contents ?? string.Empty;

			var directory = os.path.join(UnityEngine.Application.persistentDataPath, "temp-files");
			os.makedirs(directory);
			
			var filepath = os.path.join(directory, fname);

			File.WriteAllText(filepath, contents);
			os.startfile(filepath, null, true);

			return filepath;
		}

		public static string GetHexDigest16 (string path, bool checkFileExistence= true)
		{
			if (!string.IsNullOrEmpty(path))
			{
				try 
				{
					if (!checkFileExistence || File.Exists(path))
					{
						var md5 = Md5sum.Instance.GetHexDigest16(File.ReadAllBytes(path));
						return md5;
					}
				}
				catch (Exception)
				{

				}
			}

			return string.Empty;
		}

		public static string[] GetHexDigest16 (string[] paths, bool checkFileExistence= true)
		{
			if (null == paths)
			{
				return EmptyArray<string>.Instance;
			}

			var count = paths.Length;
			if (count == 0)
			{
				return EmptyArray<string>.Instance;
			}

			var digests = new string[count];
			for (int i= 0; i< count; ++i)
			{
				digests[i] = GetHexDigest16(paths[i], checkFileExistence);
			}

			return digests;
		}

		public static bool DeleteSafely (string path)
		{
			if (!string.IsNullOrEmpty(path) && File.Exists(path))
			{
				try
				{
					File.Delete(path);
					return true;
				}
				catch
				{

				}
			}
			
			return false;
		}
    }
}