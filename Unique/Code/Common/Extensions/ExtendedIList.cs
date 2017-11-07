
/********************************************************************
created:    2015-10-20
author:     lixianmin

Copyright (C) - All Rights Reserved
*********************************************************************/
using System;
using System.Collections;
using System.Collections.Generic;

namespace Unique
{
    [Obfuscators.ObfuscatorIgnore]
    public static class ExtendedIList
    {   
        public static int MoveToEx (this IList srcList, IList destList)
        {
			if (null != srcList && null != destList)
			{
				var count = srcList.Count;
				for (int i= 0; i< count; ++i)
				{
					var item = srcList[i];
					destList.Add(item);
				}

				srcList.Clear();
				return count;
			}

			return 0;
		}

		public static int MoveToEx (this IList srcList, IList destList, object locker)
		{
			if (null != srcList && null != destList && null != locker)
			{
				lock (locker)
				{
					var count = srcList.Count;
					for (int i= 0; i< count; ++i)
					{
						var item = srcList[i];
						destList.Add(item);
					}
					
					srcList.Clear();
					return count;
				}
			}

			return 0;
		}

		public static Type GetElementTypeEx (this IList list)
		{
			if (null != list)
			{
				var listType = list.GetType();

				if (list is Array)
				{
					var elementType = listType.GetElementType();
					return elementType;
				}
				else if (listType.IsGenericType)
				{
					var genericTypeDefinition = listType.GetGenericTypeDefinition();
					if (typeof(List<>) == genericTypeDefinition)
					{
						var elementType = listType.GetGenericArguments() [0];
						return elementType;
					}
				}
			}

			return null;
		}
    }
}