using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using System.Text.RegularExpressions;

namespace Framework.Tools
{
    public class FileUtils
    {
        static bool EnumFilesByPath(string path, bool subdir, ref List<string> files, string[] exclude, ExcuteActHandler excute, params string[] extNames)
        {
            path.Replace("\\", "/");
            if (path[path.Length - 1] != '/')
            {
                path = string.Concat(path, '/');
            }
            string[] fns = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < fns.Length; ++i)
            {
                var f = fns[i];
                if (null != exclude && exclude.Length > 0)
                {
                    bool excluded = false;
                    for (int e = 0; e < exclude.Length; ++ e)
                    {
                        if(f.Contains(exclude[e]))
                        {
                            excluded = true;
                            break;
                        }
                    }
                    if (excluded)
                        continue;
                }
                if (null != extNames && extNames.Length > 0)
                {
                    for (int j = 0; j < extNames.Length; ++j)
                    {
                        var ext = extNames[j];
                        if (f.Substring(f.Length - ext.Length) == ext)
                        {
                            if (null != excute)
                            {
                                if (excute(path, f.Replace(path, "")))
                                    files.Add(f);
                            }
                            else
                                files.Add(f);
                            break;
                        }
                    }
                }
                else
                {
                    if (f.Substring(f.Length - 5) == ".meta")
						continue;
					if (f.Substring(f.Length - 9) == ".DS_Store")
						continue;
					files.Add(f);
                }
            }
            if (subdir)
            {
                string[] paths = System.IO.Directory.GetDirectories(path);
                for (int i = 0; i < paths.Length; ++i)
                {
                    var p = paths[i];
                    if (null != excute)
                    {
                        if (excute(path, p.Replace(path, ""), true))
                            files.Add(p);
                    }
                    else
                        files.Add(p);
                    if (!EnumFilesByPath(p, subdir, ref files, exclude, excute, extNames))
                        return false;
                }
            }
            return true;
        }
        public static string[] EnumAllFilesByPath(string path, bool subdir, params string[] extNames)
        {
            return EnumAllFilesByPath(path, subdir, new string[] { }, extNames);
        }
        public static string[] EnumAllFilesByPath(string path, bool subdir, string[] exclude, params string[] extNames)
        {
            List<string> files = new List<string>(0);
            path.Replace("\\", "/");
            if (path[path.Length - 1] != '/')
            {
                path = string.Concat(path, '/');
            }
            string[] fns = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < fns.Length; ++i)
            {
                var f = fns[i];
                if (null != exclude && exclude.Length > 0)
                {
                    bool excluded = false;
                    for (int e = 0; e < exclude.Length; ++e)
                    {
                        if (f.Contains(exclude[e]))
                        {
                            excluded = true;
                            break;
                        }
                    }
                    if (excluded)
                        continue;
                }
                if (null != extNames && extNames.Length > 0)
                {
                    for (int j = 0; j < extNames.Length; ++j)
                    {
                        var ext = extNames[j];
                        if (f.Substring(f.Length - ext.Length) == ext)
                        {
                            files.Add(f);
                            break;
                        }
                    }
                }
                else
                {
                    if (f.Substring(f.Length - 5) == ".meta")
						continue;
					if (f.Substring(f.Length - 9) == ".DS_Store")
						continue;
                    files.Add(f);
                }
            }
            if(subdir)
            {
                string[] paths = System.IO.Directory.GetDirectories(path);
                for (int i = 0; i < paths.Length; ++i)
                {
                    var p = paths[i];
                    if (!EnumFilesByPath(p, subdir, ref files, exclude, null, extNames))
                        return null;
                }
            }
            return files.ToArray();
        }

        public delegate bool ExcuteActHandler(string path, string f, bool isPath = false);
        public static string[] ExcuteAllFilesByPath(string path, bool subdir, string[] exclude, ExcuteActHandler excute, params string[] extNames)
        {
            List<string> files = new List<string>(0);
            path.Replace("\\", "/");
            if(path[path.Length - 1] != '/')
            {
                path = string.Concat(path, '/');
            }
            string[] fns = System.IO.Directory.GetFiles(path);
            for (int i = 0; i < fns.Length; ++i)
            {
                var f = fns[i];
                if (null != exclude && exclude.Length > 0)
                {
                    bool excluded = false;
                    for (int e = 0; e < exclude.Length; ++e)
                    {
                        if (f.Contains(exclude[e]))
                        {
                            excluded = true;
                            break;
                        }
                    }
                    if (excluded)
                        continue;
                }
                if (null != extNames && extNames.Length > 0)
                {
                    for (int j = 0; j < extNames.Length; ++j)
                    {
                        var ext = extNames[j];
                        if (f.Substring(f.Length - ext.Length) == ext)
                        {
                            if (null != excute)
                            {
                                if(excute(path, f))
                                    files.Add(f);
                            }
                            else
                                files.Add(f);
                            break;
                        }
                    }
                }
                else
                {
                    if (f.Substring(f.Length - 5) == ".meta")
                        continue;
                    if (f.Substring(f.Length - 9) == ".DS_Store")
                        continue;
                    if(null != excute)
                    {
                        if (excute(path, f.Replace(path, "")))
                            files.Add(f);
                    }
                    else
                        files.Add(f);
                }
            }
            if (subdir)
            {
                string[] paths = System.IO.Directory.GetDirectories(path);
                for (int i = 0; i < paths.Length; ++i)
                {
                    var p = paths[i];
                    if (null != excute)
                    {
                        if (excute(path, p.Replace(path, ""), true))
                            files.Add(p);
                    }
                    else
                        files.Add(p);
                    if (!EnumFilesByPath(p, subdir, ref files, exclude, excute, extNames))
                        return null;
                }
            }
            return files.ToArray();
        }
        public static string GetFileMD5HashCode(string fn)
        {
            try
            {
                FileStream file = new FileStream(fn, FileMode.Open);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(file);
                file.Close();
 
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromFile() fail,error:" + ex.Message);
            }
        }
        public static string GetStringMD5HashCode(string str)
        {
            try
            {
                byte[] buf = Encoding.UTF8.GetBytes(str);
                System.Security.Cryptography.MD5 md5 = new System.Security.Cryptography.MD5CryptoServiceProvider();
                byte[] retVal = md5.ComputeHash(buf);
                StringBuilder sb = new StringBuilder();
                for (int i = 0; i < retVal.Length; i++)
                {
                    sb.Append(retVal[i].ToString("x2"));
                }
                return sb.ToString();
            }
            catch (Exception ex)
            {
                throw new Exception("GetMD5HashFromString() fail,error:" + ex.Message);
            }
        }
        public static string GetShortName(string fullName)
        {
            fullName = fullName.Replace("\\", "/");
            fullName = fullName.Substring(fullName.LastIndexOf('/') + 1);
            if (fullName.Contains("."))
                fullName = fullName.Substring(0, fullName.LastIndexOf('.'));
            return fullName;
        }
        public static string GetLastPathName(string fullName)
        {
            fullName = fullName.Replace("\\", "/");
            fullName = fullName.Substring(0, fullName.LastIndexOf('/'));
            fullName = fullName.Substring(fullName.LastIndexOf('/') + 1);
            return fullName;
        }
        public static string AbsPath2RelPath(string absolutePath, string relativePath)
        {
            //预计拼接结果
            string splicingResult = string.Empty;
            if (!Path.IsPathRooted(relativePath))
            {
                //匹配相对路径，匹配需要向上推的目录层数
                Regex regex = new Regex(@"^\\/|([..]+)");
                int backUp = regex.Matches(relativePath).Count;
                string[] ps = absolutePath.Split("/".ToCharArray());
                List<string> pathes = new List<string>();
                pathes.AddRange(ps);
                pathes.RemoveRange(pathes.Count - backUp, backUp);
                //匹配文件名，匹配需要附加的目录层数
                regex = new Regex(@"^\\/|([a-zA-Z0-9]+)");
                MatchCollection matches = regex.Matches(relativePath);
                foreach (Match match in matches)
                {
                    pathes.Add(match.Value);
                }
                //驱动器地址取绝对路径中的驱动器地址
                pathes[0] = Path.GetPathRoot(absolutePath);
                foreach (string p in pathes)
                {
                    splicingResult = Path.Combine(splicingResult, p);
                }
            }
            return splicingResult;
        }
        public static string SplitFullPath(ref string path)
        {
            int fnPos = path.LastIndexOf(@"\");
            if (fnPos < 0 || fnPos >= path.Length)
                return path;
            string ret = path.Substring(fnPos + 1);
            path = path.Substring(0, fnPos + 1);
            return ret;
        }
        public static bool SaveFile(string fullPath, byte[] buff)
        {
            string path = fullPath;
            string fn = SplitFullPath(ref path);
            return SaveFile(path, fn, buff);
        }
        public static bool SaveFile(string path, string fn, byte[] buff)
        {
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
            string fileName = path + fn;
            File.WriteAllBytes(fileName, buff);
            //FileStream fs = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.Write);
            //fs.Write(buff, 0, buff.Length);
            //fs.Close();
            return true;
        }
    }
}