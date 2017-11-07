using System;
using System.IO;
using System.Collections.Generic;
using System.Data;
using Framework.Tools;
using Framework.Logger;

namespace Library.CsvDataTable
{
    public static class CsvDataTableHelper
    {
        const string Tag = "CsvDataTable";
        //public static void LoadCsvTable(string path)
        //{
        //    if(!Directory.Exists(path))
        //    {
        //        throw new Exception("LoadCsvTable failed, path[" + path + "] has not existed!!!");
        //    }
        //    DirectoryInfo dir = new DirectoryInfo(path);
        //    FileInfo[] files = dir.GetFiles("*.csv", SearchOption.AllDirectories);
        //    foreach(var f in files)
        //    {
        //        DataTable dt = new DataTable(f.ToString());
        //        dt = CsvHelper.csv2dt(f.FullName, 0, dt);
        //        foreach(DataColumn c in dt.Columns)
        //        {
        //            int i = 0;
        //            i++;
        //        }
        //        foreach (DataRow r in dt.Rows)
        //        {
        //            int i = 0;
        //            i++;
        //        }
        //    }
        //}
        public static T ParseDataTable<T>(DataTable dt, int keyIdx)
            where T : class, ICsvDataTableReader, new()
        {
            if (null == dt)
                return null;
            T ret = new T();
            for (int i = 0; i < dt.Rows.Count; ++i)
            {
                DataRow row = dt.Rows[i];
                IConvertible key = null;
                List<IConvertible> vals = new List<IConvertible>();
                for (int k = 0; k < dt.Columns.Count; ++ k)
                {
                    if(k == keyIdx)
                    {
                        key = (IConvertible)row.ItemArray[k];
                        continue;
                    }
                    vals.Add((IConvertible)row.ItemArray[k]);
                }
                if(null == key || vals.Count <= 0)
                {
                    LogSys.Error(Tag, "ParseDataTable<{0}> error, has not a key or values is empty!!!", typeof(T));
                    continue;
                }
                ret.PutValues(key, vals.ToArray());
            }
            return ret;
        }
        public static T ReadTable<T>(string name, Stream s, int ni, int ti, int vi, int ki)
            where T : class, ICsvDataTableReader, new()
        {
            DataTable dt = CsvHelper.csv2dt(name, s, ni, ti, vi);
            return ParseDataTable<T>(dt, ki);
        }
    }
}
