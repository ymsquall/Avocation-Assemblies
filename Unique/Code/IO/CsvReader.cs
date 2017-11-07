
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
	public class CsvReader: IDisposable
	{
		public CsvReader (Stream stream)
		{
			_reader = new StreamReader(stream);
		}

		public void Dispose ()
		{
			_reader.Close();
		}

		public bool ReadRow (ArrayList items, bool autoClearItems)
		{
			if (null == items)
			{
				return false;
			}

			var lineText = _reader.ReadLine();

			if (string.IsNullOrEmpty(lineText))
			{
				return false;
			}

			int pos  = 0;

			if (autoClearItems)
			{
				items.Clear();
			}

			while (pos < lineText.Length)
			{
				string item;
				
				// Special handling for quoted field
				if (lineText[pos] == '"')
				{
					// Skip initial quote
					pos++;
					
					// Parse quoted value
					int start = pos;
					while (pos < lineText.Length)
					{
						// Test for quote character
						if (lineText[pos] == '"')
						{
							// Found one
							pos++;
							
							// If two quotes together, keep one, otherwise, indicates end of value
							if (pos >= lineText.Length || lineText[pos] != '"')
							{
								pos--;
								break;
							}
						}

						pos++;
					}

					item = lineText.Substring(start, pos - start);
					item = item.Replace("\"\"", "\"");
				}
				else
				{
					// Parse unquoted value
					int start = pos;
					while (pos < lineText.Length && lineText[pos] != ',')
					{
						pos++;
					}

					item = lineText.Substring(start, pos - start);
				}

				items.Add(item);

				// Eat up to and including next comma
				while (pos < lineText.Length && lineText[pos] != ',')
				{
					pos++;
				}

				if (pos < lineText.Length)
				{
					pos++;
				}
			}

			return true;
		}

		private readonly StreamReader _reader;
	}
}