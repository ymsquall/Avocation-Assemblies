
/********************************************************************
created:    2015-04-16
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using System.Collections;
using System.IO;

namespace Unique.IO
{
	public class CsvWriter: IDisposable
	{
		public CsvWriter (string path)
		{
			// This override using UTF8NoBOM, do not use StreamWriter(path, false, Encoding.UTF8),
			// which is platform dependent.
			_writer = new StreamWriter(path);
			_writer.NewLine = os.linesep;
		}
		
		public void Dispose ()
		{
			_writer.Close();
		}

		public void WriteRow (ArrayList items, bool autoClearItems)
		{
			if (null != items && items.Count > 0)
			{
				_WriteItem(items[0] as string);
				
				var itemCount = items.Count;
				for (int i= 1; i< itemCount; ++i)
				{
					_writer.Write(',');
					
					var item = items[i] as string;
					_WriteItem(item);
				}
				
				_writer.WriteLine();

				if (autoClearItems)
				{
					items.Clear();
				}
			}
		}

		private void _WriteItem (string item)
		{
			item = item ?? string.Empty;
			
			// Implement special handling for values that contain comma or quote
			// Enclose in quotes and double up any double quotes
			if (item.IndexOfAny(_escapedChars) == -1)
			{
				_writer.Write(item);
			}
			else
			{	
				_writer.Write('"');
				_writer.Write(item.Replace("\"", "\"\""));
				_writer.Write('"');
			}
		}

		private readonly StreamWriter _writer;
		private readonly char[] _escapedChars = new char[] { '"', ',' };
	}
}