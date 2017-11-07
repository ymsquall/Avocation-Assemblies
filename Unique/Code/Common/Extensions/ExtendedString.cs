
/*********************************************************************
created:	2014-04-25
author:		lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Globalization;

namespace Unique
{
    [Obfuscators.ObfuscatorIgnore]
    public static class ExtendedString
	{
		public static bool IsNullOrEmptyEx (this string text)
		{
			return string.IsNullOrEmpty(text);
		}

		public static bool StartsWithEx (this string text, string candidate, CompareOptions options)
		{
			if (null != text && null != candidate)
			{
				return _currentCompareInfo.IsPrefix(text, candidate, options);
			}
			
			return false;
		}

		public static bool StartsWithEx (this string text, string[] candidates, CompareOptions options)
		{
			if (null != text && null != candidates && candidates.Length > 0)
			{
				foreach (var item in candidates)
				{
					if (text.StartsWithEx(item, options))
					{
						return true;
					}
				}
			}
			
			return false;
		}

		public static bool EndsWithEx (this string text, string candidate, CompareOptions options)
		{
			if (null != text && null != candidate)
			{
				return _currentCompareInfo.IsSuffix(text, candidate, options);
			}
			
			return false;
		}

		public static bool EndsWithEx (this string text, string[] candidates, CompareOptions options)
		{
			if (null != text && null != candidates && candidates.Length > 0)
			{
				foreach (var item in candidates)
				{
					if (text.EndsWithEx(item, options))
					{
						return true;
					}
				}
			}
		
			return false;
		}

        public static string JoinEx (this string separator, IEnumerable collection)
        {
			if (null == separator || null == collection)
			{
				return string.Empty;
			}
			
			StringBuilder sbText= null;
			var iter = collection.GetEnumerator();
			
			while (iter.MoveNext())
			{
				var item = iter.Current;
				if (null != item)
				{
					sbText = new StringBuilder(128);
					sbText.Append(item);
					break;
				}
			}
			
			while (iter.MoveNext())
			{
				var item = iter.Current;
				if (null != item)
				{
					sbText.Append(separator);
					sbText.Append(item);
				}
			}
			
			return null != sbText ? sbText.ToString() : string.Empty;
        }

		public static string JoinEx<T> (this string separator
		                                , IEnumerable<T> collection
		                                , Func<T, string> processor)
		{
			if (null == separator || null == collection)
			{
				return string.Empty;
			}

			StringBuilder sbText= null;
			var iter = collection.GetEnumerator();

			while (iter.MoveNext())
			{
				var item = iter.Current;
				var result = processor(item);
				if (null != result)
				{
					sbText = new StringBuilder(128);
					sbText.Append(result);
					break;
				}
			}
			
			while (iter.MoveNext())
			{
				var item = iter.Current;
				var result = processor(item);
				if (null != result)
				{
					sbText.Append(separator);
					sbText.Append(result);
				}
			}
			
			return null != sbText ? sbText.ToString() : string.Empty;
		}

        public static int ReversedCompareToEx (this string lhs, string rhs, int deltaIndex= 0)
        {
            if (object.ReferenceEquals(lhs, rhs))
            {
                return 0;
            }
            else if (null == lhs)
            {
                return -1;
            }
            else if (null == rhs)
            {
                return 1;
            }

            var count = lhs.Length;
            if (count < rhs.Length)
            {
                return -1;
            }
            else if (count > rhs.Length)
            {
                return 1;
            }

            for (int i= count - deltaIndex - 1; i>= 0; --i)
            {
                var a = lhs[i];
                var b = rhs[i];
                if (a < b)
                {
                    return -1;
                }
                else if (a > b)
                {
                    return 1;
                }
            }
            
            return 0;
        }

		private static readonly CompareInfo _currentCompareInfo = CultureInfo.CurrentCulture.CompareInfo;
	}
}